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

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
        
            //public void DrawBitmap(SKBitmap bitmap, SKRect source, SKRect dest, SKPaint paint = null);
            canvas.DrawBitmap(AppDispatcher.Get<ImageManager>().Image, new SKRect(0, 0, info.Width, info.Height));
        }
    }    
}
