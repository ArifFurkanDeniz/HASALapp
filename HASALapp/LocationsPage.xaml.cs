using System;
using System.Collections.Generic;
using HASALapp.Services;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Plugin.Permissions.Abstractions;
using Plugin.Permissions;

namespace HASALapp
{
    public partial class LocationsPage : ContentPage
    {
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
                        await DisplayAlert("Need location", "Gunna need that location", "OK");
                    }



                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                    if (status == PermissionStatus.Granted)
                    {
                        currentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                    }
                    else
                    {
                        //to do: bilinen son konumu al
                    }
                }

                if (status == PermissionStatus.Granted)
                {
                    currentPosition = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                //...
            }

           
            var map = new Map(
           MapSpan.FromCenterAndRadius(
                    new Position(currentPosition.Latitude, currentPosition.Longitude), Distance.FromMiles(3)))
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
                double latitude = Convert.ToDouble(item.Position.ToString().Split(',')[0]);
                double longitude = Convert.ToDouble(item.Position.ToString().Split(',')[1]);
                var position = new Position(latitude, longitude ); // Latitude, Longitude
                var pin = new Pin
                {
                    Type = PinType.Place,
                    Position = position,
                    Label = item.Title,
                    Address = item.Text
                };
                map.Pins.Add(pin);
            }
         
            #endregion

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;
            base.OnAppearing();
        }
    }
}
