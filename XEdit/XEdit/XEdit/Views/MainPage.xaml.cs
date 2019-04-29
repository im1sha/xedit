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
  
        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void OnUpdate(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.DarkBlue);
            canvas.DrawBitmap(AppDispatcher.Get<ImageManager>().Image, info.Rect, BitmapStretch.Uniform);
        }
    }
}


