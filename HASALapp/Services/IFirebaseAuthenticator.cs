using System;
using System.Threading.Tasks;
using HASALapp.Models;

namespace HASALapp.Services
{
    public interface IFirebaseAuthenticator
    {
        Task<User> LoginWithEmailPassword(string email, string password);
    }
}
