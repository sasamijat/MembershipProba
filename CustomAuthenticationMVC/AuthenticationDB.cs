using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomAuthenticationMVC.Models;
using System.Web.Configuration;
using System.Data.Entity;

namespace CustomAuthenticationMVC
{
    public class AuthenticationDB : DbContext
    {
        public AuthenticationDB() : base("AuthenticationDB")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .Map(m =>
                {
                    m.ToTable("UserRoles");
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                });
            */
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}