using NETCOREM3_DatabaseFirst_EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCOREM3_DatabaseFirst_EF.DataAccess
{
    public class DAProduct
    {
        static public List<Product> Listado()
        {
            using (var data = new SalesContext())
            {
                return data.Products.OrderBy(x=>x.ProductName).ToList();
            }
        }
    }
}
