using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using XEdit.Extensions;

namespace XEdit.ViewModels
{
    class CanvasViewWorker
    {       
        private static ImageWorker _imageWorker;

        private SKCanvasView _canvasView;

        public float ViewCanvasSizeWidth => _canvasView.CanvasSize.Width;
        public double ViewWidth => _canvasView.Width;
        public float ViewCanvasSizeHeight => _canvasView.CanvasSize.Height;
        public double ViewHeight => _canvasView.Height;

        private EventHandler<SKPaintSurfaceEventArgs> _previousUpdateHandler = 
            _standardUpdateHandler;

        private readonly static EventHandler<SKPaintSurfaceEventArgs> _standardUpdateHandler = 
            (sender, args) => {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                if (_imageWorker != null && _imageWorker.Image != null)
                {
                    canvas.DrawBitmap(_imageWorker.Image, info.Rect, BitmapStretch.Uniform);
                }
            };

        public CanvasViewWorker(SKCanvasView c, ImageWorker iw)
        {
            _imageWorker = iw;
            _canvasView = c;
            _canvasView.PaintSurface += _standardUpdateHandler;
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
            _canvasView.PaintSurface -= _previousUpdateHandler;
            _previousUpdateHandler = eh;
            _canvasView.PaintSurface += eh;
        }

        public void InvalidateCanvasView()
        {
             _canvasView.InvalidateSurface();
        }
    }
}


