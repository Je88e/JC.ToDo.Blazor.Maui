using System.Threading.Tasks; 
using Microsoft.AspNetCore.Components;
using AntDesign;
using Blazor.Model.PageModel;

namespace Project.Pages.User {
  public partial class Login {
    private readonly LoginParamsType _model = new LoginParamsType();

    [Inject] public NavigationManager NavigationManager { get; set; }
         

    [Inject] public MessageService Message { get; set; }

    public void HandleSubmit() {
      if (_model.UserName == "admin" && _model.Password == "ant.design") {
        NavigationManager.NavigateTo("/");
        return;
      }

      if (_model.UserName == "user" && _model.Password == "ant.design") NavigationManager.NavigateTo("/");
    } 
  }
}