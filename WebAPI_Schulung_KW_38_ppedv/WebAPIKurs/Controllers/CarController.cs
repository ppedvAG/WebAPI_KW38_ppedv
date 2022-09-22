using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIKurs.Data;
using WebAPIKurs.Models;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))] //Klassenweit -> Jede Methode bekommen die jeweiligen ProducesResponseTypes
    public class CarController : ControllerBase
    {
        private readonly CarDbContext _context;

        public CarController(CarDbContext context)
        {
            _context = context;
        }

        // GET: api/Car

        /// <summary>
        /// Car-Liste
        /// </summary>
        /// <returns>IEnumerable von Autos </returns>

        [HttpGet]
        [ProducesResponseType(typeof(Car), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Car>>> GetCar()
        {
            return await _context.Car.ToListAsync();
        }

        [HttpGet("als IList")]
        [ProducesResponseType(typeof(Car), StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Car), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<Car>>> GetCar1()
        {
            return await _context.Car.ToListAsync();
        }

        // GET: api/Car/5

        /// <summary>
        /// Test
        /// </summary>
        /// <param name="id">Id von Car</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Car.FindAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return car;
        }

        // PUT: api/Car/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]

        //Mehrere [ProducesResponseType(typeof(Car), StatusCodes.??)] werden aufgelöst 
        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Car
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            _context.Car.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Car.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }
    }
}
