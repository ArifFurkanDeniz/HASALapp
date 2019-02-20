using System;
using System.Threading.Tasks;
using HASALapp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace HASALapp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SplashPage();

            /* var _task = StartedPage();


             if (!_task)
             {
                 MainPage = new LoginPage();
             }
             else
             {
                // MainPage = new MyTabbedPage();
                 MainPage = new NavigationPage(new MyTabbedPage());
             }*/

        }

        protected override void OnStart()
        {
            // Handle when your app starts
       
             

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private bool StartedPage()
        {
            bool firstLogin = false;

            if (!string.IsNullOrEmpty(SettingsService.LastUsedEmail) && !string.IsNullOrEmpty(SettingsService.LastUsedPassword) && !GeneralHelper.IsNotFirstLogin)
            {
                firstLogin = true;

            }

            return firstLogin;
        }
    }
}
