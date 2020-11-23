using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sales.BusinessObject;
using Sales.Service;

namespace NETCOREM3_DatabaseFirst_EF.Controllers
{
    public class EmployeeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Listado = await EmpService.Listado();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Grabar(string nombre
                                    , long salario
                                    , int edad
                                    , string perfil)
        {
            EmployeeBO employee = new EmployeeBO();
            employee.edad = edad;
            employee.salario = salario;
            employee.nombre = nombre;
            employee.perfil = perfil;

            EmployeeBO emp =  await EmpService.Insertar(employee);

            return RedirectToAction("Index");

        }
    }
}
