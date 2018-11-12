using System;
using System.Collections.Generic;
using HASALapp.Models;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class AnnouncementDetailPage : ContentPage
    {
        private Announcement _announcement;

        public AnnouncementDetailPage(Announcement announcement)
        {
            InitializeComponent();
            _announcement = announcement;
        }


        protected override void OnAppearing()
        {
            Title = _announcement.Title;
            LabelText.Text = _announcement.Description;
            Image.Source = _announcement.ImageUrl;
            base.OnAppearing();
        }

    }
}
