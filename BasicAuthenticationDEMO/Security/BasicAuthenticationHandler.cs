using BasicAuthenticationDEMO.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

using System.Threading.Tasks;

namespace BasicAuthenticationDEMO.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOption>
    {

        private readonly DemoDbContext context;
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
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());

            }

            if (!headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());

            }

            byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            string mailPassword = Encoding.UTF8.GetString(headerValueBytes);
            string[] parts = mailPassword.Split(':');
            string mail = parts[0];
            string password = parts[1];

            User user = context.Users.FirstOrDefault(x => x.Email == mail && x.Password == x.Password);


            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, mail),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
