namespace DependencyInversion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICarService service = new CarService();
            
            //Tag 4,5 zum Testen
            service.Repair(new MockCar());

            //Produktiv
            service.Repair(new Car());
           
        }
    }

    #region Feste Kopplung

    //Programmierer A: 5 Tage -> (Tag 1 -> Tag 5) 
    public class OldCar
    {
        public string Marke { get; set; }
        public string Modell { get; set; }
    }


    //Programmierer B: 3 Tage (Beginnt aber an Tag 5/6  - 8/9)
    public class OldCarService
    {
        public void Repair(OldCar oldCar)
        {
            //repariere das Auto 
        }
    }


    #endregion

    #region Loose Kopplung


    public interface ICar
    {
        string Brandt { get; set; }
        string Model { get; set; }
    }

    public interface ICarService
    {
        void Repair(ICar car);
    }


    //Programmierer A: 5 Tage -> Von Tag 1 bis Tag 5
    public class Car : ICar
    {
        public string Brandt { get; set; }
        public string Model { get; set; }
    }

    public class MockCar : ICar
    {
        public string Brandt { get; set; } = "VW";
        public string Model { get; set; } = "POLO";
    }

    //Programmierer B: 3 Tage -> Von Tag 1 bis Tag 3
    public class CarService : ICarService
    {
        public void Repair(ICar car)
        {
            //Repariere Auto 
        }
    }
    #endregion
}