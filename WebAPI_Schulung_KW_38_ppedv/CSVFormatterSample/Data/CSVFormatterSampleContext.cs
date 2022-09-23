using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CSVFormatterSample.Models;

namespace CSVFormatterSample.Data
{
    public class CSVFormatterSampleContext : DbContext
    {
        public CSVFormatterSampleContext (DbContextOptions<CSVFormatterSampleContext> options)
            : base(options)
        {
        }

        public DbSet<CSVFormatterSample.Models.Movie> Movie { get; set; } = default!;
    }
}
