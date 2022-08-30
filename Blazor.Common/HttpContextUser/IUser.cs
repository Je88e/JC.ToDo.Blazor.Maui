using Blazor.Model.Dto;
using System.Security.Claims;

namespace Blazor.Common.HttpContextUser
{
    public interface IUser
    {
        string Name { get; }
        int ID { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
        MessageModel<string> MessageModel { get; set; }
    }
}
