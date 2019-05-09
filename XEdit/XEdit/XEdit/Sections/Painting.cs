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
using System.Linq;

namespace XEdit.Sections
{
    class Painting : BaseSection
    {
        readonly MainViewModel _mainVM;

        public override string Name { get; } = "Paint";

        public Painting(MainViewModel vm)
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
                    _inProgressPathsOnImage = new Dictionary<long, SKPath>();
                    _paint = GetSkPaint(color);

                    _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
                    _mainVM.TouchWorker.SetUpdateHandler(OnTouchEffectAction);
                },
                close: () => {
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    _mainVM.TouchWorker.SetUpdateHandler();
                });
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
        private Dictionary<long, SKPath> _inProgressPathsOnImage;
        private SKPath _completedPathInPixels;
        private SKPath _completedPathOnImage;
        private SKPaint _paint;
        private volatile bool _isPathCompleted = false;

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            if (!(sender is Grid))
            {
                return;
            }
            var grid = (sender as Grid);

            SKSize canvasViewSize = (grid.Children.Where((i) =>
                i.GetType() == typeof(SKCanvasView)).
                    ToArray().FirstOrDefault() as SKCanvasView).CanvasSize;

            SKBitmap bitmap = _mainVM.ImageWorker.Image;

            SKRect rect;
            float scale; // determines image orientation
            (scale, rect) = SizeCalculator.GetScaleAndRect(canvasViewSize, bitmap);

            SKPoint location = ConvertToPixel(args.Location);

            if (location.X > rect.Right || location.X < rect.Left ||
                location.Y < rect.Top || location.Y > rect.Bottom)
            {
                args.Type = TouchActionType.Released;
            }
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    if (!_inProgressPathsInPixels.ContainsKey(args.Id))
                    {

                        SKPath pathInPixels = new SKPath();
                        pathInPixels.MoveTo(location);
                        _inProgressPathsInPixels.Add(args.Id, pathInPixels);

                        //
                        SKPath pathOnImage = new SKPath();
                        pathOnImage.MoveTo(ConvertToPositionOnImage(
                            location, 
                            rect,
                            bitmap.Height / rect.Height
                            ));

                        _inProgressPathsOnImage.Add(args.Id, pathOnImage);
                    }
                    break;
                case TouchActionType.Moved:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        SKPath pathInPixels = _inProgressPathsInPixels[args.Id];
                        pathInPixels.LineTo(location);

                        //
                        SKPath pathOnImage = _inProgressPathsOnImage[args.Id];
                        pathOnImage.LineTo(ConvertToPositionOnImage(
                            location,
                            rect,
                            scale
                            ));
                    }
                    break;
                case TouchActionType.Released:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        _isPathCompleted = true;

                        _completedPathInPixels = _inProgressPathsInPixels[args.Id];
                        _inProgressPathsInPixels.Remove(args.Id);

                        //
                        _completedPathOnImage = _inProgressPathsOnImage[args.Id];
                        _inProgressPathsOnImage.Remove(args.Id);
                    }
                    break;
                case TouchActionType.Cancelled:
                    if (_inProgressPathsInPixels.ContainsKey(args.Id))
                    {
                        _inProgressPathsInPixels.Remove(args.Id);

                        //
                        _inProgressPathsOnImage.Remove(args.Id);
                    }
                    break;
            }

            if (_isPathCompleted)
            {
                _mainVM.ImageWorker.AddImageState(bitmap);

                SKPaint paint = new SKPaint() {
                    Style = _paint.Style,
                    StrokeCap = _paint.StrokeCap,
                    StrokeJoin = _paint.StrokeJoin,
                    Color = _paint.Color,
                    StrokeWidth = _paint.StrokeWidth * scale,
                };

                SKBitmap newBitmap = new SKBitmap(bitmap.Info);

                using (SKCanvas canvas = new SKCanvas(newBitmap))
                using (paint)
                {
                    canvas.Clear();
                    canvas.DrawBitmap(bitmap, new SKPoint());
                    canvas.DrawPath(_completedPathOnImage, paint);
                }

                _mainVM.ImageWorker.Image = newBitmap;
                _isPathCompleted = false;
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

        /// <summary>
        /// Converts position of point on canva to position on image
        /// </summary>
        private SKPoint ConvertToPositionOnImage(SKPoint point, SKRect rect, 
            float bitmapSizeToRectSize)
        {
            return new SKPoint(((float)point.X - rect.Left) * bitmapSizeToRectSize,
                ((float)point.Y - rect.Top) * bitmapSizeToRectSize);
        }
    }
}
