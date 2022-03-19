using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        public UserService(FoodJournalContext foodJournalContext)
        {
            this.Database = foodJournalContext;
        }

        private FoodJournalContext Database { get; }

        public User AddUser(User userToAdd)
        {
            if (userToAdd == null)
            {
                throw new ArgumentNullException(nameof(userToAdd));
            }

            if (this.UserExists(userToAdd))
            {
                throw new ArgumentException("Already exists");
            }

            this.Database.Add(userToAdd);
            this.Database.SaveChanges();
            return userToAdd;
        }

        public User GetUser(int id)
        {
            return this.GetUser(id, null);
        }

        public User GetUser(string? email)
        {
            return this.GetUser(null, email);
        }

        public User GetUser(int? id, string? email)
        {
            if (id is not null)
            {
                return this.GetUserById(id.Value);
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("id and email cannot both be null");
            }

            User? userToReturn = this.Database?.Users?.Include(x => x.UserPassword.Password).SingleOrDefault(user => user.Email == email);

            if (userToReturn is null)
            {
                throw new ArgumentException($"User with an email of {email} not found.");
            }

            return userToReturn;
        }

        public void SavePassword(User newUser, string hashedPassword)
        {
            var dbUser = this.GetUser(newUser.Id);
            dbUser.UserPassword = new UserPassword()
            {
                Password = new Password()
                {
                    Text = hashedPassword,
                },
            };
            this.Database.SaveChanges();
        }

        public bool VerifyPassword(User user, string passwordToVerify)
        {
            User userFromDb = this.GetUser(user.Id);
            return userFromDb.UserPassword.Password.Text.Equals(passwordToVerify);
        }

        private User GetUserById(int userId)
        {
            User matchingUser = this.Database.Users.Find(userId);

            if (matchingUser is null)
            {
                throw new ArgumentException($"Id: {userId} was not found.");
            }

            return matchingUser;
        }

        private bool UserExists(User userToAdd)
        {
            if (this.Database is null)
            {
                throw new Exception();
            }

            return this.Database.Users.Any(user => user.Id == userToAdd.Id || user.Email == userToAdd.Email);
        }
    }
}