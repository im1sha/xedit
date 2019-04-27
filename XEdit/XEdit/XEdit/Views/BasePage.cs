using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace XEdit.Views
{
    public class BasePage : ContentPage
    {
        public ICommand NavigateCommand { private set; get; }

        public BasePage()
        {
            NavigationPage.SetHasNavigationBar(this, false);

            NavigateCommand = new Command<Type>(async (Type pageType) =>
            {
                Page page = (Page)Activator.CreateInstance(pageType);
                await Navigation.PushAsync(page);
            });

            BindingContext = this;
        }
    }
}

