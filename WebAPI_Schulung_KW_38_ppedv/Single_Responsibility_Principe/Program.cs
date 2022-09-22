using System.Xml.Linq;

namespace Single_Responsibility_Principe
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class BadEmployee
    {
        //Repräsentiert ein Datensatz
        public int Id { get; set; }
        public string Name { get; set; }


        //Methode schreibt in DB
        public void InsertEmployeeToTable(BadEmployee employee)
        {
            //any Code
        }


        //Ausgabe (Service-Layer, Frontend-Layer) 
        public void GenerateReport()
        {
            //any Code
        }
    }


    #region Verbesserte Variante
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    //DataAccess Layer -> CRUD(Create, Read, Updata, Delete)
    public class EmployeeRepository //Klasse die mit einer Tabelle kommuniziert
    {
        public void Insert(Employee employee)
        {
            //Datenbank
        }
    }

    //Generate Report 
    public class EmployeeService
    {
        //Report zu einem Mitarbeiter wird generiert 
        public void GenerateReport()
        {

        }
    }
    #endregion
}