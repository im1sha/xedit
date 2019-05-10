using Plugin.Media;
using Plugin.Media.Abstractions;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XEdit.PlatformSpecific;

namespace XEdit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : BasePage
	{
		public StartPage() 
		{
            InitializeComponent();
            MessagingCenter.Subscribe<Application, bool>(
               this,
               Messages.Camera,
               async (sender, result) => {
                   if (!result)
                   {
                       await DisplayAlert("Camera", $"Permisson is required. Give permisson and try again.", "OK");                      
                   }
                   else
                   {
                       await TakePhoto();
                   }
               });
        }

        public async void OnSelectFromGallery(object sender, EventArgs e)
        {
            using (Stream stream = await DependencyService.Get<IPhotoLibrary>().PickPhotoAsync())
            {
                if (stream != null)
                {
                    UniqueInstancesManager.Get<ImageWorker>().Image = SKBitmap.Decode(stream);
                    NavigateCommand.Execute(typeof(MainPage));
                }
            }
        }

        public async void OnSelectTakePhoto(object sender, EventArgs e)
        {
            if (DependencyService.Get<IUtils>().AskForCameraPermissons())
            {
                await TakePhoto();
            }
        }

        public async Task TakePhoto()
        {          
            await CrossMedia.Current.Initialize();

            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());  
                //    new StoreCameraMediaOptions
                //{
                //    SaveToAlbum = true,
                //    Directory = "XEdit",
                //    Name = $"{DateTime.Now.ToString("dd.MM.yyyy_hh.mm.ss")}.jpg"
                //}

                if (file == null)
                {
                    return;
                }

                UniqueInstancesManager.Get<ImageWorker>().Image = SKBitmap.Decode(file.Path);
                NavigateCommand.Execute(typeof(MainPage));
            }
        }
    }
}

