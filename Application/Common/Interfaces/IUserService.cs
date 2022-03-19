using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        public User AddUser(User userToAdd);

        public User? GetUser(int? id, string email);

        void SavePassword(User newUser, string hashedPassword);

        bool VerifyPassword(User user, string passwordToVerify);
    }
}