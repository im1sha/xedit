using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEdit.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {

        public GalleryPage()
        {
            InitializeComponent();
            //  SetPageContent();        
        }

        //    private void SetPageContent()
        //    {
        //        if (!chosenContent.IsSet)
        //        {
        //            HideImage();
        //        }
        //        else
        //        {
        //            ShowImage();
        //        }          
        //    }

        //    private void ShowImage()
        //    {
        //        messageLabel.IsVisible = false;
        //        pickPictureButton.IsVisible = false;
        //        editedImage.IsVisible = true;
        //    }

        //    private void HideImage()
        //    {
        //        messageLabel.IsVisible = true;
        //        pickPictureButton.IsVisible = true;
        //        editedImage.IsVisible = false;
        //    }

        //    private async void PickPictureButton_Clicked(object sender, EventArgs e)
        //    {
        //        ShowImage();

        //        Stream stream = await DependencyService.Get<PicturePicker.IPicturePicker>().GetImageStreamAsync();

        //        if (stream != null)
        //        {
        //            chosenContent.Image = new Image
        //            {
        //                Source = ImageSource.FromStream(() => stream),
        //                BackgroundColor = Color.White
        //            };              
        //        }
        //        else
        //        {
        //            HideImage();
        //        }
        //    }
        //}
    }
}

//TapGestureRecognizer recognizer = new TapGestureRecognizer();
//recognizer.Tapped += (sender2, args) =>
//{
//    (this as ContentPage).Content = stack;
//    pickPictureButton.IsEnabled = true;
//};
//image.GestureRecognizers.Add(recognizer);
