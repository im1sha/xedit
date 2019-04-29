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
using XEdit.Core;

namespace XEdit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : BasePage
	{
		public StartPage() 
		{
            InitializeComponent();
		}

        public async void OnSelectFromGallery(object sender, EventArgs e)
        {
            using (Stream stream = await DependencyService.Get<IPhotoLibrary>().PickPhotoAsync())
            {
                if (stream != null)
                {
                    AppDispatcher.Get<ImageManager>().Image = SKBitmap.Decode(stream);
                    NavigateCommand.Execute(typeof(MainPage));
                }
            }
        }   
    }
}