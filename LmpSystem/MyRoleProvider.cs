using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using LmpSystem.Repository;
using LmpSystem.Models;

namespace LmpSystem
{
    public class MyRoleProvider : RoleProvider
    {
        //MyDBContext db = new MyDBContext();
        readonly UnitOfWork db = new UnitOfWork(new LmpSystemEntities());

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
        
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            //select role from Users join UserRole on Users.UserId = UserRole.UserId where users.UserName = 'ltanh'
            //select role from Users u, UserRole r where u.UserId = r.UserId and u.UserName = 'ltanh'
            //select role from UserRole where UserId in (select UserId from Users where UserName = 'ltanh')
            //throw new NotImplementedException();

            //var roles = db.Context.Database.SqlQuery<string>("select role from Users join UserRole on Users.UserId = UserRole.UserId where users.UserName = '" + username + "'").ToArray();
            var roles = db.Context.Database.SqlQuery<string>("Select UserRole from [User] Where Username = '" + username + "'").ToArray();
            return roles;        
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}