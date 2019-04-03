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
    public class RotateSection : CoreSection
    {
        public override string Name => "Rotate";

        public RotateSection() : base()
        {
            Handlers.Add(new FlipHorizontal());
            Handlers.Add(new FlipVertical());
            SelectedHandler = Handlers[0];
        }


        public class FlipHorizontal : CoreHandler
        {
            public override string Name => "Horizontal Flip";

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

                SKBitmap flippedBitmap = new SKBitmap(ViewFunctionality.ResourceBitmap.Width, ViewFunctionality.ResourceBitmap.Height);

                using (SKCanvas canvas = new SKCanvas(flippedBitmap))
                {
                    canvas.Clear();
                    canvas.Scale(1, -1, 0, ViewFunctionality.ResourceBitmap.Height / 2);
                    canvas.DrawBitmap(ViewFunctionality.ResourceBitmap, new SKPoint());
                }

                ViewFunctionality.SetBitmap(flippedBitmap);
                canvasView.InvalidateSurface();
            }

            void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();
                canvas.DrawBitmap(ViewFunctionality.ResourceBitmap, info.Rect, BitmapStretch.Uniform);
            }

        

        }

        public class FlipVertical : CoreHandler
        {
            public override string Name => "Vertical Flip";

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

                SKBitmap flippedBitmap = new SKBitmap(ViewFunctionality.ResourceBitmap.Width, ViewFunctionality.ResourceBitmap.Height);

                using (SKCanvas canvas = new SKCanvas(flippedBitmap))
                {
                    canvas.Clear();
                    canvas.Scale(-1, 1, ViewFunctionality.ResourceBitmap.Width / 2, 0);
                    canvas.DrawBitmap(ViewFunctionality.ResourceBitmap, new SKPoint());
                }


                ViewFunctionality.SetBitmap(flippedBitmap);
                canvasView.InvalidateSurface();

            }

            void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();
                canvas.DrawBitmap(ViewFunctionality.ResourceBitmap, info.Rect, BitmapStretch.Uniform);
            }

       
       
        }
    }
}


