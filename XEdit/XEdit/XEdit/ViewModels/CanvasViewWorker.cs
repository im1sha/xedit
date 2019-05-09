using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;

namespace XEdit.ViewModels
{
    public class CanvasViewWorker
    {
        private Layout<View> _container;
        private static ImageWorker _imageWorker;

        public SKCanvasView CanvasView { get; private set; }

        public float ViewCanvasSizeWidth => CanvasView.CanvasSize.Width;
        public double ViewWidth => CanvasView.Width;
        public float ViewCanvasSizeHeight => CanvasView.CanvasSize.Height;
        public double ViewHeight => CanvasView.Height;

        private EventHandler<SKPaintSurfaceEventArgs> _previousUpdateHandler = 
            _standardUpdateHandler;

        private readonly static EventHandler<SKPaintSurfaceEventArgs> _standardUpdateHandler = 
            (sender, args) => {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;
                using (canvas)
                {
                    canvas.Clear();

                    if (_imageWorker != null && _imageWorker.Image != null)
                    {
                        canvas.DrawBitmap(_imageWorker.Image, info.Rect, BitmapStretch.Uniform);
                    }
                }
            };

        public CanvasViewWorker(Layout<View> container, SKCanvasView c, ImageWorker iw)
        {
            _container = container;
            _imageWorker = iw;
            CanvasView = c;
            CanvasView.PaintSurface += _standardUpdateHandler;
        }

        public bool ChangeCanvas(SKCanvasView newCanvasView)
        {
            View itemToRemove = _container.Children.Where(i => i is SKCanvasView).FirstOrDefault();

            if (itemToRemove != null)
            {
                _container.Children.Remove(itemToRemove);
                _container.Children.Add(newCanvasView);
                CanvasView = newCanvasView;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Sets rendering handler for SKCanvasView
        /// </summary>
        /// <param name="eh">If null passed then it should set standard handler</param>
        public void SetUpdateHandler(EventHandler<SKPaintSurfaceEventArgs> eh = null)
        {
            if (eh == null)
            {
                eh = _standardUpdateHandler;
            }
            CanvasView.PaintSurface -= _previousUpdateHandler;
            _previousUpdateHandler = eh;
            CanvasView.PaintSurface += eh;
        }

        public void Invalidate()
        {
             CanvasView.InvalidateSurface();
        }
    }
}


