using System;
using System.IO;
using System.Net.Http;
using System.Reflection;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaBase;

namespace XEdit.Core
{
    public class PictureLoader : ContentPage
    {
        SKCanvasView canvasView = new SKCanvasView();
        SKBitmap libraryBitmap;


        public PictureLoader()
        {
            NavigationPage.SetHasNavigationBar(this, false);


            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            Content = canvasView;


            // Add tap gesture recognizer
            TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();

            tapRecognizer.Tapped += async (sender, args) =>
            {
                IPhotoLibrary photoLibrary = DependencyService.Get<IPhotoLibrary>();

                using (Stream stream = await photoLibrary.PickPhotoAsync())
                {
                    if (stream != null)
                    {
                        libraryBitmap = SKBitmap.Decode(stream);
                        canvasView.InvalidateSurface();
                    }
                }

            };

            canvasView.GestureRecognizers.Add(tapRecognizer);
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (libraryBitmap != null)
            {
                using (SKPaint paint = new SKPaint())
                {
                    canvas.DrawBitmap(libraryBitmap, info.Rect, BitmapStretch.Uniform, paint: paint);
                }
            }
        }
    }
}