using Microsoft.AspNetCore.Components;
using AntDesign;
using Blazor.Model.PageModel;

namespace Blazor.UI.Pages
{
  public partial class Login {
    private readonly LoginParamsType _model = new LoginParamsType();

        [Inject] public NavigationManager NavigationManager { get; set; }

        //[Inject] public IAccountService AccountService { get; set; }

        [Inject] public MessageService Message { get; set; }

        public void HandleSubmit()
        {
            if (_model.UserName == "admin" && _model.Password == "ant.design")
            {
                NavigationManager.NavigateTo("/");
                return;
            }
            NavigationManager.NavigateTo("/");
        }

        public async Task GetCaptcha()
        {
            //var captcha = await AccountService.GetCaptchaAsync(_model.Mobile);
            //await Message.Success($"Verification code validated successfully! The verification code is: {captcha}");
            await Message.Success($"Verification code validated successfully! ");
        }
    }
}