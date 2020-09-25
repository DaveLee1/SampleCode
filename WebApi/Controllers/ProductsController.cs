using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProducts dal;
        public ProductsController(IProducts dal)
        {
            this.dal = dal;
        }

        [HttpGet]
        public IEnumerable<Products> GetProducts()
        {
            return dal.GetProducts();
        }

        [HttpPost]
        public void Insert([FromBody] Products prod)
        {
            dal.Insert(prod);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            dal.Delete(id);
        }

    }
}
