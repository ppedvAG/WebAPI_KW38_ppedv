using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.ServiceAPI.Data;
using MovieApp.Shared.Entities;

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

        [HttpPost("NewMovie")]
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
}
