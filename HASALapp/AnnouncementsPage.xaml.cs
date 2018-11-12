using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HASALapp.Models;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class AnnouncementsPage : ContentPage
    {
        public AnnouncementsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            FirebaseService firebaseService = new FirebaseService();
            var announcementList = await firebaseService.GetAnnouncements();

            ObservableCollection<Announcement> announcements = new ObservableCollection<Announcement>(announcementList);
            AnnouncementsView.ItemsSource = announcements;
            base.OnAppearing();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
        
            Navigation.PushAsync(new AnnouncementDetailPage((Announcement)e.Item));
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            AnnouncementsView.SelectedItem = null;
        }
    }
}
