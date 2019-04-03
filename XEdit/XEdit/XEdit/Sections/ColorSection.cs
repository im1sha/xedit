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
    public class ColorSection : CoreSection
    {
        public override string Name => "Color";


        public ColorSection() : base()
        {
            Handlers.Add(new GreenColor());
            Handlers.Add(new YellowCollor());
            SelectedHandler = Handlers[0];
        }

        private class MainCrop
        {

        }

        public class GreenColor : CoreHandler
        {
            public override string Name => "Green";

            public override Action<object> GetAction(object target, EventArgs args)
            {
                return (obj) => { AddSkCanvasAsChild(target, args); };
            }

            private SKBitmap resourceBitmap;

            private void AddSkCanvasAsChild(object target, EventArgs args)
            {
                SKCanvasView canvasView = new SKCanvasView();

                canvasView.PaintSurface += OnCanvasViewPaintSurface;

                if (target is Xamarin.Forms.Layout<Xamarin.Forms.View>)
                {
                    (target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Add(canvasView);

                    // Load resource bitmap
                    string resourceID = "XEdit.Media.monkey.png";
                    Assembly assembly = GetType().GetTypeInfo().Assembly;

                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    {
                        resourceBitmap = SKBitmap.Decode(stream);
                    }
                }
            }

            private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                if (resourceBitmap != null)
                {
                    SKRect pictureFrame = SKRect.Create(0, 0, info.Width, info.Height);
                    SKRect dest = pictureFrame.AspectFit(new SKSize(resourceBitmap.Width, resourceBitmap.Height));
                    canvas.DrawBitmap(resourceBitmap, dest, new SKPaint() { FilterQuality = SKFilterQuality.High });
                }
            }
        }

        public class YellowCollor : CoreHandler
        {
            public override string Name => "Yellow";

            public override Action<object> GetAction(object target, EventArgs args)
            {
                return (obj) => { AddSkCanvasAsChild(target, args); };
            }

            private SKBitmap resourceBitmap;

            private void AddSkCanvasAsChild(object target, EventArgs args)
            {
                SKCanvasView canvasView = new SKCanvasView();

                canvasView.PaintSurface += OnCanvasViewPaintSurface;

                if (target is Xamarin.Forms.Layout<Xamarin.Forms.View>)
                {
                    (target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Add(canvasView);

                    // Load resource bitmap
                    string resourceID = "XEdit.Media.MonkeyFace.png";
                    Assembly assembly = GetType().GetTypeInfo().Assembly;

                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    {
                        resourceBitmap = SKBitmap.Decode(stream);
                    }
                }
            }

            private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                if (resourceBitmap != null)
                {
                    SKRect pictureFrame = SKRect.Create(0, 0, info.Width, info.Height);
                    SKRect dest = pictureFrame.AspectFit(new SKSize(resourceBitmap.Width, resourceBitmap.Height));
                    canvas.DrawBitmap(resourceBitmap, dest, new SKPaint() { FilterQuality = SKFilterQuality.High });
                }
            }
        }
    }
}
