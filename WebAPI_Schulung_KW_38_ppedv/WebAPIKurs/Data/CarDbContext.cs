using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPIKurs.Models;

namespace WebAPIKurs.Data
{
    public class CarDbContext : DbContext
    {
        public CarDbContext (DbContextOptions<CarDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebAPIKurs.Models.Car> Car { get; set; } = default!;
    }
}
