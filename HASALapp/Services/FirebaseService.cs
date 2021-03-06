﻿
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
                AuthSecret = FirebaseService.User.Token,
                BasePath = "https://hasal-3c840.firebaseio.com/"
            };

            client = new FirebaseClient(config);
        }

        public async Task<List<Announcement>> GetAnnouncements()
        {
            string apiString = CollectionEnum.announcements.ToString();

          
            var response = await client.GetAsync(apiString);  

            if (response.Body == "null")
            {
                return new List<Announcement>();
            }

            var announcements = JsonConvert.DeserializeObject<Dictionary<string, Announcement>>(response.Body);

            var announcementList = new List<Announcement>();

            foreach (var item in announcements.OrderBy(x=>x.Value.Desc))
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

            if (_surveys.Result.Body == "null")
            {
                return new List<Survey>();
            }


            var surveyList = new List<Survey>();
            foreach (var item in surveys.OrderBy(x => x.Value.Desc))
            {
                var _surveyChoices = await client.GetAsync(CollectionEnum.surveys.ToString()+"/"+item.Key +"/" + CollectionEnum.Choices.ToString());
               
                if (_surveyChoices.Body != "null")
                {
                    var _surveyChoices2 = JsonConvert.DeserializeObject<Dictionary<string, Choice>>(_surveyChoices.Body);

                    var survey = item.Value;
                    survey.Key = item.Key;
                    survey._Choices = _surveyChoices2.Select(x => new Choice { Key = x.Key, Title = x.Value.Title }).ToList();

                    if (choices != null)
                    {
                        var userChoices = choices.Where(x => x.Key == item.Key).Select(x => x.Value).FirstOrDefault();
                        if (userChoices != null)
                        {
                            foreach (var choice in survey._Choices)
                            {
                                if (userChoices.Values.Where(x => x.ChoiceKey == choice.Key).Any())
                                {
                                    choice.IsSelected = true;

                                }
                            }
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

            if (response.Body == "null")
            {
                return new List<Location>();
            }


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

            return true;
           
        }

        public async Task<bool> PostQuestion(string questionKey, string title, string text)
        {
            try
            {
                DateTime e = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                DateTime s = new DateTime(1970, 1, 1, 0, 0, 0);
                long timeSpan = e.ToUniversalTime().Ticks - s.Ticks;

                if (questionKey == null)
                {
                    var response = await client.PushAsync(CollectionEnum.questions.ToString() + '/' + User.UserId + '/',
                                                                  new Question()
                                                                  {
                                                                      Title = title,
                                                                      Asc = timeSpan,
                                                                      Desc = timeSpan
                                                                      * -1
                                                                  });

                    var response2 = await client.PushAsync(CollectionEnum.questions.ToString() + '/' + User.UserId + '/' + response.Result.name + "/messages",
                                                                  new Message()
                                                                  {
                                                                       Text = text,
                                                                       Type = 1,
                                                                      Asc = timeSpan,
                                                                      Desc = timeSpan
                                                                      * -1
                                                                  });



                }
                else
                {
                    var response = await client.PushAsync(CollectionEnum.questions.ToString() + '/' + User.UserId + '/' + questionKey +"/messages",
                                                                  new Message() 
                                                                    { 
                                                                        Text  = text,
                                                                        Type =1,
                                                                        Asc = timeSpan,
                                                                        Desc = timeSpan
                                                                      * -1
                                                                  });
                }


            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<List<Question>> GetMyQuestions()
        {
            var response = await client.GetAsync(CollectionEnum.questions.ToString() + '/' + User.UserId);
            var questions = JsonConvert.DeserializeObject<Dictionary<string, Question>>(response.Body);

            if (response.Body == "null")
            {
                return new List<Question>();
            }


            var questionList = new List<Question>();
            foreach (var item in questions.OrderBy(x=>x.Value.Desc))
            {
                var question = item.Value;
                question.Key = item.Key;
                questionList.Add(item.Value);
            }

            return questionList;
        }

        public async Task<bool> SendToken(string token)
        {
            try
            {
                var response = await client.UpdateAsync(CollectionEnum.tokens.ToString() + '/' + User.UserId,
                                                                 new UserToken() { Token = token});
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
