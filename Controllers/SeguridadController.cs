using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCOREM3_DatabaseFirst_EF.DataAccess;

namespace NETCOREM3_DatabaseFirst_EF.Controllers
{
    public class SeguridadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Correo, string Clave)
        {
            if (DAUser.Validar(Correo, Clave))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("Index");

        }
    }
}
