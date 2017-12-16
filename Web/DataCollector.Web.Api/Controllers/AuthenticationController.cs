using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using DataCollector.Web.Api.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace DataCollector.Web.Api.Controllers
{
    /// <summary>
    /// Class provides an client's authentication functionality.
    /// </summary>
    /// <CreatedOn>16.12.2017 21:39</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    [Authorize]
    [Produces("application/json")]
    [Route("authorization")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration m_Configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <CreatedOn>16.12.2017 21:39</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public AuthenticationController(IConfiguration configuration)
        {
            m_Configuration = configuration;
        }


        /// <summary>
        /// Request the token by checking the credentials using the database.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <CreatedOn>16.12.2017 21:39</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (request.Username == "Jon" && request.Password == "Again, not for production use, DEMO ONLY!")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.Username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_Configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }

            return BadRequest("Could not verify username and password");
        }
    }
}
