using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HASALapp.Models;
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
            Title = _survey.Title;
            LabelText.Text = _survey.Text;
            Image.Source = _survey.ImageUrl;
         

            base.OnAppearing();
        }

      
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
