using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using XEdit.DataStorage;

namespace XEdit.XAML
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        //readonly ChosenDataViewModel vm = new ChosenDataViewModel();
         
        public MainPage()
        {
            InitializeComponent();

           // BindingContext = vm;

            masterPage.listView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
