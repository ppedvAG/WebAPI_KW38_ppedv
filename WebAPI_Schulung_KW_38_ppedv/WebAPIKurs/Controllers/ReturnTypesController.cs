using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIKurs.Models;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnTypesController : ControllerBase
    {
        #region String-Rückgaben
        [HttpGet]
        public string GetHelloWorld()
        {
            return "Hello World";
        }

        [HttpGet("GetHelloWorld2 with ContentResult")]
        public ContentResult GetHelloWorld2()
        {
            return Content("Hello World"); 
        }
        #endregion


        #region Complex - Objects
        [HttpGet("GetComplexObject")]
        public Car GetComplexObject1()
        {
            Car car = new Car();
            car.Id = 1;
            car.Brand = "VW";
            car.Model = "Polo";
            return car; 
        }


        #endregion

        #region IActionResult + ActionResult

        //- Können Objekte als JSON übertragen + HTTP-Code kann zurück gegeben werden
        //- IActionResult bei -> Post / Put / Delete
        //- ActionResukt bei -> Get



        #region Synchrone Methoden
        [HttpGet("GetCarWith_IActionResult")]
        public IActionResult GetCarWith_IActionResult()
        {
            Car? car = new Car();
            car.Id = 1;
            car.Brand = "Porsche";
            car.Model = "924";

            if (car == null)
                return NotFound(); //404

            if (car.Brand == "Porsche")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben"); // 400
            }

            return Ok(car);
        }


        [HttpGet("GetCarWith_ActionResult")]
        public ActionResult GetCarWith_ActionResult()
        {
            Car? car = new Car();
            car.Id = 1;
            car.Brand = "Porsche";
            car.Model = "924";

            if (car == null)
                return NotFound(); //404

            if (car.Brand == "Porsche")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben"); // 400
            }

            return Ok(car);
        }

        #endregion


        #region Assynchrone Methoden

        [HttpGet("GetCarWith_IActionResultAsync")]
        public async Task<IActionResult> GetCarWith_IActionResultAsnyc()
        {
            //Asyhcrone Mock-Methode (Eigentlich wird hier EFCore mit seinen asychronen Methoden 
            await Task.Delay(1000);

            Car? car = new Car();
            car.Id = 1;
            car.Brand = "Porsche";
            car.Model = "924";

            if (car == null)
                return NotFound(); //404

            if (car.Brand == "Porsche")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben"); // 400
            }

            return Ok(car);
        }


        [HttpGet("GetCarWith_ActionResultAsync")]
        public async Task<ActionResult> GetCarWith_ActionResultAsnyc()
        {
            //Asyhcrone Mock-Methode (Eigentlich wird hier EFCore mit seinen asychronen Methoden 
            await Task.Delay(1000);

            Car? car = new Car();
            car.Id = 1;
            car.Brand = "Porsche";
            car.Model = "924";

            if (car == null)
                return NotFound(); //404

            if (car.Brand == "Porsche")
            {
                return BadRequest("Eine Fehlermeldung kann man hier eingeben"); // 400
            }

            return Ok(car);
        }

        #endregion

        #region IEnumerable 

        [HttpGet("sync")]
        public IEnumerable<Car> GetCars()
        {
            IList<Car> cars = new List<Car>();
            cars.Add(new Car() { Id = 1, Brand = "VW", Model = "POLO" });
            cars.Add(new Car() { Id = 2, Brand = "Audi", Model = "A4" });
            cars.Add(new Car() { Id = 3, Brand = "Volvo", Model = "Irgendwas" });
            cars.Add(new Car() { Id = 4, Brand = "Citrön", Model = "Ente" });


            IEnumerable<Car> carEnumerable = cars.AsEnumerable();



            foreach(Car currentCar in carEnumerable)
            {
                Task task = Task.Delay(1000);
                task.Wait(); //ohne Async/Await müsste man Delay so anprogrammieren. 

                yield return currentCar; 
            }
        }
        #endregion


        #region IAsyncEnumerable geht nur in Verbindung mit EF Core 

        #endregion
        #endregion
    }
}
