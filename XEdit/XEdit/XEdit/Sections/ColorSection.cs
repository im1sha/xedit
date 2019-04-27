using SkiaBase;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using XEdit.Interaction;

namespace XEdit.Sections
{
    public class ColorSection : _CoreSection
    {
        public override string Name => "Color";

        public ColorSection() : base()
        {
            Handlers.Add(new PastelColor());
            Handlers.Add(new GrayScaleColor());
            SelectedHandler = Handlers[0];
        }

        private class MainCrop
        {

        }

        public class PastelColor : _CoreHandler
        {
            public override string Name => "Pastel";
            public override Action<object> SelectAction(object target, EventArgs args)
            {
                return (obj) => { AddSkCanvasAsChild(target, args); };
            }

            private void AddSkCanvasAsChild(object target, EventArgs args)
            {
                SKCanvasView canvasView = new SKCanvasView();

                canvasView.PaintSurface += OnCanvasViewPaintSurface;

                if (target is Xamarin.Forms.Layout<Xamarin.Forms.View>)
                {
                    (target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Add(canvasView);
                }

                canvasView.InvalidateSurface();

            }

            private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                using (SKPaint paint = new SKPaint())
                {
                    paint.ColorFilter =
                     SKColorFilter.CreateColorMatrix(new float[]
                     {
                                         0.75f, 0.25f, 0.25f, 0, 0,
                                         0.25f, 0.75f, 0.25f, 0, 0,
                                         0.25f, 0.25f, 0.75f, 0, 0,
                                         0, 0, 0, 1, 0
                     });

                    canvas.DrawBitmap(_ViewFunctionality.ResourceBitmap, info.Rect, BitmapStretch.Uniform, paint: paint);
                }

            }
        }

        public class GrayScaleColor : _CoreHandler
        {
            public override string Name => "Gray Scale";

            public override Action<object> SelectAction(object target, EventArgs args)
            {
                return (obj) => { AddSkCanvasAsChild(target, args); };
            }

            private void AddSkCanvasAsChild(object target, EventArgs args)
            {
                SKCanvasView canvasView = new SKCanvasView();

                canvasView.PaintSurface += OnCanvasViewPaintSurface;

                if (target is Xamarin.Forms.Layout<Xamarin.Forms.View>)
                {
                    (target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Add(canvasView);
                }

                canvasView.InvalidateSurface();

            }

            private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                using (SKPaint paint = new SKPaint())
                {
                    paint.ColorFilter =
                    paint.ColorFilter =
                        SKColorFilter.CreateColorMatrix(new float[]
                        {
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0.21f, 0.72f, 0.07f, 0, 0,
                        0,     0,     0,     1, 0
                        });


                    canvas.DrawBitmap(_ViewFunctionality.ResourceBitmap, info.Rect, BitmapStretch.Uniform, paint: paint);
                }

            }
        }
    }
}


