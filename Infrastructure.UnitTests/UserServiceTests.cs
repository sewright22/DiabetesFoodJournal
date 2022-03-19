using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Dsl;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;

namespace DiabetesFoodJournal.Infrastructure.UnitTests
{
    [TestClass]
    public class UserServiceTests
    {
        private FoodJournalContext? dbContext;
        private IFixture? fixture;
        private IPostprocessComposer<User>? newUserBuilder;

        [TestMethod]
        public void AddNewUser()
        {
            var expectedEmail = "test@unittest.com";
            var newUser = this.newUserBuilder.With(x => x.Email, expectedEmail).Without(x => x.UserPassword).Create();
            var userService = this.fixture.Create<UserService>();

            userService.AddUser(newUser);

            var actual = this.dbContext.Users.Find(1);
            actual.Should().NotBeNull();
            actual.Email.Should().Be(expectedEmail);
            actual.UserPassword.Should().BeNull();
        }

        [TestMethod]
        public void AddNullUser()
        {
            var userService = this.fixture.Create<UserService>();
            Action act = () => userService.AddUser(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void AddPasswordToExistingUser()
        {
            var expectedPassword = "askdjhfjlkasndfjklansdf";
            var existingUser = this.newUserBuilder.Without(x => x.UserPassword).Create();
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.Add(existingUser);
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();
            userService.SavePassword(existingUser, expectedPassword);
            var actualPassword = this.dbContext?.UserPasswords?.Include(up => up.Password).SingleOrDefault(x => x.UserId == existingUser.Id);
            actualPassword?.Password?.Text.Should().Be(expectedPassword);
        }

        [TestCleanup]
        public void Dispose()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }

        [TestMethod]
        public void GetUserByEmail()
        {
            var expectedEmail = "test@unittest.com";
            var newUser = this.newUserBuilder.With(x => x.Email, expectedEmail).Create();
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.Add(newUser);
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            User actual = userService.GetUser(null, expectedEmail);

            actual.Should().BeEquivalentTo(newUser);
        }

        [TestMethod]
        public void GetUserById()
        {
            var expectedUser = this.newUserBuilder.Create();
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.Add(expectedUser);
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            var actual = userService.GetUser(expectedUser.Id, string.Empty);

            actual.Should().BeEquivalentTo(expectedUser);
        }

        [TestMethod]
        public void GetUserByIdThrowsArgumentException()
        {
            var expectedUser = newUserBuilder.Create();
            this.dbContext?.AddRange(newUserBuilder.CreateMany());
            this.dbContext?.Add(expectedUser);
            this.dbContext?.AddRange(newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            var method = () => userService.GetUser(65, string.Empty);

            method.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void PasswordSuccessfullyVerified()
        {
            var existingUser = this.newUserBuilder.Create();
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.Add(existingUser);
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();
            userService.VerifyPassword(existingUser, existingUser.UserPassword.Password.Text).Should().BeTrue();
        }

        [TestInitialize]
        public void Setup()
        {
            this.fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            this.newUserBuilder = this.fixture.Build<User>().Without(x => x.Id);
            var options = new DbContextOptionsBuilder<FoodJournalContext>().UseInMemoryDatabase(databaseName: "FoodJournal").Options;
            this.dbContext = new FoodJournalContext(options);
            this.fixture.Inject(dbContext);
        }

        [TestMethod]
        public void UserAlreadyExists()
        {
            var expectedEmail = "test@unittest.com";
            this.dbContext.Add(new User
            {
                Email = expectedEmail,
            });
            this.dbContext.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            Action act = () => userService.AddUser(new User()
            {
                Email = expectedEmail,
            });

            act.Should().Throw<ArgumentException>();
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow(null, "")]
        public void UserIdAndEmailAreNull(int? id, string email)
        {
            var expectedUser = newUserBuilder.Create();
            this.dbContext?.AddRange(newUserBuilder.CreateMany());
            this.dbContext?.Add(expectedUser);
            this.dbContext?.AddRange(newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            var method = () => userService.GetUser(id, email);

            method.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void UserNotFoundByEmail()
        {
            var expectedUser = this.newUserBuilder.Create();
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.Add(expectedUser);
            this.dbContext?.AddRange(this.newUserBuilder.CreateMany());
            this.dbContext?.SaveChanges();

            var userService = this.fixture.Create<UserService>();

            var method = () => userService.GetUser(id: null, email: "email@email.com");

            method.Should().Throw<ArgumentException>();
        }
    }
}