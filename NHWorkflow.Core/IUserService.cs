using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHWorkflow.Core
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out User user);
        Task<bool> AddUser(string username, string password);
    }

    public class User
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }

    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
