using System;
using System.Collections.Generic;
using System.Linq;
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
        const int pageSize = 5;
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
            string apiString = CollectionEnum.announcements.ToString();
            var response = await client.GetAsync(apiString);
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
            var _surveys =  client.GetAsync(CollectionEnum.surveys.ToString());
            var _choices =  client.GetAsync(CollectionEnum.userchoices.ToString() + '/' + User.UserId);

            var tasks = await Task.WhenAll(_surveys, _choices);

            var surveys = JsonConvert.DeserializeObject<Dictionary<string, Survey>>(_surveys.Result.Body);
            var choices = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, UserChoice>>>(_choices.Result.Body);
          

            var surveyList = new List<Survey>();
            foreach (var item in surveys)
            {
                var survey = item.Value;
                survey.Key = item.Key;
                var userChoices = choices.Where(x => x.Key == item.Key).Select(x=>x.Value).FirstOrDefault();
                if (userChoices!=null)
                {
                    foreach (var choice in survey.Choices)
                    {
                        if (userChoices.Values.Where(x=>x.ChoiceKey == choice.Key).Any())
                        {
                            choice.IsSelected = true;

                        }
                    }
                }
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

        public async Task<bool> PostSurvey(string surveyKey, string choiceKey)
        {
            try
            {
                var delete = await client.DeleteAsync(CollectionEnum.userchoices.ToString() + '/' + User.UserId + '/' + surveyKey);
                var response = await client.PushAsync(CollectionEnum.userchoices.ToString() + '/' + User.UserId + '/' + surveyKey,
                                                      new UserChoice() { ChoiceKey = choiceKey });
               
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
           
        }
    }
}
