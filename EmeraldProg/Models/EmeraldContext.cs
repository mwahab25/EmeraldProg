using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmeraldProg.Models
{
    public class EmeraldContext :DbContext
    {
        public EmeraldContext(DbContextOptions<EmeraldContext> options)
            : base(options)
        { }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
