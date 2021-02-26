using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Dbir.Auth
{
    public class SpaAuthticateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public SpaAuthticateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // ローカルストレージからトークンとユーザ名を取得
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");
            var userID = await _localStorage.GetItemAsync<string>("userID");
            // トークンのチェックを入れる場合ここで一度だけ実施

            // トークンが見つからい場合は未ログイン
            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            // HTTPの認証用のトークンを設定
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            // 認証情報を返す
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userID) }, "apiauth")));
        }

        public async Task MarkUserAsAuthenticated(string userID, string authToken)
        {
            // ローカルストレージに認証情報を保持して変更通知を行う
            await _localStorage.SetItemAsync("userID", userID);
            await _localStorage.SetItemAsync("authToken", authToken);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task MarkUserAsLoggedOut()
        {
            // ローカルストレージの認証情報を削除して変更通知を行う
            await _localStorage.RemoveItemAsync("userID");
            await _localStorage.RemoveItemAsync("authToken");
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

}
