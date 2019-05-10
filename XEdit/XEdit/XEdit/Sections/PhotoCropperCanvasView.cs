using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using XEdit.TouchTracking;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class PhotoCropperCanvasView : SKCanvasView
    {
        const int CORNER = 50;      // pixel length of cropper corner
        const int RADIUS = 100;     // pixel radius of touch hit-test

        SKBitmap _bitmap;
        CroppingRectangle _croppingRect;
        SKMatrix _inverseBitmapMatrix;

        struct TouchPoint
        {
            public int CornerIndex { set; get; }
            public SKPoint Offset { set; get; }
        }

        Dictionary<long, TouchPoint> touchPoints = new Dictionary<long, TouchPoint>();

        // Drawing objects
        readonly SKPaint _cornerStroke = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.DarkGray,
            StrokeWidth = 10
        };
        readonly SKPaint _edgeStroke = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 1
        };

        public PhotoCropperCanvasView(TouchWorker touchWorker, SKBitmap bitmap, float? aspectRatio = null)
        {
            _bitmap = bitmap;
            SKRect bitmapRect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
            _croppingRect = new CroppingRectangle(bitmapRect, aspectRatio);
            touchWorker.SetUpdateHandler(OnTouchEffectTouchAction);
        }

        public SKBitmap CroppedBitmap
        {
            get
            {
                SKRect cropRect = _croppingRect.Rect;
                SKBitmap croppedBitmap = new SKBitmap((int)cropRect.Width, 
                                                      (int)cropRect.Height);
                SKRect dest = new SKRect(0, 0, cropRect.Width, cropRect.Height);
                SKRect source = new SKRect(cropRect.Left, cropRect.Top, 
                                           cropRect.Right, cropRect.Bottom);

                using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                {
                    canvas.DrawBitmap(_bitmap, source, dest);
                }

                return croppedBitmap;
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
        {
            base.OnPaintSurface(args);

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            // Calculate rectangle for displaying bitmap 
            float scale = Math.Min((float)info.Width / _bitmap.Width, (float)info.Height / _bitmap.Height);
            float x = (info.Width - scale * _bitmap.Width) / 2;
            float y = (info.Height - scale * _bitmap.Height) / 2;
            SKRect bitmapRect = new SKRect(x, y, x + scale * _bitmap.Width, y + scale * _bitmap.Height);
            canvas.DrawBitmap(_bitmap, bitmapRect);

            // Calculate a matrix transform for displaying the cropping rectangle
            SKMatrix bitmapScaleMatrix = SKMatrix.MakeIdentity();
            bitmapScaleMatrix.SetScaleTranslate(scale, scale, x, y);

            // Display rectangle
            SKRect scaledCropRect = bitmapScaleMatrix.MapRect(_croppingRect.Rect);
            canvas.DrawRect(scaledCropRect, _edgeStroke);

            // Display heavier corners
            using (SKPath path = new SKPath())
            {
                path.MoveTo(scaledCropRect.Left, scaledCropRect.Top + CORNER);
                path.LineTo(scaledCropRect.Left, scaledCropRect.Top);
                path.LineTo(scaledCropRect.Left + CORNER, scaledCropRect.Top);

                path.MoveTo(scaledCropRect.Right - CORNER, scaledCropRect.Top);
                path.LineTo(scaledCropRect.Right, scaledCropRect.Top);
                path.LineTo(scaledCropRect.Right, scaledCropRect.Top + CORNER);

                path.MoveTo(scaledCropRect.Right, scaledCropRect.Bottom - CORNER);
                path.LineTo(scaledCropRect.Right, scaledCropRect.Bottom);
                path.LineTo(scaledCropRect.Right - CORNER, scaledCropRect.Bottom);

                path.MoveTo(scaledCropRect.Left + CORNER, scaledCropRect.Bottom);
                path.LineTo(scaledCropRect.Left, scaledCropRect.Bottom);
                path.LineTo(scaledCropRect.Left, scaledCropRect.Bottom - CORNER);

                canvas.DrawPath(path, _cornerStroke);
            }

            // Invert the transform for touch tracking
            bitmapScaleMatrix.TryInvert(out _inverseBitmapMatrix);
        }

        void OnTouchEffectTouchAction(object sender, TouchActionEventArgs args)
        {
            SKPoint pixelLocation = ConvertToPixel(args.Location);
            SKPoint bitmapLocation = _inverseBitmapMatrix.MapPoint(pixelLocation);

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    // Convert radius to bitmap/cropping scale
                    float radius = _inverseBitmapMatrix.ScaleX * RADIUS;

                    // Find corner that the finger is touching
                    int cornerIndex = _croppingRect.HitTest(bitmapLocation, radius);

                    if (cornerIndex != -1 && !touchPoints.ContainsKey(args.Id))
                    {
                        TouchPoint touchPoint = new TouchPoint
                        {
                            CornerIndex = cornerIndex,
                            Offset = bitmapLocation - _croppingRect.Corners[cornerIndex]
                        };

                        touchPoints.Add(args.Id, touchPoint);
                    }
                    break;

                case TouchActionType.Moved:
                    if (touchPoints.ContainsKey(args.Id))
                    {
                        TouchPoint touchPoint = touchPoints[args.Id];
                        _croppingRect.MoveCorner(touchPoint.CornerIndex, 
                                                bitmapLocation - touchPoint.Offset);
                        InvalidateSurface();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (touchPoints.ContainsKey(args.Id))
                    {
                        touchPoints.Remove(args.Id);
                    }
                    break;
            }
        }

        SKPoint ConvertToPixel(Xamarin.Forms.Point pt)
        {
            return new SKPoint((float)(CanvasSize.Width * pt.X / Width),
                                (float)(CanvasSize.Height * pt.Y / Height));
        }
    }
}
