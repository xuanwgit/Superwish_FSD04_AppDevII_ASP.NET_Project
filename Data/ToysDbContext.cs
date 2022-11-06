using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Superwish_FSD04_AppDevII_ASP.NET_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Superwish_FSD04_AppDevII_ASP.NET_Project.Data
{
    public class ToysDbContext : IdentityDbContext
    {
        public ToysDbContext(DbContextOptions<ToysDbContext> options) : base(options){}
        public DbSet<Cart_Item> Cart_Items => Set<Cart_Item>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
   
    }

    
}