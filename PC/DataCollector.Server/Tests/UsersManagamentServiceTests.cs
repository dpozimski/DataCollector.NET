using DataCollector.Server.DataAccess.Models;
using DataCollector.Server.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DataCollector.Server.Tests
{
    /// <summary>
    /// Klasa testująca <see cref="UsersManagementService"/>.
    /// </summary>
    public class UsersManagamentServiceTests : IDisposable
    {
        private readonly UsersManagementService managementService;
        private readonly User user;

        public UsersManagamentServiceTests()
        {
            managementService = new UsersManagementService(TestModelsFactory.TestConnectionString);
            user = new User()
            {
                Login = "test1",
                Password = "123"
            };
        }

        [Fact]
        public void GetUsersTest()
        {
            var list = managementService.GetUsers();
            Assert.True(list.All(s => s.Role == UserRole.Administrator || s.Role == UserRole.Viewer));
        }

        [Fact]
        public void GetUsersLoginHistoryTest()
        {
            var adminUser = managementService.GetUser("admin");
            var list = managementService.GetUserLoginHistory(adminUser.Item1);
            Assert.True(list.Count > 0);
        }
        
        [Fact]
        public void UpdateUserTest()
        {
            string name = "asd" + DateTime.Now.Ticks;
            var user = managementService.GetUser("viewer");
            user.Item1.FirstName = name;
            managementService.UpdateUser(user.Item1);
            user = managementService.GetUser("viewer");
            Assert.Equal(name, user.Item1.FirstName);
        }

        [Fact]
        public void AddUserTimeStampHistory()
        {
            var user = managementService.GetUser("admin");
            managementService.RecordLogoutTimeStamp(user.Item2);
            var list = managementService.GetUserLoginHistory(user.Item1);
            Assert.True(list.Any(s => s.ID == user.Item2));
        }

        [Theory]
        [InlineData("admin", "admin")]
        [InlineData("viewer", "viewer")]
        public void ValidateCredentialsTest(string login, string password)
        {
            Assert.True(managementService.ValidateCredentials(login, password));
        }

        [Fact]
        public void AddNonExistsUserTest()
        {
            bool success = managementService.AddUser(user);
            Assert.True(success);
        }

        public void Dispose()
        {
            managementService.DeleteUser(user.Login);
        }
    }
}
