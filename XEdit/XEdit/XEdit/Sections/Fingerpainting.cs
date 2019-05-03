using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.TouchTracking;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class FingerPainting : BaseSection
    {
        readonly MainViewModel _mainVM;

        public override string Name { get; } = "Paint";

        public FingerPainting(MainViewModel vm)
        {
            _mainVM = vm;

            Handlers = new ObservableCollection<VisualHandler>();
            for (int i = 0; i < _colors.Length; i++)
            {
                Handlers.Add(CreateHandler(_colors[i].color, _colors[i].name));
            }

            _handlerToColorDictionary = new Dictionary<VisualHandler, (SKColor, string)>();
            for (int i = 0; i < Handlers.Count; i++)
            {
                _handlerToColorDictionary.Add(Handlers[i], _colors[i]);
            }
        }

        private VisualHandler CreateHandler(SKColor color, string name)
        {
            return new VisualHandler(
                name: name,
                url: null,
                perform: () =>
                {
                    _inProgressPathsInPixels = new Dictionary<long, SKPath>();
                    _inProgressPathsInPoints = new Dictionary<long, SKPath>();
                    //_completedPaths = new List<SKPath>();
                    _paint = GetSkPaint(color);

                    _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
                    _mainVM.TouchWorker.SetUpdateHandler(OnTouchEffectAction);
                }
                );
        }

        private (SKColor color, string name) GetDefaultColor()
        {
            return (SKColors.Cyan, "Cyan");
        }

        private (SKColor color, string name)[] _colors = {
                (SKColors.Cyan, "Cyan"),
                (SKColors.Magenta, "Magenta"),
                (SKColors.Yellow, "Yellow"),
                (SKColors.Red, "Red"),
                (SKColors.Green, "Green"),
                (SKColors.Blue, "Blue"),
            };

        private Dictionary<VisualHandler, (SKColor color, string name)> _handlerToColorDictionary;
        private SKPaint GetSkPaint(SKColor c, float width = 10)
        {
            return new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                Color = c,
                StrokeWidth = width,
            };
        }

        private Dictionary<long, SKPath> _inProgressPathsInPixels;
        private Dictionary<long, SKPath> _inProgressPathsInPoints;
        //private List<SKPath> _completedPaths;
        private SKPath _completedPathInPixels;
        private SKPath _completedPathInPoints;
        private SKPaint _paint;
        private volatile bool _newCompletedPath = false;

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        SKPath pathInPixels = new SKPath();
                        pathInPixels.MoveTo(ConvertToPixel(args.Location));
                        _inProgressPathsInPixels.Add(args.Id, pathInPixels);

                        //
                        SKPath pathInPoints = new SKPath();
                        pathInPoints.MoveTo(new SKPoint((float) args.Location.X*2, (float) args.Location.Y * 2));
                        _inProgressPathsInPoints.Add(args.Id, pathInPoints);
                    }
                    break;
                case TouchActionType.Moved:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        SKPath path = _inProgressPathsInPixels[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));

                        //
                        SKPath pathInPoints = _inProgressPathsInPoints[args.Id];
                        pathInPoints.LineTo(new SKPoint((float)args.Location.X * 2, (float)args.Location.Y * 2));
                    }
                    break;
                case TouchActionType.Released:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        _newCompletedPath = true;

                        _completedPathInPixels = _inProgressPathsInPixels[args.Id];
                        _inProgressPathsInPixels.Remove(args.Id);

                        //
                        _completedPathInPoints = _inProgressPathsInPoints[args.Id];
                        _inProgressPathsInPoints.Remove(args.Id);
                    }
                    break;
                case TouchActionType.Cancelled:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        _inProgressPathsInPixels.Remove(args.Id);
                        _inProgressPathsInPoints.Remove(args.Id);
                    }
                    break;
            }

            if (_newCompletedPath)
            {
                var bitmap = _mainVM.ImageWorker.Image;
                _mainVM.ImageWorker.AddImageState(bitmap);

                SKBitmap newBitmap = new SKBitmap(bitmap.Info);
                using (SKCanvas canvas = new SKCanvas(newBitmap))
                {
                    canvas.Clear();
                    canvas.DrawBitmap(bitmap, new SKPoint());
                    canvas.DrawPath(_completedPathInPoints, _paint);                    
                }

                _mainVM.ImageWorker.Image = newBitmap;
                _newCompletedPath = false;
            }

            _mainVM.CanvasViewWorker.Invalidate();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            using (canvas)
            {
                canvas.Clear();
                canvas.DrawBitmap(_mainVM.ImageWorker.Image, info.Rect, BitmapStretch.Uniform);

                foreach (SKPath path in _inProgressPathsInPixels.Values)
                {
                    canvas.DrawPath(path, _paint);
                }
            }
        }

        private SKPoint ConvertToPixel(Point point)
        {
            return new SKPoint(
                (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeWidth * point.X /
                _mainVM.CanvasViewWorker.ViewWidth),
                (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeHeight * point.Y /
                _mainVM.CanvasViewWorker.ViewHeight)
                );
        }


    }
}
