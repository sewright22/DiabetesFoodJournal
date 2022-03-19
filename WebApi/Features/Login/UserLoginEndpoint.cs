﻿using Application.Common.Interfaces;
using Domain.Entities;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Features.Login
{
    public class UserLoginEndpoint : Endpoint<LoginRequest>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserService _userService;

        public UserLoginEndpoint(IUserService userService, IPasswordHasher<User> passwordHasher)
        {
            this._userService = userService;
            this._passwordHasher = passwordHasher;
        }

        public override void Configure()
        {
            Verbs(Http.POST);
            Routes("/api/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
        {
            var user = this._userService.GetUser(null, req.Username);

            if (user == null)
            {
                throw new ArgumentException("Unknown user.");
            }

            var passwordVerificationResult = this._passwordHasher.VerifyHashedPassword(user, user.UserPassword.Password.Text, req.Password);

            if (passwordVerificationResult.HasFlag(PasswordVerificationResult.Success))
            {
                var jwtToken = JWTBearer.CreateToken(
                    signingKey: "This is a super secret long key for authentication.",
                    expireAt: DateTime.UtcNow.AddDays(1),
                    claims: new[] { ("Username", user.Email), ("UserID", user.Id.ToString()) },
                    roles: new string[] { },
                    permissions: new string[] { "ReadOwnEntries", "WriteEntries" });

                await SendAsync(new
                {
                    Username = req.Username,
                    Token = jwtToken
                });
            }
            else
            {
                ThrowError("The supplied credentials are invalid!");
            }
        }
    }
}