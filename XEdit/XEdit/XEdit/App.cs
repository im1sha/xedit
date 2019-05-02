using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:XamlCompilation(XamlCompilationOptions.Compile)]
namespace XEdit
{
	public class App : Application
	{
		public App ()
		{
            UniqueInstancesManager.Register(ImageWorker.Instance); // register image handler
            MainPage = new NavigationPage(new Views.StartPage());
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

