﻿using System;
using System.Threading.Tasks;
using Firebase.Auth;
using HASALapp.Droid.Services;
using HASALapp.Models;
using HASALapp.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthenticator))]
namespace HASALapp.Droid.Services
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
       

        public async Task<User> LoginWithEmailPasswordAsync(string email, string password)
        {

            var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
          
            try
            {
                var token = await user.User.GetIdTokenAsync(false);

                if (token!=null)
                {
                    var _user = new User()
                    {
                        Token = token.Token,
                        UserId = user.User.Uid,
                        Email = user.User.Email
                    };
                    return _user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
                
        }
    }
}
