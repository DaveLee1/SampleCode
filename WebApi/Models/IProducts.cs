using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public interface IProducts
    {
        void Insert(Products prod);
        IList<Products> GetProducts();
        void Delete(int id);
        Products SelectProd(Products prod);
    }
}
