using Microsoft.EntityFrameworkCore;
using NETCOREM3_DatabaseFirst_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst_EF.DataAccess
{
    public class DACustomer
    {
        public static List<Customer> Listado()
        {
            using (var data = new SalesContext())
            {
                return data.Customers.OrderBy(x=>x.Country).ToList();
            }        
        }

        public static async Task<List<Customer>> ListadoAsync()
        {
            using (var data = new SalesContext())
            {
                return await data.Customers.ToListAsync();
            }
        }

        public static List<Customer> ListadoConSP()
        {
            using (var data = new SalesContext())
            {
                return data.Customers.FromSqlRaw("SP_SEL_CUSTOMER").ToList();
            }
        }

        public static Customer Obtener(int id)
        {

            using (var data = new SalesContext())
            {
                return data.Customers.Where(x => x.Id == id).FirstOrDefault();
            }
        }

        public static bool Insertar(Customer customer)
        {
            bool exito = true;

            try
            {
                using (var data = new SalesContext())
                {
                    data.Customers.Add(customer);
                    data.SaveChanges();
                }

            }
            catch (Exception)
            {
                exito = false;
            }
            return exito;
        }


        public static bool Actualizar(Customer customer)
        {
            bool exito = true;

            try
            {
                using (var data = new SalesContext())
                {
                    Customer customerNow = data.Customers.Where(z => z.Id == customer.Id).FirstOrDefault();

                    customerNow.FirstName = customer.FirstName;
                    customerNow.LastName = customer.LastName;
                    customerNow.City = customer.City;
                    customerNow.Country = customer.Country;
                    customerNow.Phone = customer.Phone;
                    data.SaveChanges();
                }
            }
            catch (Exception)
            {

                exito = false;
            }

            return exito;
        }

        public static bool Eliminar(int id)
        {
            bool exito = true;
            try
            {
                using (var data = new SalesContext())
                {
                    Customer customer = data.Customers.Where(p => p.Id == id).FirstOrDefault();

                    data.Customers.Remove(customer);
                    data.SaveChanges();
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
