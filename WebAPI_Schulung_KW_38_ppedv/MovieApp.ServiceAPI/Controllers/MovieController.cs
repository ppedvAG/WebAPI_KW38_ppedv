using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.ServiceAPI.Data;
using MovieApp.Shared.Entities;
using System.Linq;

namespace MovieApp.ServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext _movieDbContext;

        public MovieController(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        //https:localhost:12345/api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _movieDbContext.Movies.ToListAsync();
        }

        /// <summary>
        /// Paging-Beispiel (Seitenweise Ergebnisse anzeigen lassen) 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet ("PaggingSample1")]
        public async Task<ActionResult<IList<Movie>>> GetMoviesWithPagging1 (int pageNumber=1, int pageSize = 3)
        {
            return await _movieDbContext.Movies.OrderBy(m => m.Title)
                                               .Skip((pageNumber - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
        }

        /// <summary>
        /// Paging-Beispiel (Seitenweise Ergebnisse anzeigen lassen) 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("PaggingSample2")]
        public async Task<ActionResult<IList<Movie>>> GetMoviesWithPagging2([FromQuery]PagingParameters pagingParameters)
        {
            return await _movieDbContext.Movies.OrderBy(m => m.Title)
                                               .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                                               .Take(pagingParameters.PageSize)
                                               .ToListAsync();
        }




        //https:localhost:12345/api/Movie/123

        [HttpGet("id")]
        public async Task<ActionResult<Movie>> GetMovie (int id)
        {
            Movie? movie = await _movieDbContext.Movies.FindAsync(id);

            //Langsamere Alternative 
            //Movie? movie2 = await _movieDbContext.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound(); 

            return Ok(movie);
        }

        //Beim Anlegen eines Datensatzes, liefern wir als Ergebnis den Datensatz + befüllte Id zurück -> daher ist der ReturnTyp ActionResult

        [HttpPost]

        //public async Task<ActionResult<Movie>> PostMovie(/*[FromQuery] */Movie movie)
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            if (movie == null)
                return NotFound();

            //UnitOfWork DesignPattern (Speichern in 2 Schritten) 
            _movieDbContext.Movies.Add(movie); //Schritt 1: Wir markieren einen Datensatz, der hinzugefügt werden soll
            await _movieDbContext.SaveChangesAsync(); //Hier wird SQL an DB gesendet

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie); //Wir verwenden hier GetMovie Methode um den Datensatz zurück zu geben.
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if ( id != movie.Id)
            {
                return BadRequest();
            }

            //Markieren den Datensatz, dass diese modifiert ist -> (ChangeTrancker festgehalten) 
            
            _movieDbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _movieDbContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if (MovieExists(id))
                {
                    return NotFound();
                }
                else
                    throw; 
            }

            return NoContent(); //204
        }

        private bool MovieExists ( int id)
        {
            return _movieDbContext.Movies.Any(x => x.Id == id);
        }

    }

    //public class PagingParameters
    //{
    //    public PagingParameters(int site, int size)
    //    {
    //        Site = site;
    //        Size = size;
    //    }

    //    public int Site { get; set; }
    //    public int Size { get; set; }
    //}


    public class PagingParameters
    {
        const int maxPageSize = 50;
        private int _pageSize = 10;
        

        public int PageNumber { get; set; } = 1;
        


        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
