using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEdit.ViewModels;

namespace XEdit.Layout
{
  
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        SKCanvasView canvasView;
        SKBitmap resourceBitmap;


        // SKBitmap libraryBitmap;
        // SKBitmap webBitmap;
        // HttpClient httpClient = new HttpClient();


        public MainPage ()
		{

			InitializeComponent ();

            BindingContext = new FiltersViewModel();






            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            skiaWrapper.Children.Add(canvasView);

            // Load resource bitmap
            string resourceID = "XEdit.Images.usa.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
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
        }

     

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


        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            //if (webBitmap != null)
            //{
            //    float x = (info.Width - webBitmap.Width) / 2;
            //    float y = (info.Height / 3 - webBitmap.Height) / 2;
            //    canvas.DrawBitmap(webBitmap, x, y);
            //}

            if (resourceBitmap != null)
            {
                SKRect pictureFrame = SKRect.Create(0, 0, info.Width, info.Height);

                SKRect dest = pictureFrame.AspectFit(new SKSize(resourceBitmap.Width, resourceBitmap.Height));

                canvas.DrawBitmap(resourceBitmap, dest, new SKPaint() { FilterQuality = SKFilterQuality.High });
            }

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