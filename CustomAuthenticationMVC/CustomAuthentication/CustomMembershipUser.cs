using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomAuthenticationMVC.Models;
using System.Web.Security;

namespace CustomAuthenticationMVC.CustomAuthentication
{
    public class CustomMembershipUser : MembershipUser
    {
        
        #region User Properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
        #endregion
        

        public CustomMembershipUser(User user) : base("CustomMembership", user.Username, user.UserId, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Role = user.Role;
        }
    }
}
