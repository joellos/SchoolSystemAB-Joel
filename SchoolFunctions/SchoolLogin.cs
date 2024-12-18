using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSystemAB.Models;

namespace SchoolSystemAB.SchoolFunctions
{
    public class SchoolLogin
    {
        public SchoolLogin()
        {
        }
        SchoolMenu menu = new SchoolMenu();
        private bool ValidLogin(string? username, string password)
        {
            using (var context = new SchoolDbContext())
            {
                var tempUser = context.Users.FirstOrDefault(u => u.Username == username);
                if (tempUser != null && tempUser.Password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Login()
        {
            var attempts = 3;
            while (attempts != 0)
            {
                using (var context = new SchoolDbContext())
                {
                    Console.WriteLine("Välkommen till skolans inloggningssystem!");
                    Console.WriteLine("Vänligen ge användarnamn.");
                    string username = Console.ReadLine();
                    Console.WriteLine("Vänligen ge lösenord.");
                    string password = ReadPassword();

                    if (ValidLogin(username, password))
                    {
                        var user = context.Users.FirstOrDefault(u => u.Username == username);
                        if (user != null && user.UserType == "Admin")
                        {
                            Console.WriteLine("Inloggning lyckades! Välkommen Admin!");
                            Console.WriteLine("Vill du gå till (1) Admin menyn eller (2) Vanliga menyn?");
                            string choice = Console.ReadLine();
                            if (choice == "1")
                            {
                                AdminSchoolMenu adminMenu = new AdminSchoolMenu();
                                adminMenu.DisplayAdminMenu();
                            }
                            else
                            {
                                menu.DisplayMenu();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Inloggning lyckades!");
                            menu.DisplayMenu();
                        }
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Inloggning misslyckades. Vänligen försök igen.");
                        attempts--;
                    }
                }
            }
            Console.WriteLine("Du har använt upp dina försök. Programmet avslutas nu.");
            Environment.Exit(0);
        }

        private string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password.Append(keyInfo.KeyChar);
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }

        public void Logout()
        {
            Console.WriteLine("Du har loggat ut.");
            menu.DisplayMenu();
            return;
        }
    }
}
