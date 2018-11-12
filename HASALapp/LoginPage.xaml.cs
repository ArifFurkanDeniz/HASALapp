using System;
using System.Collections.Generic;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {

            InitializeComponent();

            #if DEBUG
            emailEntry.Text = "ariffurkandeniz@gmail.com";
            passwordEntry.Text = "535353";
            #endif
        }

        async void LoginClicLogin_Clicked(object sender, System.EventArgs e)
        {
            //NavigationPage nav = new NavigationPage(new MyTabbedPage());
            try
            {
                var user = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPassword(emailEntry.Text, passwordEntry.Text);
                if (user != null)
                {
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
