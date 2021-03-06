﻿using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace AspNetIdentity.WebApi.Providers
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
    
        private readonly string _issuer = string.Empty;

        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return null;
        }


        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="protectedText">the encrypted JWT</param>
        /// <returns>A Claims principal sourced from the jwt </returns>
        public System.Security.Claims.ClaimsPrincipal Validate(string protectedText)
        {
        
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];

            string symmetricKeyAsBase64 = ConfigurationManager.AppSettings["as:AudienceSecret"];

            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken = null;
            System.Security.Claims.ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(protectedText, new TokenValidationParameters() { ValidAudience = audienceId, IssuerSigningKey = signingKey.SigningKey, ValidIssuer = _issuer }, out validatedToken);
            if (validatedToken.ValidTo < DateTime.Now.ToUniversalTime())
            {
                return null;
            }
            return claimsPrincipal;         
        }
    }
}