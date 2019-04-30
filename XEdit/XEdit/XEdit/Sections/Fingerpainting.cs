﻿using SkiaSharp;
using SkiaSharp.Views.Forms;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Xamarin.Forms;

using XEdit.Core;
using XEdit.Extensions;
using XEdit.TouchTracking;

namespace XEdit.Sections
{
    class FingerPainting : CoreSection
    {
        public override string Name { get; } = "Finger Painting";

        public override Handler SelectedHandler
        {
            get
            {
                return _selectedHandler;
            }
            set
            {
                if (_selectedHandler != value)
                {
                    _selectedHandler?.Exit(null);
                    _selectedHandler = value;
                    OnPropertyChanged();
                    _selectedHandler.Perform(null);
                }
            }
        }

        public override Command LeaveCommand
        {
            get => new Command(obj => {
                SelectedHandler?.Exit(null);
                _selectedHandler = null;
            });
        }

        public FingerPainting()
        {
            Handlers = new ObservableCollection<Handler>();
            for (int i = 0; i < _colors.Length; i++)
            {
                Handlers.Add(CreateHandler(_colors[i].color, _colors[i].name));
            }

            _handlerToColorDictionary = new Dictionary<Handler, (SKColor, string)>();
            for (int i = 0; i < Handlers.Count; i++)
            {
                _handlerToColorDictionary.Add(Handlers[i], _colors[i]);
            }
        }

        Handler CreateHandler(SKColor color, string name)
        {
            return new Handler(
                name,
                null,
                (obj) => {                  
                    _inProgressPaths = new Dictionary<long, SKPath>();
                    _completedPaths = new List<SKPath>();
                    _paint = GetSkPaint(color);

                    AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler(OnCanvasViewPaintSurface);
                    AppDispatcher.Get<ImageManager>().SetTouchEffectUpdateHandler(OnTouchEffectAction);
                },
                (obj) => {
                    AppDispatcher.Get<ImageManager>().SetCanvasUpdateHandler();
                    AppDispatcher.Get<ImageManager>().SetTouchEffectUpdateHandler();
                });
        }

        (SKColor color, string name) GetDefaultColor()
        {
            return (SKColors.Cyan, "Cyan");
        }

        (SKColor color, string name)[] _colors = {
                (SKColors.Cyan, "Cyan"),
                (SKColors.Magenta, "Magenta"),
                (SKColors.Yellow, "Yellow"),
                (SKColors.Red, "Red"),
                (SKColors.Green, "Green"),
                (SKColors.Blue, "Blue"),
            };

        Dictionary<Handler, (SKColor color, string name)> _handlerToColorDictionary;
        SKPaint GetSkPaint(SKColor c, float width = 20) {
             return new SKPaint {
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                Color = c,
                StrokeWidth = width,
            };
        }

        //TouchAction="OnTouchEffectAction" 
        //Capture="True"

        Dictionary<long, SKPath> _inProgressPaths;
        List<SKPath> _completedPaths;
        SKPaint _paint;
             
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {         
            switch (args.Type)
            {
            case TouchActionType.Pressed:
                if (!_inProgressPaths.ContainsKey(args.Id))
                {
                    SKPath path = new SKPath();
                    path.MoveTo(ConvertToPixel(args.Location));
                    _inProgressPaths.Add(args.Id, path);
                }
                break;
            case TouchActionType.Moved:
                if (_inProgressPaths.ContainsKey(args.Id))
                {
                    SKPath path = _inProgressPaths[args.Id];
                    path.LineTo(ConvertToPixel(args.Location));
                }
                break;
            case TouchActionType.Released:
                if (_inProgressPaths.ContainsKey(args.Id))
                {
                    _completedPaths.Add(_inProgressPaths[args.Id]);
                    _inProgressPaths.Remove(args.Id);
                }
                break;
            case TouchActionType.Cancelled:
                if (_inProgressPaths.ContainsKey(args.Id))
                {
                    _inProgressPaths.Remove(args.Id);
                }
                break;
            }

            AppDispatcher.Get<ImageManager>().InvalidateCanvasView();
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            var img = AppDispatcher.Get<ImageManager>().CloneImage();

            using (canvas)
            using (SKPaint paint = new SKPaint())
            {
                canvas.Clear();
                canvas.DrawBitmap(img, info.Rect, BitmapStretch.Uniform);
                foreach (SKPath path in _completedPaths)
                {
                    canvas.DrawPath(path, _paint);
                }
                foreach (SKPath path in _inProgressPaths.Values)
                {
                    canvas.DrawPath(path, _paint);
                }
            }

            img = null;
            GC.Collect();
        }

        SKPoint ConvertToPixel(Point pt)
        {
            return new SKPoint(
                (float)(AppDispatcher.Get<ImageManager>().ViewCanvasSizeWidth * pt.X /
                AppDispatcher.Get<ImageManager>().ViewWidth),
                (float)(AppDispatcher.Get<ImageManager>().ViewCanvasSizeHeight * pt.Y /
                AppDispatcher.Get<ImageManager>().ViewHeight)
                );
        }        
    }
}