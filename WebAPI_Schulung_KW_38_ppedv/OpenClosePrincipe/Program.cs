namespace OpenClosePrincipe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

   

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }

    #region BadReport
    public class BadReportGenerator
    {
        public BadReportGenerator()
        {

        }

        public void Generate (Employee em)
        {
            if (em.Type == 1)
            {
                //PDF wird generiert
            }
            else if (em.Type == 2)
            {
                //Crystal Report wird genriert
            }
            else if (em.Type == 3)
            {

            }

        }
    }
    #endregion

    #region Gute Variante

    public abstract class BaseReportGenerator
    {
        public abstract void Generate (Employee em);    
    }

    public class PDFGenerator : BaseReportGenerator
    {
        //Shortcut -> 1.) STRG + '.' 2.) [Enter] 

        public override void Generate(Employee em)
        {
            //PDF wird erstellt
        }
    }

    public class CRGenerator : BaseReportGenerator
    {
        //Shortcut -> 1.) STRG + '.' 2.) [Enter] 

        public override void Generate(Employee em)
        {
            //PDF wird erstellt
        }
    }
    #endregion


}