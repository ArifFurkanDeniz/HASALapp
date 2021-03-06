﻿using System;
using System.Collections.Generic;
using HASALapp.Services;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System.Globalization;
#if __IOS__
using Plugin.ExternalMaps;
#endif

namespace HASALapp
{
    public partial class LocationsPage : ContentPage
    {
        private string selectedPinName;
        private Position selectedPosition;
        private Button button;
        public LocationsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            Plugin.Geolocator.Abstractions.Position currentPosition = null;
            var locator = CrossGeolocator.Current;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await DisplayAlert("Konum izni gerekli", "Uygulama konum bilgisini kullanmalı", "Tamam");
                    }



                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                    if (status == PermissionStatus.Granted)
                    {
                        currentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    }
                    else
                    {
                        currentPosition = await locator.GetLastKnownLocationAsync();

                    }
                } 
                else if (status == PermissionStatus.Granted)
                {
                    currentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Konum izni verilmedi", "Bu sayfayı kullanabilmeniz için konum izni vermelisiniz.", "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Konum özellğiğinin açılması gerekli", ex.Message, "Tamam");
            }

            MapSpan region = null;
            Map map = null;

            if (currentPosition!=null)
            {
                region = MapSpan.FromCenterAndRadius(
                               new Position(currentPosition.Latitude, currentPosition.Longitude), Distance.FromMiles(3));
               
            }
            else{
                // hasal'dan başlat 41.013929, 29.038842
                region = MapSpan.FromCenterAndRadius(
                               new Position(41.013929, 29.038842), Distance.FromMiles(3));

            }

            map = new Map(region)
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };





            FirebaseService firebaseService = new FirebaseService();
            var locationList = await firebaseService.GetLocations();
#region pins
            foreach (var item in locationList)
            {
                double latitude = Convert.ToDouble(item.Position.ToString().Split(',')[0], CultureInfo.InvariantCulture);
                double longitude = Convert.ToDouble(item.Position.ToString().Split(',')[1], CultureInfo.InvariantCulture);
                var position = new Position(latitude, longitude ); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = item.Title,
                    Address = item.Text,
                     
                };

                pin.Clicked += (sender, args) => {

#if __IOS__
                    button.IsEnabled = true;
                    button.Text = pin.Label + " için yol tarifi";
#endif
                   selectedPinName = pin.Label;
                    selectedPosition = pin.Position;
                };

                map.Pins.Add(pin);
            }
         
#endregion

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);

#if __IOS__
                 button = new Button()
                {
                    Text = "Yol tarifi",
                    VerticalOptions = LayoutOptions.End,
                    IsEnabled = false
                    
                };

                button.Clicked += (sender, args)=> {
                   
                    if (selectedPosition != null)
                    {
                        CrossExternalMaps.Current.NavigateTo(selectedPinName, selectedPosition.Latitude, selectedPosition.Longitude);
                    }
                   
                };

            button.BackgroundColor = Color.White;
            button.TextColor = Color.FromHex("#823642");
            stack.Children.Add(button);
#endif

            Content = stack;
            base.OnAppearing();
        }
    }
}
