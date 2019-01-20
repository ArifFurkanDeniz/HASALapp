using System;
using System.Threading.Tasks;
using Firebase.Auth;
using HASALapp.iOS.Services;
using HASALapp.Models;
using HASALapp.Services;
using Xamarin.Forms;
using User = HASALapp.Models.User;

[assembly: Dependency(typeof(FirebaseAuthenticator))]
namespace HASALapp.iOS.Services
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<User> LoginWithEmailPasswordAsync(string email, string password)
        {

            var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);

            try
            {
                var token = await user.User.GetIdTokenAsync(false);

                var _user = new User()
                {
                    Token = token,
                    UserId = user.User.Uid,
                    Email = user.User.Email
                };
                return _user;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
