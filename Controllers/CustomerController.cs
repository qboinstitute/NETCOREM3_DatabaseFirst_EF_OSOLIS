using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NETCOREM3_DatabaseFirst_EF.DataAccess;
using NETCOREM3_DatabaseFirst_EF.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace NETCOREM3_DatabaseFirst_EF.Controllers
{

    public class CustomerController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public CustomerController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReporteXLS()
        {
            string WebRootPah = _hostingEnvironment.WebRootPath;
            string fileName = @"ReporteClientes.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, fileName);
            FileInfo file = new FileInfo(Path.Combine(WebRootPah, fileName));
            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(WebRootPah, fileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workBook = new XSSFWorkbook();
                ISheet excelSheet = workBook.CreateSheet("Reportes");

                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("CustomerId");
                row.CreateCell(1).SetCellValue("FirstName");
                row.CreateCell(2).SetCellValue("LastName");
                row.CreateCell(3).SetCellValue("City");

                List<Customer> listado = DACustomer.Listado();
                int contador = 1;
                string firstName = string.Empty;
                foreach (var item in listado)
                {
                    if (item.FirstName.Length > 100)
                        firstName = item.FirstName.Substring(0, 100);
                    else
                        firstName = item.FirstName;

                    row = excelSheet.CreateRow(contador);
                    row.CreateCell(0).SetCellValue(item.Id);
                    row.CreateCell(1).SetCellValue(firstName);
                    row.CreateCell(2).SetCellValue(item.LastName);
                    row.CreateCell(3).SetCellValue(item.City);
                    contador++;
                }

                workBook.Write(fs);
            }

            using (var fs = new FileStream(Path.Combine(WebRootPah, fileName), FileMode.Open))
            {
                fs.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public IActionResult ReporteCSV()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Id,FirstName,LastName");
            List<Customer> listado = DACustomer.Listado();
            foreach (var item in listado)
            {
                builder.AppendLine($"{item.Id},{item.FirstName},{item.LastName}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Customers.csv");
        }

        public IActionResult ListadoBusqueda(string city)
        {
            ViewBag.ListadoClientes = DACustomer.Listado(city);
            return PartialView("Listado");
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
            else
            {
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
