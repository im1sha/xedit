﻿using SkiaBase;
using SkiaBase.Bitmaps;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using TouchTracking;
using XEdit.Interaction;

namespace XEdit.Sections
{
    public class CropSection : _CoreSection
    {
        public override string Name => "Crop";

        public CropSection() : base()
        {
            Handlers.Add(new FreeSizeCrop());
            SelectedHandler = Handlers[0];
        }

        public class MainCropper
        {
            public bool IsCroppingInProgress { get; set; } = false;

            private SKBitmap currentBitmap;
            private PhotoCropperCanvasView photoCropperView;

            private object target; // wrapper of canva at XAML

            public void RunCropping(object target)
            {
                photoCropperView = new PhotoCropperCanvasView(_ViewFunctionality.ResourceBitmap);
                _ViewFunctionality.AddNewCanvaAsChild(target, photoCropperView);
                this.target = target;
            }

            // target stored in local field
            public void OnApply()
            {
                currentBitmap = photoCropperView.CroppedBitmap;
                _ViewFunctionality.SetBitmap(currentBitmap);

                RedrawCanvas();
            }

        

            void RedrawCanvas()
            {
                SKCanvasView canvasView = new SKCanvasView();
                canvasView.PaintSurface += OnCanvasViewPaintSurface;
                photoCropperView.UnregisterEffects();
                _ViewFunctionality.AddNewCanvaAsChild(target, canvasView);
                canvasView.InvalidateSurface();
            }


            void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();
                canvas.DrawBitmap(currentBitmap, info.Rect, BitmapStretch.Uniform);

            }
        }

        /// <summary>
        /// Should call UregisterEffects()
        /// </summary>
        private class PhotoCropperCanvasView : SKCanvasView
        {
            const int CORNER = 50;      // pixel length of cropper corner
            const int RADIUS = 100;     // pixel radius of touch hit-test

            SKBitmap bitmap;
            CroppingRectangle croppingRect;
            SKMatrix inverseBitmapMatrix;

            // Touch tracking
            TouchEffect touchEffect = new TouchEffect();
            struct TouchPoint
            {
                public int CornerIndex { set; get; }
                public SKPoint Offset { set; get; }
            }

            Dictionary<long, TouchPoint> touchPoints = new Dictionary<long, TouchPoint>();

            // Drawing objects
            SKPaint cornerStroke = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 10
            };
            SKPaint edgeStroke = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.White,
                StrokeWidth = 2
            };

            public PhotoCropperCanvasView(SKBitmap bitmap, float? aspectRatio = null)
            {
                this.bitmap = bitmap;

                SKRect bitmapRect = new SKRect(0, 0, bitmap.Width, bitmap.Height);
                croppingRect = new CroppingRectangle(bitmapRect, aspectRatio);


                touchEffect.TouchAction += OnTouchEffectTouchAction;

            }

            public SKBitmap CroppedBitmap
            {
                get
                {
                    SKRect cropRect = croppingRect.Rect;
                    SKBitmap croppedBitmap = new SKBitmap((int)cropRect.Width,
                                                          (int)cropRect.Height);
                    SKRect dest = new SKRect(0, 0, cropRect.Width, cropRect.Height);
                    SKRect source = new SKRect(cropRect.Left, cropRect.Top,
                                               cropRect.Right, cropRect.Bottom);

                    using (SKCanvas canvas = new SKCanvas(croppedBitmap))
                    {
                        canvas.DrawBitmap(bitmap, source, dest);
                    }

                    return croppedBitmap;
                }
            }

            protected override void OnParentSet()
            {
                base.OnParentSet();

                // Attach TouchEffect to parent view

                Parent?.Effects?.Add(touchEffect);
            }

            protected override void OnPaintSurface(SKPaintSurfaceEventArgs args)
            {
                base.OnPaintSurface(args);

                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear(SKColors.Gray);

                // Calculate rectangle for displaying bitmap

                float scale = Math.Min((float)info.Width / bitmap.Width, (float)info.Height / bitmap.Height);
                float x = (info.Width - scale * bitmap.Width) / 2;
                float y = (info.Height - scale * bitmap.Height) / 2;
                SKRect bitmapRect = new SKRect(x, y, x + scale * bitmap.Width, y + scale * bitmap.Height);
                canvas.DrawBitmap(bitmap, bitmapRect);

                //Calculate a matrix transform for displaying the cropping rectangle

                SKMatrix bitmapScaleMatrix = SKMatrix.MakeIdentity();
                bitmapScaleMatrix.SetScaleTranslate(scale, scale, x, y);

                //Display rectangle
                SKRect scaledCropRect = bitmapScaleMatrix.MapRect(croppingRect.Rect);
                canvas.DrawRect(scaledCropRect, edgeStroke);

                //Display heavier corners
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

                    canvas.DrawPath(path, cornerStroke);
                }

                //    Invert the transform for touch tracking

                bitmapScaleMatrix.TryInvert(out inverseBitmapMatrix);
            }

            void OnTouchEffectTouchAction(object sender, TouchActionEventArgs args)
            {
                SKPoint pixelLocation = ConvertToPixel(args.Location);
                SKPoint bitmapLocation = inverseBitmapMatrix.MapPoint(pixelLocation);

                switch (args.Type)
                {
                    case TouchActionType.Pressed:
                        //  Convert radius to bitmap/ cropping scale
                        float radius = inverseBitmapMatrix.ScaleX * RADIUS;

                        // Find corner that the finger is touching
                        int cornerIndex = croppingRect.HitTest(bitmapLocation, radius);

                        if (cornerIndex != -1 && !touchPoints.ContainsKey(args.Id))
                        {
                            TouchPoint touchPoint = new TouchPoint
                            {
                                CornerIndex = cornerIndex,
                                Offset = bitmapLocation - croppingRect.Corners[cornerIndex]
                            };

                            touchPoints.Add(args.Id, touchPoint);
                        }
                        break;

                    case TouchActionType.Moved:
                        if (touchPoints.ContainsKey(args.Id))
                        {
                            TouchPoint touchPoint = touchPoints[args.Id];
                            croppingRect.MoveCorner(touchPoint.CornerIndex,
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
                return new SKPoint(
                    (float)(CanvasSize.Width * pt.X / Width),
                    (float)(CanvasSize.Height * pt.Y / Height));
            }

            public void UnregisterEffects() {
                touchEffect.TouchAction -= OnTouchEffectTouchAction;
                Parent?.Effects?.Remove(touchEffect);
            }
        }   

        public class FreeSizeCrop : _CoreHandler
        {
            private MainCropper mainCropperInstance = new MainCropper();
            public override string Name => "Free size";
            public FreeSizeCrop() : base()
            {
            }

            public override Action<object> SelectAction(object target, EventArgs args)
            {
                if (!mainCropperInstance.IsCroppingInProgress)
                {
                    mainCropperInstance = new MainCropper();

                    return (obj) => {
                        mainCropperInstance.RunCropping(target);
                        mainCropperInstance.IsCroppingInProgress = true;
                    };
                }

                return (obj) => {
                    mainCropperInstance.OnApply();
                    mainCropperInstance.IsCroppingInProgress = false;

                };
                
            }

            public override Action<object> CancelAction(object target, EventArgs args)
            {
                //to replace

                if (mainCropperInstance.IsCroppingInProgress)
                {
                    return (obj) => {
                        mainCropperInstance.OnApply();
                        mainCropperInstance.IsCroppingInProgress = false;
                    };
                }

                return (obj) => { };
            }

        }

        public class OneToOneCrop : _CoreHandler
        {
            private MainCropper cropperInstance;
            public override string Name => "1 : 1";
            public OneToOneCrop() : base()
            {
            }

            public override Action<object> SelectAction(object target, EventArgs args)
            {
                return (obj) => { };
            }

            public override Action<object> CancelAction(object target, EventArgs args)
            {
                return (obj) => { };
            }
        }
    }
}
