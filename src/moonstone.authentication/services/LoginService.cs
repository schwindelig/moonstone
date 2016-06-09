using Microsoft.AspNet.Identity.Owin;
using moonstone.authentication.managers;
using moonstone.core.exceptions;
using moonstone.core.services;
using moonstone.core.services.results;
using System;

namespace moonstone.authentication.services
{
    public class LoginService : ILoginService
    {
        public SignInManager SignInManager { get; set; }

        public LoginService(SignInManager signInManager)
        {
            this.SignInManager = signInManager;
        }

        public LoginResult Login(string username, string password, bool isPersistent, bool shouldLockOut)
        {
            try
            {
                var result = this.SignInManager.PasswordSignIn(
                    userName: username,
                    password: password,
                    isPersistent: isPersistent,
                    shouldLockout: shouldLockOut);

                switch (result)
                {
                    case SignInStatus.Success:
                        return LoginResult.Success;

                    case SignInStatus.LockedOut:
                        return LoginResult.LockedOut;

                    case SignInStatus.RequiresVerification:
                        return LoginResult.RequiresVerification;

                    case SignInStatus.Failure:
                        return LoginResult.Failed;

                    default:
                        throw new NotImplementedException(
                            $"SignInStatus {result} is not implemented.");
                }
            }
            catch (Exception e)
            {
                throw new LoginException(
                    $"Failed to log in user {username}", e);
            }
        }

        public void Logout()
        {
            this.SignInManager.AuthenticationManager.SignOut();
        }
    }
}