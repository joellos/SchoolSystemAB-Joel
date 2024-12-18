using SchoolSystemAB.SchoolFunctions;

namespace SchoolSystemDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SchoolLogin login = new SchoolLogin();
            login.Login();
        }
    }
}
