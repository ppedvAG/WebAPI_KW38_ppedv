using WebAPIKurs.Models;

namespace WebAPIKurs.Data
{
    public static class SeedData
    {
        public static void Initialize(CarDbContext context)
        {
            //Wenn Tabelle leer ist, wird Initialisierung ausgeführt. 
            if (!context.Car.Any())
            {

                IList<Car> cars = new List<Car>();

                cars.Add(new Car() { Id = 1, Brand = "VW", Model = "Polo" });
                cars.Add(new Car() { Id = 2, Brand = "Audi", Model = "Quattro" });
                cars.Add(new Car() { Id = 3, Brand = "Prosche", Model = "999" });

                context.Car.AddRange(cars.ToArray());

                context.SaveChanges();
            }
        }
    }
}
