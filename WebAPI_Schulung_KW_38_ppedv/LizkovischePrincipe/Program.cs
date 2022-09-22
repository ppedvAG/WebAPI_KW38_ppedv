namespace LizkovischePrincipe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    public class BadKirsche
    {
        public int TagDerREife = 100;

        public string GetColor()
        {
            return "rot";
        }
    }

    public class BadErdbeere : BadKirsche
    {
        public string GetColor()
        {
            return base.GetColor();
        }
    }


    #region Fruits

    public abstract class Fruits
    {
        public abstract string GetColour();
    }

    public class Kirsche : Fruits
    {
        public override string GetColour()
        {
            return "rot";
        }
    }

    #endregion


}