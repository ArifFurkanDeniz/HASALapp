using System;
using System.Collections.Generic;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class MyTabbedPage : TabbedPage
    {
        public MyTabbedPage()
        {
            InitializeComponent();


            this.BarTextColor = Color.White;
            this.BarBackgroundColor = Color.FromHex("#823642");
        }

        void Logout_Clicked(object sender, System.EventArgs e)
        {
            SettingsService.LastUsedEmail = null;
            SettingsService.LastUsedPassword = null;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        async void AskQuestion_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AskQuestionPage(new Models.Question()));
        }


        async void MyQuestions_clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MyQuestionPage());
        }


    }
}


