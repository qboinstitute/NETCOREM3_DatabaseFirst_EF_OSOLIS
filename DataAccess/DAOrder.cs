using Microsoft.EntityFrameworkCore;
using NETCOREM3_DatabaseFirst_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst_EF.DataAccess
{
    public class DAOrder
    {

        public static async Task<List<Order>> listadoAsync()
        {
            using (var data = new SalesContext())
            {
                return await data.Orders.OrderByDescending(x=>x.OrderDate).ToListAsync();
            }
        }


        public static async Task<bool> Insertar(Order cabecera, List<OrderItem> detalle)
        {
            bool exito = true;

            try
            {
                using (var data = new SalesContext())
                {
                    await data.Orders.AddAsync(cabecera);//Id no existe
                    await data.SaveChangesAsync();//Id se genera y ya existe

                    int newOrderID = cabecera.Id;
                    decimal totalAmount = 0;
                    foreach (var item in detalle)
                    {
                        totalAmount = totalAmount * (item.UnitPrice * item.Quantity);
                        item.OrderId = newOrderID;
                    }
                    await data.OrderItems.AddRangeAsync(detalle);
                    cabecera.TotalAmount = totalAmount;
                    await data.SaveChangesAsync();
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
