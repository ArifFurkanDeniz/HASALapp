using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HASALapp.Models;
using HASALapp.Services;
using Xamarin.Forms;

namespace HASALapp
{
    public partial class SurveyDetailPage : ContentPage
    {
        private Survey _survey;
        public SurveyDetailPage(Survey survey)
        {
            InitializeComponent();
            _survey = survey;
        }

        protected override void OnAppearing()
        {
            ObservableCollection<Choice> choices = new ObservableCollection<Choice>(_survey.Choices);
            ChoiceList.ItemsSource = choices;
            ChoiceList.SelectedItem = choices.Where(x => x.IsSelected).FirstOrDefault();
            Title = _survey.Title;
            LabelText.Text = _survey.Text;
            Image.Source = _survey.ImageUrl;
         

            base.OnAppearing();
        }

      
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            FirebaseService firebaseService = new FirebaseService();
            var result = await firebaseService.PostSurvey(_survey.Key, ((Choice)ChoiceList.SelectedItem).Key);
          
            if (result)
            {
                await DisplayAlert("Başarılı", "Anket oyunuz gödnerilmiştir.", "Tamam");
            }
            else
            {
               await DisplayAlert("Başarısız", "Anket oyunuz gönderilemedi.", "Tamam");
            }
        }
    }
}
