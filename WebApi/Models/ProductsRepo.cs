using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ProductsRepo : IProducts
    {
        private readonly ApplicationDbContext db;

        public ProductsRepo(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Delete(int id)
        {
            var p = db.Products.Find(id);
            if(p != null)
            {
                db.Products.Remove(p);
                db.SaveChanges();
            }
        }

        public IList<Products> GetProducts()
        {
            return db.Products.ToList();
        }

        public void Insert([FromBody] Products prod)
        {
            db.Products.Add(prod);
            db.SaveChanges();
        }

        public Products SelectProd([FromBody] Products prod)
        {
           return db.Products.Find(prod.Id);
        }
    }
}
