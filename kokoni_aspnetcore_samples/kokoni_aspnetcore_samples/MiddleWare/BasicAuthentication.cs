using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace kokoni_aspnetcore_samples.MiddleWare
{
    public class BasicAuthentication
    {
        private string _USER_NAME { get; set; } = "root";

        private string _PASSWORD { get; set; } = "admin";

        readonly RequestDelegate _context;

        public BasicAuthentication(RequestDelegate context,string username,string password)
        {
            _context   = context;
            _USER_NAME = username;
            _PASSWORD  = password;
        }

        public async Task Invoke(HttpContext context)
        {
            // Basic認証のヘッダーを取得して判定する
            string header = context.Request.Headers["Authorization"];
            if (header != null && header.StartsWith("Basic"))
            {
                var encodedCredentials = header.Substring("Basic".Length).Trim();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var separatorIndex = credentials.IndexOf(':');
                var userName = credentials.Substring(0, separatorIndex);
                var password = credentials.Substring(separatorIndex + 1);

                if (userName == _USER_NAME && password == _PASSWORD)
                {
                    // コンテキストにユーザー情報をセットする
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, "User")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    context.User = new ClaimsPrincipal(identity);

                    await _context(context);
                    return;
                }
            }

            // ブラウザの認証ダイアログを出すには、レスポンスヘッダーにWWW-Authenticate: Baic が必要
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = 401;
        }
    }
}
