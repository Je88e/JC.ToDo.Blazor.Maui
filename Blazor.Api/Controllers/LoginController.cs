using Blazor.Common.Extensions.ServerExtensions.Authorizations; 
using Blazor.Common.Helper;
using Blazor.Common.MemoryCache;
using Blazor.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blazor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly IHttpContextAccessor accessor;
        private readonly ILogger<LoginController> _logger;
        public LoginController(IHttpContextAccessor _accessor, ILogger<LoginController> logger)
        {
            _logger = logger;
            accessor = _accessor;
        }

        [HttpGet]
        [Authorize(Roles = "Jesse")]
        public IEnumerable<string> GetJwtToken()
        {
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.Name,"Jesse"),
                new Claim(JwtRegisteredClaimNames.Email,"1210333872@qq.com"),
                new Claim(JwtRegisteredClaimNames.Sub,"25")
            };

            //var token = new JwtSecurityToken(claims:claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JesseChadJesseChad"));//至少16位

            var token = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: claims,
                    expires: System.DateTime.Now.AddDays(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );


            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new string[] { jwtToken };
        }

        [HttpGet("HandleJsonWebTokenStr")]
        [Authorize(Roles = "Jesse")]
        //[Authorize(Policy = "SystemOrAdmin")]
        public MessageModel<string> HandleJsonWebTokenStr(string jwtStr)
        {
            //method1 
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);

            //method2
            var sub = User.FindFirst(q => q.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            //method3
            var name = accessor.HttpContext.User.Identity.Name;
            var claims = accessor.HttpContext.User.Claims;
            var claimTypeVal = claims.Where(q => q.Type == JwtRegisteredClaimNames.Email).Select(q => q.ToString()).ToList();
            //JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};
            claimTypeVal.Add(sub);
            claimTypeVal.Add(name);
            return new MessageModel<string>()
            {
                status = 200,
                success = true,
                response = string.Join(',', claimTypeVal),
            };
        }

        //[HttpGet("GetJwtStr/{rolename:required}")]
        [HttpGet("GetJwtStr")]
        public async Task<object> GetJwtStr(string rolename, string pwd)
        {
            if (rolename is null || pwd is null)
            {
                throw new ArgumentNullException();
            }
            if ((rolename == "Jesse" || rolename == "Admin") && MD5Helper.MD5Encrypt32(pwd) == MD5Helper.MD5Encrypt32(Appsettings.app(new string[] { "Account", "Password" })))
            {
                // 将用户id和角色名，作为单独的自定义变量封装进 token 字符串中。
                TokenModelJwt tokenModel = new TokenModelJwt { Uid = IdHelper.VipId(), Role = rolename };
                var jwtStr = JwtHelper.IssueJwt(tokenModel);//登录，获取到一定规则的 Token 令牌
                return new MessageModel<string>()
                {
                    status = 200,
                    success = true,
                    response = "登录成功",
                    msg = rolename,
                    token = jwtStr
                };
            }
            else
            {
                return new MessageModel<string>()
                {
                    status = 200,
                    success = false,
                    response = "登录失败",
                    msg = rolename
                };
            }

        }

        //[HttpPost("Login")]
        //public async Task<object> Login(UserReq req)
        //{
        //    var currCode = MemoryHelper.GetMemory(req.ValidateKey);
        //    MessageModel<string> apiResult = new MessageModel<string>() { success = false };
        //    if (currCode.ToString() != req.ValidateCode)
        //    {
        //        apiResult.msg = "验证码错误，请重新输入或刷新重试！";
        //    }
        //    else
        //    {
        //        //UserRes user = _userService.GetUsers(req);
        //        //if (string.IsNullOrEmpty(user.UserName))
        //        //{
        //        //    apiResult.Msg = "账号不存在，用户名或密码错误！";
        //        //}
        //        //else
        //        //{
        //        //    apiResult.IsSuccess = true;
        //        //    apiResult.Result = _customJWTService.GetToken(user);
        //        //}
        //    }
        //    return apiResult;

        //}

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        [HttpGet("GetValidateCodeImages")]
        public IActionResult GetValidateCodeImages(string t)
        {
            var validateCodeString = ValidateImgHelper.CreateValidateString();
            //将验证码记入缓存中
            MemoryHelper.SetMemory(t, validateCodeString, 5);
            //接收图片返回的二进制流
            byte[] buffer = ValidateImgHelper.CreateValidateCodeBuffer(validateCodeString);
            return File(buffer, @"image/jpeg");
        }
    }
}
