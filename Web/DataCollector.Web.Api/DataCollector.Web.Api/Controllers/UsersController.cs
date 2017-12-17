using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Users;

namespace DataCollector.Web.Api.Controllers
{
    /// <summary>
    /// The Users controller which implements basic CRUD methods.
    /// </summary>
    /// <CreatedOn>17.12.2017 09:01</CreatedOn>
    /// <CreatedBy>dpozimski</CreatedBy>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    [Route("users")]
    [Produces("application/json")]
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
        [HttpPut("AddUser")]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            var success = await m_UsersManagementService.AddUserAsync(user);
            if (success)
                return Json(success);
            else
                return Json("Cannot add new user. Username propable exists already.");
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await m_UsersManagementService.GetUsersAsync();
            return Json(users);
        }
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserAsync(string username)
        {
            var user = await m_UsersManagementService.GetUserAsync(username);
            return Json(user);
        }
        [HttpGet("GetUsersHistory")]
        public async Task<IActionResult> GetUserLoginHistoryAsync(string username)
        {
            User user = new User() { Login = username };
            var history = await m_UsersManagementService.GetUserLoginHistoryAsync(user);
            return Json(history);
        }
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUserAsync(User user)
        {
            await m_UsersManagementService.UpdateUserAsync(user);
            return Json("OK");
        }
        [HttpPost("RecordLogout")]
        public async Task<IActionResult> RecordLogoutAsync(int sessionId)
        {
            await m_UsersManagementService.RecordLogoutTimeStampAsync(sessionId);
            return Json("OK");
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUserAsync(string username)
        {
            var success = await m_UsersManagementService.DeleteUserAsync(username);
            if (success)
                return Json(success);
            else
                return Json("User couldn't be deleted");
        }
        #endregion
    }
}
