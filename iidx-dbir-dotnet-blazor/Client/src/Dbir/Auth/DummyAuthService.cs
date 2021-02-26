using Dbir.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Dbir.Auth
{
    public class DummyAuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public DummyAuthService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            // 3秒待機させて本当の応答のように見せる
            await Task.Delay(3000);
            if (loginModel.UserID == "demo" && loginModel.Password == "demo")
            {
                var res = new LoginResult()
                {
                    IsSuccessful = true,
                    IDToken = "hoge"
                };
                await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.UserID, res.IDToken);
                return res;
            }
            else
            {
                return new LoginResult()
                {
                    IsSuccessful = false,
                    Error = new Exception("NotAuthrized")
                };
            }
        }

        public async Task LogoutAsync()
        {
            await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }
}
