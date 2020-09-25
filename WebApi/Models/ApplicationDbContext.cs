using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public virtual DbSet<Trial> Trials { get; set; }
        public virtual DbSet<Products> Products { get; set; }
    }
    public class Trial
    {
        public int Id { get; set; }
    }
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
    }
}
