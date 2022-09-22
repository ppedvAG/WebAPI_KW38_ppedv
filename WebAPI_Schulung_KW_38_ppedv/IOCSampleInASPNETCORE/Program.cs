using Microsoft.Extensions.DependencyInjection;

namespace IOCSampleInASPNETCORE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //IServiceCollection + ServiceCollection 
            //Die ServiceCollection bekomme alle Dienste, die später in einer ASP.NET Core wieder verwenden werden

            IServiceCollection services = new ServiceCollection();

            //Hinzufügen eines Dienstes in den IOC Container
            services.AddSingleton<IDateTimeService, DateTimeService>();

            //weitere Dienste hinzugügen....


            //wenn wir fertig mit der Initialisierung sind -> Build 

            IServiceProvider serviceProvider = services.BuildServiceProvider();



            //Abrufen eines Dienstes 

            //NULL wird zurück gegeben, wenn Service nicht gefunden wurde 
            IDateTimeService? currentDateTimeService = serviceProvider.GetService<DateTimeService>();
            
            //Exception wird ausgegeben, wenn Service nicht gefunden wurde
            IDateTimeService currentDateTimeService2 = serviceProvider.GetRequiredService<DateTimeService>();
        }
    }


    public interface IDateTimeService
    {
        string GetCurrentTime();
    }

    public class DateTimeService : IDateTimeService
    {
        private DateTime _currentTime;  
        //Ctor + tab + tab -> Konstruktor 

        public DateTimeService()
        {
            _currentTime = DateTime.Now;
        }

        public string GetCurrentTime()
        {
            return _currentTime.ToString();
        }
    }
}