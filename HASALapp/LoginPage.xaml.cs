using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {

            InitializeComponent();

            /*#if DEBUG
            emailEntry.Text = "ariffurkandeniz@gmail.com";
            passwordEntry.Text = "535353";
            #endif*/
           

            emailEntry.Text = SettingsService.LastUsedEmail;
            passwordEntry.Text = SettingsService.LastUsedPassword;

        }
        protected async override void OnAppearing()
        {
    
            if (!string.IsNullOrEmpty(SettingsService.LastUsedEmail) && !string.IsNullOrEmpty(SettingsService.LastUsedPassword) && !GeneralHelper.IsNotFirstLogin)
            {
                try
                {
                    var user = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPasswordAsync(SettingsService.LastUsedEmail, SettingsService.LastUsedPassword);
                    //user.Start();

                    if (user != null)
                    {
                        GeneralHelper.IsNotFirstLogin = true;
                        FirebaseService.User = user;
                        await Navigation.PushModalAsync(new NavigationPage(new MyTabbedPage()));
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Uyarı", ex.Message, "Tamam");
                }
               
            }
            base.OnAppearing();
        }


        async void Login_Clicked(object sender, System.EventArgs e)
        {
            //NavigationPage nav = new NavigationPage(new MyTabbedPage());
            try
            {
                var user = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPasswordAsync(emailEntry.Text, passwordEntry.Text);
                if (user != null)
                {
                    GeneralHelper.IsNotFirstLogin = true;
                    SettingsService.LastUsedEmail = emailEntry.Text;
                    SettingsService.LastUsedPassword = passwordEntry.Text;
                    FirebaseService.User = user;
                    await Navigation.PushModalAsync(new NavigationPage(new MyTabbedPage()));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Uyarı",ex.Message, "Tamam");
            }
           
           
        }

        void ForgetPassword_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}
