using DataCollector.Server.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Server.DataAccess.Context
{
    /// <summary>
    /// Klasa inicjalizująca nową bazę danych.
    /// </summary>
    class DataCollectorContextInitializer : DropCreateDatabaseIfModelChanges<DataCollectorContext>
    {
        protected override void Seed(DataCollectorContext context)
        {
            CreateUsers(context);
            CreateProcedures(context);

            base.Seed(context);
        }

        private void CreateUsers(DataCollectorContext context)
        {
            var admin = new User();
            admin.Login = "admin";
            admin.AssignPassword("admin");
            admin.Role = UserRole.Administrator;
            context.Users.Add(admin);

            var viewer = new User();
            viewer.Login = "viewer";
            viewer.AssignPassword("viewer");
            viewer.Role = UserRole.Viewer;
            context.Users.Add(viewer);
        }

        private void CreateProcedures(DataCollectorContext context)
        {
            context.Database.ExecuteSqlCommand(Properties.Resources.SPU_GetMeasurePoints);
            context.Database.ExecuteSqlCommand(Properties.Resources.SPU_GetSphereMeasurePoints);
        }
    }
}
