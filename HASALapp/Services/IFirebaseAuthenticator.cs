using System;
using System.Threading.Tasks;
using HASALapp.Models;

namespace HASALapp.Services
{
    public interface IFirebaseAuthenticator
    {
        Task<User> LoginWithEmailPasswordAsync(string email, string password);

    }
}
