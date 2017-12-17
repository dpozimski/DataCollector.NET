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
using Users;
using System.Threading.Tasks;

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
        #region [Private Fields]
        private readonly IConfiguration m_Configuration;
        private readonly IUsersManagementService m_UsersManagementService;
        #endregion

        #region [ctor]
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="usersManagementService">the users management service</param>
        /// <CreatedOn>16.12.2017 21:39</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        public AuthenticationController(IConfiguration configuration, IUsersManagementService usersManagementService)
        {
            m_Configuration = configuration;
            m_UsersManagementService = usersManagementService;
        }
        #endregion

        #region [Public Methods]
        /// <summary>
        /// Request the token by checking the credentials using the database.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <CreatedOn>16.12.2017 21:39</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        [HttpPost("RequestToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestTokenAsync([FromBody] TokenRequest request)
        {
            //validate credentials
            var isValid = await m_UsersManagementService.ValidateCredentialsAsync(request.Username, request.Password);
            //badrequest if credentials are invalid
            if(!isValid)
                return BadRequest("The username or password is invalid");
            else
            {
                //produce a jwt tpken
                var token = ProduceJwtToken(request.Username);
                //return json message with token inside
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
        }
        #endregion

        #region [Private Methods]        
        /// <summary>
        /// Produces the JWT token using verified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        /// <CreatedOn>17.12.2017 08:44</CreatedOn>
        /// <CreatedBy>dpozimski</CreatedBy>
        private JwtSecurityToken ProduceJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_Configuration["SecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: "softpower.pl",
                audience: "softpower.pl",
                claims: new Claim[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
        }
        #endregion
    }
}
