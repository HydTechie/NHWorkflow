using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NHWorkflow.Core
{
    //public class UserService : IUserService
    //{
    //    private IDictionary<string, (string PasswordHash, User User)> _users =
    //        new Dictionary<string, (string PasswordHash, User User)>();

    //    public UserService(IDictionary<string, string> users)
    //    {
    //        foreach (var user in users)
    //        {
    //            _users.Add(user.Key.ToLower(), (System.Security.Cryptography. .HashPassword(user.Value), new User(user.Key)));
    //        }
    //    }

    //    private static string getSalt()
    //    {
    //        byte[] bytes = new byte[128 / 8];
    //        using (var keyGenerator = RandomNumberGenerator.Create())
    //        {
    //            keyGenerator.GetBytes(bytes);

    //            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    //        }
    //    }

    //    public Task<bool> AddUser(string username, string password)
    //    {
    //        if (_users.ContainsKey(username.ToLower()))
    //        {
    //            return Task.FromResult(false);
    //        }
    //        _users.Add(username.ToLower(), (BCrypt.Net.BCrypt.HashPassword(password), new User(username)));
    //        return Task.FromResult(true);
    //    }

    //    public Task<bool> ValidateCredentials(string username, string password, out User user)
    //    {
    //        user = null;
    //        var key = username.ToLower();
    //        if (_users.ContainsKey(key))
    //        {
    //            var hash = _users[key].PasswordHash;
    //            if (BCrypt.Net.BCrypt.Verify(password, hash))
    //            {
    //                user = _users[key].User;
    //                return Task.FromResult(true);
    //            }
    //        }
    //        return Task.FromResult(false);
    //    }

    //    Task<bool> IUserService.AddUser(string username, string password)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    Task<bool> IUserService.ValidateCredentials(string username, string password, out User user)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
