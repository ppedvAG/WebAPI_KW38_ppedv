using Microsoft.EntityFrameworkCore;
using MovieApp.Shared.Entities;
using System.Security.Cryptography.X509Certificates;

namespace MovieApp.ServiceAPI.Data
{
    //DbContext Klasse -> ist die Zugriffsklasse von EFCore auf Datenbank 
    //DbContext beinhaltet -> ConnectionString, Connection, Timeout,
    //mithilfe von DBSets können wir den Umfang des Datenbankzugriffes definieren 

    public class MovieDbContext : DbContext
    {
        
        public MovieDbContext(DbContextOptions<MovieDbContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        //Name der Property ('Movies') wird als Tabellennamen verwendet
        public DbSet<Movie> Movies { get; set; }
    }
}
