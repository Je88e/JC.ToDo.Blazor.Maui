using AntDesign;
using Blazor.Model.Dto;
using Blazor.Model.PageModel;
using Blazor.UI.UIService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Project.Pages.User
{
    public partial class Login
    { 

        [Inject] public NavigationManager NavigationManager { get; set; }


        [Inject] public MessageService Message { get; set; }

        [Inject] public HttpClient Http { get; set; }
        [Inject] public MessageService MsgSvr { get; set; }
        [Inject] public AuthenticationStateProvider AuthProvider { get; set; }

        private LoginDto model = new LoginDto();
        bool isLoading;

        async void OnLogin()
        {
            isLoading = true;

            var httpResponse = await Http.PostAsJsonAsync($"api/Login/BlazorToDoJwtLogin", model);
            UserDto result = await httpResponse.Content.ReadFromJsonAsync<UserDto>();

            if (string.IsNullOrWhiteSpace(result?.Token) == false)
            {
                MsgSvr.Success($"登录成功");
                Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
                ((AuthProvider)AuthProvider).MarkUserAsAuthenticated(result);
            }
            else
            {
                MsgSvr.Error($"用户名或密码错误");
            }
            isLoading = false;
            await InvokeAsync(StateHasChanged);
        }

        public void HandleSubmit()
        {
            if (model.UserName == "admin" && model.Password == "ant.design")
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            if (model.UserName == "user" && model.Password == "ant.design") NavigationManager.NavigateTo("/");
        }
    }
}