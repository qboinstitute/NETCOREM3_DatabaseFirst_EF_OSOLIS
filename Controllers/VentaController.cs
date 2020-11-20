using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCOREM3_DatabaseFirst_EF.DataAccess;
using NETCOREM3_DatabaseFirst_EF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NETCOREM3_DatabaseFirst_EF.Controllers
{
    public class VentaController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.ListadoCliente = DACustomer.Listado();
            ViewBag.ListadoProducto = DAProduct.Listado();

            string tcEuro= String.Empty;
            using (var httpClient = new HttpClient())
            {
                using (var respuesta = await httpClient.GetAsync("https://api.exchangeratesapi.io/latest?base=USD"))
                {
                    string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
                    tcEuro = (string )JObject.Parse(apiRespuesta)["rates"]["EUR"];
                }
            }

            ViewBag.TipoCambioEUR = tcEuro;

            return View();
        }

        public IActionResult ListadoProducto()
        {
            List<OrderItem> listado;
            var productos = HttpContext.Session.GetString("listaProducto");
            if (productos==null)
                listado = new List<OrderItem>();
            else
                listado = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            ViewBag.ProductosAgregados = listado;
            ViewBag.ListadoProducto = DAProduct.Listado();
            return PartialView();
        }

        public IActionResult QuitarProductoOrden(int productID)
        {
            List<OrderItem> listado;
            var productos = HttpContext.Session.GetString("listaProducto");
            if (productos == null)
                listado = new List<OrderItem>();
            else
                listado = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            OrderItem item = listado.Where(x => x.ProductId == productID).FirstOrDefault();
            listado.Remove(item);
            HttpContext.Session.SetString("listaProducto", JsonConvert.SerializeObject(listado));
            return Json("OK");

        }


        [HttpPost]
        public IActionResult AgregarProducto(int productID, decimal unitPrice, int quantity)
        {
            List<OrderItem> listado;
            var productos = HttpContext.Session.GetString("listaProducto");
            if (productos == null)
                listado = new List<OrderItem>();
            else
                listado = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            if (listado.Where(x => x.ProductId == productID).Count() > 0)
            {
                return Json("DUP");
            }

            OrderItem detalle = new OrderItem();
            detalle.ProductId = productID;
            detalle.UnitPrice = unitPrice;
            detalle.Quantity = quantity;

            listado.Add(detalle);

            HttpContext.Session.SetString("listaProducto", JsonConvert.SerializeObject(listado));

            return Json("OK");

        }

        public async Task<IActionResult> ListadoOrdenes()
        {
            ViewBag.Listado = await DAOrder.listadoAsync();
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarOrden(int customerID
                                        , DateTime orderDate
                                        , string orderNumber)
        {
            Order cabecera = new Order();
            cabecera.CustomerId = customerID;
            cabecera.OrderDate = orderDate;
            cabecera.OrderNumber = orderNumber;

            List<OrderItem> detalle = new List<OrderItem>();
            var productos = HttpContext.Session.GetString("listaProducto");
            detalle = JsonConvert.DeserializeObject<List<OrderItem>>(productos);

            bool exito = await DAOrder.Insertar(cabecera, detalle);
            return Json(exito);

        }
    }
}
