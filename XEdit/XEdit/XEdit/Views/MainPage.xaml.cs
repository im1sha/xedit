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
using XEdit.Core;
using XEdit.Extensions;
using XEdit.ViewModels;

namespace XEdit.Views
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent ();
            BindingContext = new MainViewModel();      
        }

        void OnRedrawCanvasView(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawBitmap(AppDispatcher.Get<ImageManager>().Image, info.Rect, BitmapStretch.Uniform);
        }

        private async void Save(object sender, EventArgs e)
        {
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg;
            int quality = 100;

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                AppDispatcher.Get<ImageManager>().Image.Encode(wstream, imageFormat, quality);

                byte[] data = memStream.ToArray();

                bool success = false;

                if (data != null && data.Length != 0)
                {
                    bool isGranted = DependencyService.Get<IUtils>().AskForWriteStoragePermission();
                    if (isGranted)
                    {
                        success = await DependencyService.Get<IPhotoLibrary>().
                            SavePhotoAsync(data, "testFolder", DateTime.Now.ToBinary().ToString() + ".png");
                    }
                                        
                }

                if (success)
                { 
                    statusLabel.Text = "OK" + DateTime.Now.ToBinary().ToString().Substring(10);
                }
                else
                {
                    statusLabel.Text = "FAILED";
                }
               
            }
        }
    }    
}
