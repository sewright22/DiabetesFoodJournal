using Application.Common.Interfaces;
using Domain.Entities;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Features.Features.CreateUser
{
    public class CreateUserEndpoint : Endpoint<CreateUserRequest>
    {
        public CreateUserEndpoint(IUserService userService)
        {
            this.UserService = userService;
        }

        private IUserService UserService { get; }

        public override void Configure()
        {
            this.Verbs(Http.POST);
            this.Routes("api/user/create");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateUserRequest request, CancellationToken ct)
        {
            var newUser = this.UserService.AddUser(new User
            {
                Email = request.Email,
            });

            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(newUser, request.Password);

            this.UserService.SavePassword(newUser, hashedPassword);

            var response = new CreateUserResponse
            {
                FullName = $"{request.FirstName} {request.LastName}",
                Email = $"{request.Email}",
            };

            await SendAsync(response).ConfigureAwait(false);
        }
    }
}