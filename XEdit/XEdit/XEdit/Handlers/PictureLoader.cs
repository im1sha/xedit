using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XEdit.Interaction;

namespace XEdit.Handlers
{
    public class PictureLoader : CoreSection
    {
        private readonly PictureUploader uploader = new PictureUploader();

        public override string Name => "Load picture";
      
        public PictureLoader() : base()
        {
            Handlers.Add(uploader);
            SelectedHandler = Handlers[0];
        }

        /// <summary>
        /// 
        /// </summary>
        public class PictureUploader : CoreHandler 
        {
            public override string Name => "Upload";

            public override Action<object> SelectAction(object target, EventArgs args)
            {
                return (obj) => { AddSkCanvasAsChild(target, args); };
            }

            private void AddSkCanvasAsChild(object target, EventArgs args)
            {
                SKCanvasView canvasView = new SKCanvasView();

                canvasView.PaintSurface += OnCanvasViewPaintSurface;

                if (ViewFunctionality.IsTargetSuitable( target ))
                {
                    ViewFunctionality.AddNewCanvaAsChild(target, canvasView);

                    // Load resource bitmap
                    string resourceID = "XEdit.Media.usa.png";
                    Assembly assembly = GetType().GetTypeInfo().Assembly;

                    using (Stream stream = assembly.GetManifestResourceStream(resourceID))
                    {
                        ViewFunctionality.SetBitmap(SKBitmap.Decode(stream));
                    }
                }
            }

            //// Add tap gesture recognizer
            //TapGestureRecognizer tapRecognizer = new TapGestureRecognizer();
            //tapRecognizer.Tapped += async (sender, args) =>
            //{
            //    // Load bitmap from photo library
            //    XEdit.Core.Data.IPhotoLibrary photoLibrary
            //        = DependencyService.Get<XEdit.Core.Data.IPhotoLibrary>();
            //    using (Stream stream = await photoLibrary.PickPhotoAsync())
            //    {
            //        if (stream != null)
            //        {
            //            libraryBitmap = SKBitmap.Decode(stream);
            //            canvasView.InvalidateSurface();
            //        }
            //    }
            //};
            //canvasView.GestureRecognizers.Add(tapRecognizer);


            //protected override async void OnAppearing()
            //{
            //    base.OnAppearing();
            //    // Load web bitmap.
            //    string url = "https://developer.xamarin.com/demo/IMG_3256.JPG?width=480";
            //    try
            //    {
            //        using (Stream stream = await httpClient.GetStreamAsync(url))
            //        using (MemoryStream memStream = new MemoryStream())
            //        {
            //            await stream.CopyToAsync(memStream);
            //            memStream.Seek(0, SeekOrigin.Begin);
            //            webBitmap = SKBitmap.Decode(memStream);
            //            canvasView.InvalidateSurface();
            //        }
            //    }
            //    catch
            //    {
            //    }
            //}

            private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
            {
                SKImageInfo info = args.Info;
                SKSurface surface = args.Surface;
                SKCanvas canvas = surface.Canvas;

                canvas.Clear();

                if (ViewFunctionality.ResourceBitmap != null)
                {
                    SKRect pictureFrame = SKRect.Create(0, 0, info.Width, info.Height);
                    SKRect dest = pictureFrame.AspectFit(new SKSize(ViewFunctionality.ResourceBitmap.Width, ViewFunctionality.ResourceBitmap.Height));
                    canvas.DrawBitmap(ViewFunctionality.ResourceBitmap, dest, new SKPaint() { FilterQuality = SKFilterQuality.High });
                }
            }

            // SKBitmap libraryBitmap;
            // SKBitmap webBitmap;
            // HttpClient httpClient = new HttpClient();


            //if (webBitmap != null)
            //{
            //    float x = (info.Width - webBitmap.Width) / 2;
            //    float y = (info.Height / 3 - webBitmap.Height) / 2;
            //    canvas.DrawBitmap(webBitmap, x, y);
            //}

            //if (libraryBitmap != null)
            //{
            //    float scale = Math.Min((float)info.Width / libraryBitmap.Width,
            //                           info.Height / 3f / libraryBitmap.Height);
            //    float left = (info.Width - scale * libraryBitmap.Width) / 2;
            //    float top = (info.Height / 3 - scale * libraryBitmap.Height) / 2;
            //    float right = left + scale * libraryBitmap.Width;
            //    float bottom = top + scale * libraryBitmap.Height;
            //    SKRect rect = new SKRect(left, top, right, bottom);
            //    rect.Offset(0, 2 * info.Height / 3);
            //    canvas.DrawBitmap(libraryBitmap, rect);
            //}
            //else
            //{
            //    using (SKPaint paint = new SKPaint())
            //    {
            //        paint.Color = SKColors.Blue;
            //        paint.TextAlign = SKTextAlign.Center;
            //        paint.TextSize = 48;
            //        canvas.DrawText("Tap to load bitmap",
            //            info.Width / 2, 5 * info.Height / 6, paint);
            //    }
            //}
        }

       
    }
}

