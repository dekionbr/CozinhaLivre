using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web;
using TCC.CL.Core.Entities;
using TCC.CL.Core.Business;
using System.Web.Caching;
using TCC.CL.Core.Enumeration;

namespace TCC.CL.Core.Seguranca
{
    public class RoleManager : RoleProvider
    {
        public const string Administrador = "Administrador";
        public const string Funcionario = "Funcionario";
        public const string Autor = "Autor";
        public const string AutorPatrocinado = "AutorPatrocinado";

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
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
            var context = HttpContext.Current;

            Usuario user = null;

            if (context == null)
                user = context.Cache.Get(username) as Usuario;

            if (user == null)
            {
                user = UsuarioBusiness.Obter(username);
                if(user == null)
                    throw new InvalidOperationException();

                if (context != null)
                    context.Cache.Add(username, user, null, Cache.NoAbsoluteExpiration, TimeSpan.FromHours(2), CacheItemPriority.Default, null);

                switch (user.Grupo)
                {
                    case Grupo.Administrador:
                        return new string[] { Grupo.Administrador.ToString(), Grupo.Funcionario.ToString(), Grupo.Autor.ToString(), Grupo.AutorPatrocinado.ToString() };
                    case Grupo.Funcionario:
                        return new string[] { Grupo.Funcionario.ToString(), Grupo.Autor.ToString(), Grupo.AutorPatrocinado.ToString() };
                    case Grupo.Autor:
                        return new string[] { Grupo.Autor.ToString() };
                    case Grupo.AutorPatrocinado:
                        return new string[] { Grupo.AutorPatrocinado.ToString() };
                    default:
                        throw new NotImplementedException();
                }
            }

            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return GetRolesForUser(username).Contains(roleName);
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
