using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users;

namespace DataCollector.Web.Controllers.API
{
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        #region [Private Fields]
        private IUsersManagementService m_UsersManagementService;
        #endregion

        #region [ctor]
        public UsersController(IUsersManagementService usersManagementService)
        {
            m_UsersManagementService = usersManagementService;
        }
        #endregion

        #region [Public Methods]
        [HttpGet("[action]")]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await m_UsersManagementService.GetUsersAsync();
        }
        [HttpGet("[action]")]
        public async Task<UserSession> GetUser(string username)
        {
            return await m_UsersManagementService.GetUserAsync(username);
        }
        #endregion


    }
}