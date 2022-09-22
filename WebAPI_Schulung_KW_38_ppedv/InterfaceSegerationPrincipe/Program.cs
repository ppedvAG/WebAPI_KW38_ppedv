namespace InterfaceSegerationPrincipe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    #region Bad Sample
    public interface IVehicle
    {
        void Drive();
        void Fly();
        void Swim();
    }

    public class MultiVehicle : IVehicle
    {
        public void Drive()
        {
            
        }

        public void Fly()
        {
            
        }

        public void Swim()
        {
            
        }
    }

    public class AmphibischesFahrzeug : IVehicle
    {
        public void Drive()
        {
            Console.WriteLine("kann fahren");
        }

        public void Swim()
        {
            Console.WriteLine("kann schwimmen");
        }

        public void Fly()
        {
            throw new NotImplementedException();
        }

        
    }
    #endregion


    public interface ISwim
    {
        void Swim();
    }

    public interface IDrive
    {
        void Drive();
    }

    public interface IFly
    {
        void Fly(); 
    }

    public class MulitVehicle2 : ISwim, IFly, IDrive
    {
        public void Drive()
        {
            Console.WriteLine("Drive");
        }

        public void Fly()
        {
            Console.WriteLine("Fly");
        }

        public void Swim()
        {
            //cw + tab + tab -> Console.WriteLine();
            Console.WriteLine("Swim");
        }
    }
}