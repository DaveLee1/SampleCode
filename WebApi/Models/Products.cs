using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string ProdName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
