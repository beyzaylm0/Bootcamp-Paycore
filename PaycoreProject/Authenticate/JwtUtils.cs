using Microsoft.Extensions.Options;
using PaycoreProject.Model;
using System.Security.Claims;
using System.Text;
using System;
using PaycoreProject.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using PaycoreProject.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

using Serilog;
using NHibernate;


namespace PaycoreProject.Authenticate
{
    public class JwtUtils : IJwtUtils
    {
        protected readonly ISession session;
        protected readonly IHibernateRepository<User> hibernateRepository;
        private readonly JwtConfig jwtConfig;
        public JwtUtils(ISession session, IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this.session = session;
            this.jwtConfig = jwtConfig.CurrentValue;
            hibernateRepository = new HibernateRepository<User>(session);
        }

        public BaseResponse<AuthenticateResponse> GenerateToken(AuthenticateRequest authenticateRequest)
        {
            try
            {
                if (authenticateRequest is null)
                {
                    return new BaseResponse<AuthenticateResponse>("Please enter valid informations.");
                }

                var account = hibernateRepository.Where(x => x.Email.Equals(authenticateRequest.Email)).FirstOrDefault();
                if (account is null)
                {
                    return new BaseResponse<AuthenticateResponse>("Please validate your informations that you provided.");
                }

             

                DateTime now = DateTime.UtcNow;
                string token = GetToken(account, now);

                try
                {
                   

                    hibernateRepository.BeginTransaction();
                    hibernateRepository.Update(account);
                    hibernateRepository.Commit();
                    hibernateRepository.CloseTransaction();
                }
                catch (Exception ex)
                {
                    Log.Error("GenerateToken Update Account LastActivity:", ex);
                    hibernateRepository.Rollback();
                    hibernateRepository.CloseTransaction();
                }

                AuthenticateResponse tokenResponse = new AuthenticateResponse
                {
                    AccessToken = token,
                    ExpireTime = now.AddMinutes(jwtConfig.AccessTokenExpiration),
                    Email = account.Email,
                    SessionTimeInSecond = jwtConfig.AccessTokenExpiration * 60
                };

                return new BaseResponse<AuthenticateResponse>(tokenResponse);
            }
            catch (Exception ex)
            {
                Log.Error("GenerateToken :", ex);
                return new BaseResponse<AuthenticateResponse>("GenerateToken Error");
            }
        }
        private string GetToken(User account, DateTime date)
        {
            Claim[] claims = GetClaims(account);
            byte[] secret = Encoding.ASCII.GetBytes(jwtConfig.Secret);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                jwtConfig.Issuer,
                shouldAddAudienceClaim ? jwtConfig.Audience : string.Empty,
                claims,
                expires: date.AddMinutes(jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }
        private Claim[] GetClaims(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("AccountId", user.Id.ToString()),
                new Claim("Email",user.Email)
            };

            return claims;
        }
    }
}
