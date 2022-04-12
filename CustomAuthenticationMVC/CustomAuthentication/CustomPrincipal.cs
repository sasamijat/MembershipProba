﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;

namespace CustomAuthenticationMVC.CustomAuthentication
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        #endregion
        public IIdentity Identity
        {
            get; private set;
        }
        public bool IsInRole(string role)
        {
            if (Role == role)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }
    }
}