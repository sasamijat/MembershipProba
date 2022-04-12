namespace CustomAuthenticationMVC.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CustomAuthenticationMVC.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<CustomAuthenticationMVC.AuthenticationDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CustomAuthenticationMVC.AuthenticationDB context)
        {
            if (context.Roles.FirstOrDefault() != null)
                return;
                context.Roles.AddOrUpdate
                    (
                    new Role { RoleName = "Administrator"},
                    new Role { RoleName = "Buyer"},
                    new Role { RoleName = "Seller"}
                    );

                   
            

        }
    }
}
