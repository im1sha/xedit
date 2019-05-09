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
using XEdit.Extensions;
using XEdit.ViewModels;

namespace XEdit.Views
{ 
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        MainViewModel vm;

        public MainPage()
		{
			InitializeComponent ();

            vm = new MainViewModel();
            BindingContext = vm;
            vm.OnViewCreated(skiaWrapper, skiaCanvasView, variableValuesSlider, touchTracker);

            MessagingCenter.Subscribe<MainViewModel, string>(
                this, 
                Messages.SaveSuccess,   
                async (sender, name) => {
                    if (name != null)
                    {
                        await DisplayAlert("Save", $"Saved at XEdit as {name}", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Save", $"Permisson is required. Give permisson and save again.", "OK");
                    }
                });    
        }
  
        private async void OnBack(object sender, EventArgs e)
        {
            vm.OnPopScreen();
            await Navigation.PopAsync();
        }      
    }
}


