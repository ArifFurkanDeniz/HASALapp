using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using HASALapp.Enums;
using HASALapp.Models;
using Newtonsoft.Json;

namespace HASALapp.Services
{
    public class FirebaseService
    {
        IFirebaseClient client;
        public static User User;
        public FirebaseService()
        {

            IFirebaseConfig config = new FirebaseConfig
            {
                //AuthSecret = "",
                BasePath = "https://hasal-3c840.firebaseio.com/"
            };

            client = new FirebaseClient(config);
        }

        public async Task<List<Announcement>> GetAnnouncements()
        {
            var response = await client.GetAsync(CollectionEnum.announcements.ToString());
            var announcements = JsonConvert.DeserializeObject<Dictionary<string, Announcement>>(response.Body);

            var announcementList = new List<Announcement>();
            foreach (var item in announcements)
            {
                var announcement = item.Value;
                announcement.Key = item.Key;
                announcementList.Add(item.Value);
            }

            return announcementList;
        }

        public async Task<List<Survey>> GetSurveys()
        {
            var response = await client.GetAsync(CollectionEnum.surveys.ToString());
            var surveys = JsonConvert.DeserializeObject<Dictionary<string, Survey>>(response.Body);

            var surveyList = new List<Survey>();
            foreach (var item in surveys)
            {
                var survey = item.Value;
                survey.Key = item.Key;
                surveyList.Add(item.Value);
            }

            return surveyList;
        }

        public async Task<List<Location>> GetLocations()
        {
            var response = await client.GetAsync(CollectionEnum.locations.ToString());
            var locations = JsonConvert.DeserializeObject<Dictionary<string, Location>>(response.Body);

            var locationList = new List<Location>();
            foreach (var item in locations)
            {
                var location = item.Value;
                location.Key = item.Key;
                locationList.Add(item.Value);
            }

            return locationList;
        }
    }
}
