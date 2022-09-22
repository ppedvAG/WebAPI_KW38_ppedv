using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIKurs.Services;

namespace WebAPIKurs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    //Pro Request wird der Controller via Factory-Klasse neu erstellt. 

    public class MyDateTimeController : ControllerBase
    {
        //private readonly IDateTimeService _dateTimeService;
        private readonly ILogger<MyDateTimeController> _logger;

        //Konstruktor Injections
        public MyDateTimeController(ILogger<MyDateTimeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string AktuelleUhrzeit([FromServices]IDateTimeService service)
        {
            _logger.LogInformation("MyDateTime->Get->AktuelleUhrzeit");

            return service.GetCurrentDateTime();
        }

        [HttpGet("WeitereUhrzeit1")]
        public string WeitereUhrzeit()
        {
            IDateTimeService timeService = HttpContext.RequestServices.GetService<IDateTimeService>();


            return timeService.GetCurrentDateTime();
        }

        [HttpGet("/WeitereUhrzeit2")]
        public string WeitereUhrzeit2()
        {
            IDateTimeService timeService = HttpContext.RequestServices.GetService<IDateTimeService>();


            return timeService.GetCurrentDateTime();
        }
    }
}
