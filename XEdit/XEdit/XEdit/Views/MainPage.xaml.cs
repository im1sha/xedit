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
            AppDispatcher.Get<ImageManager>().SetCanvasViewReference(skiaCanvasView);
        }
  
        private async void OnBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }      
    }
}


