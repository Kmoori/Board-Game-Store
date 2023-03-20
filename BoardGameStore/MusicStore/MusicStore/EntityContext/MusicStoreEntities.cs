using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace MusicStore.EntityContext
{
    public class MusicStoreEntities : DbContext
    {
        public DbSet<Termekek> Termekek { get; set; }


        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        //javított autentikáció
        public DbSet<Mvc3ToolsUpdateWeb_Default.Models.LogOnModel> logOnModels { get; set; }
        public MusicStoreEntities()           
        {

        }
    }
}