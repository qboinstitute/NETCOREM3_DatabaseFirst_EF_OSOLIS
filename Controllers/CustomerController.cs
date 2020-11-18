using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCOREM3_DatabaseFirst_EF.DataAccess;
using NETCOREM3_DatabaseFirst_EF.Models;

namespace NETCOREM3_DatabaseFirst_EF.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Listado()
        {
            ViewBag.ListadoClientes = DACustomer.Listado();
            return PartialView();
        }

        public IActionResult Grabar(int idCliente, 
                                    string nombres,
                                    string apellidos,
                                    string pais,
                                    string ciudad,
                                    string telefono) 
        {
            Customer customer = new Customer();
            customer.FirstName = nombres;
            customer.LastName = apellidos;
            customer.City = ciudad;
            customer.Country = pais;
            customer.Phone = telefono;

            bool exito = true;

            if (idCliente == -1) //Es un nuevo registro
                exito = DACustomer.Insertar(customer);
            else {
                //Es una actualización
                customer.Id = idCliente;
                exito = DACustomer.Actualizar(customer);            
            }

            return Json(exito);
        }

        [HttpPost]
        public IActionResult Eliminar(int idCliente)
        {
            bool exito = DACustomer.Eliminar(idCliente);
            return Json(exito);
        }

        public IActionResult Obtener(int idCliente)
        {
            Customer customer = DACustomer.Obtener(idCliente);
            return Json(customer);
        }



    }
}
