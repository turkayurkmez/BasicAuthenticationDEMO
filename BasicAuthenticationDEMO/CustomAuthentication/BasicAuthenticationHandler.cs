using BasicAuthenticationDEMO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using System.Threading.Tasks;

namespace BasicAuthenticationDEMO.CustomAuthentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOption>
    {
        DemoDbContext context;
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOption> options,
                                          ILoggerFactory logger,
                                          UrlEncoder urlEncoder,
                                          ISystemClock systemClock,
                                          DemoDbContext context) : base(options, logger, urlEncoder, systemClock)
        {
            this.context = context;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authenticate"))
            {

            }
            if (!AuthenticationHeaderValue.TryParse("Authenticate", out AuthenticationHeaderValue headerValue))
            {

            }

            if (headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {

            }

            byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            var mailPassword = Encoding.UTF8.GetString(headerValueBytes);
            var parts = mailPassword.Split(':');
            string mail = parts[0];
            string password = parts[1];

            var user = context.Users.FirstOrDefault(x => x.Email == mail && x.Password == x.Password);


            var claims = new[]
            {
                new Claim(ClaimTypes.Name, mail),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
