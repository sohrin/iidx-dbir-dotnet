using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using NLog;

namespace Dbir.Auth
{
    public class LoginViewModel : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IAuthService AuthService { get; set; }

        public LoginData LoginData { get; set; } = new LoginData();
        public string ErrorMessage { get; set; }
        public bool IsLoading { get; set; } = false;

        public async Task SubmitAsync()
        {
            IsLoading = true;
            var model = new LoginModel() { UserID = LoginData.UserID, Password = LoginData.Password };
            var result = await AuthService.LoginAsync(model);
            if (result.IsSuccessful)
            {
                NavigationManager.NavigateTo("/");
            }
            else
            {
                ErrorMessage = "ログインに失敗しました。";
            }
            IsLoading = false;
        }
    }

    public class LoginData
    {
        [Required(ErrorMessage = "ユーザIDを入力してください。")]
        [StringLength(32, ErrorMessage = "ユーザIDが長すぎます。")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "パスワードを入力してください。")]
        [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
        public string Password { get; set; }
    }

    public class FirebaseAuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public FirebaseAuthService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var provider = new FirebaseAuthProvider(new FirebaseConfig("ApiKEYを入れる"));
                var firebaseResult = await provider.SignInWithEmailAndPasswordAsync(loginModel.UserID, loginModel.Password);
                // トークンを取得
                var res = new LoginResult()
                {
                    IsSuccessful = true,
                    IDToken = firebaseResult.FirebaseToken
                };
                await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.UserID, res.IDToken);
                return res;
            }
            catch (FirebaseAuthException e)
            {
                return new LoginResult()
                {
                    IsSuccessful = false,
                    Error = e
                };
            }
        }

        public async Task LogoutAsync()
        {
            await ((SpaAuthticateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }

}
