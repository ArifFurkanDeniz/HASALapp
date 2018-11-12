using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HASALapp.Models;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class SurveysPage : ContentPage
    {
        public SurveysPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            FirebaseService firebaseService = new FirebaseService();
            var surveyList = await firebaseService.GetSurveys();

            ObservableCollection<Survey> surveys = new ObservableCollection<Survey>(surveyList);

            SurveysView.ItemsSource = surveys;
            base.OnAppearing();
        }


        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
           
            Navigation.PushAsync(new SurveyDetailPage((Survey)e.Item));


        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            SurveysView.SelectedItem = null;
        }
    }
}
