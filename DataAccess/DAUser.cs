using NETCOREM3_DatabaseFirst_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst_EF.DataAccess
{
    public class DAUser
    {
        public static User Validar_Obj(string correo, string clave)
        {
            User usuario = new User();

            try
            {
                using (var data = new SalesContext())
                {
                    usuario = data.Users.Where(x => x.state == true && x.email.Equals(correo) && x.pwd.Equals(clave)).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                usuario = null;
            }

            return usuario;

        }



        public static bool Validar(string correo, string clave)
        {
            bool exito = true;

            try
            {
                using (var data  = new SalesContext())
                {
                    exito = (data.Users.Where(x => x.state==true && x.email.Equals(correo) && x.pwd.Equals(clave)).Count() > 0) ? true : false;
                }
            }
            catch (Exception)
            {
                exito = false;
            }

            return exito;
        
        }


    }
}
