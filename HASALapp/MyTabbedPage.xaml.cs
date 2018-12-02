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
        }

        void Logout_Clicked(object sender, System.EventArgs e)
        {
            SettingsService.LastUsedEmail = null;
            SettingsService.LastUsedPassword = null;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}
