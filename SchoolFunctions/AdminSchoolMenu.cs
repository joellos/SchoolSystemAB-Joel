using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSystemAB.Models;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystemAB.SchoolFunctions
{
    public class AdminSchoolMenu
    {
        SchoolMenu userMenu = new SchoolMenu();
        public void DisplayAdminMenu()
        {
            while (true)
            {
                Console.WriteLine("Välkommen till administratör SchoolAB! Vänligen välj en av de följande alternativen för att lägga till eller ta bort eleverna och personal på skolan.");
                Console.WriteLine("1. Lägg till elev:");
                Console.WriteLine("2. Ta bort elev:");
                Console.WriteLine("3. Lägg till anställd:");
                Console.WriteLine("4. Ta bort anställd:");
                Console.WriteLine("5. Logga ut.");
                Console.WriteLine("6. Avsluta programmet");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Clear();
                        
                        AddStudent();
                        break;
                    case "2":
                        Console.Clear();
                        userMenu.DisplayStudents();
                        RemoveStudent();
                        break;
                    case "3":
                        Console.Clear();
                        
                        AddEmployee();
                        break;
                    case "4":
                        Console.Clear();
                        
                        userMenu.DisplayStaff();
                        break;
                        return;
                    case "5":
                        Console.Clear();
                        SchoolLogin login = new SchoolLogin();
                        login.Login();
                        return;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Avslutar programmet...");
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }

            }

        }

        public void AddEmployee()
        {
            using (var context = new SchoolDbContext())
            {
                Console.WriteLine("Vänligen ange namn på anställd.");
                string? employeeName = Console.ReadLine();
                if (string.IsNullOrEmpty(employeeName))
                {
                    Console.WriteLine("Namn kan inte vara tomt.");
                    return;
                }

                Console.WriteLine("Vänligen ange position på anställd.");
                string? position = Console.ReadLine();
                if (string.IsNullOrEmpty(position))
                {
                    Console.WriteLine("Position kan inte vara tomt.");
                    return;
                }
                if (position == "Lärare")
                {
                    Random random = new Random();
                    string[] classes = { "NAKB2", "SASAMB2", "TEKNB1", "ESB1" };
                    string assignedClassName = classes[random.Next(classes.Length)];

                    var assignedClass = context.Classes.FirstOrDefault(c => c.ClassName == assignedClassName);


                    var employee = new Employee
                    {
                        EmployeeName = employeeName,
                        Position = position,
                        Class = assignedClass
                    };

                    context.Employees.Add(employee);
                    context.SaveChanges();
                }

                else
                {
                    var employee = new Employee
                    {
                        EmployeeName = employeeName,
                        Position = position
                    };

                    context.Employees.Add(employee);
                    context.SaveChanges();
                }
            }
        }

        public void RemoveEmployee()
        {
            using (var context = new SchoolDbContext())
            {
                Console.WriteLine("Vilken anställd vill du ta bort? Ange anställdens Anställd ID.");
                int employeeId = int.Parse(Console.ReadLine());
                var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

                if (employee == null)
                {
                    Console.WriteLine("Kunde inte hitta den anställde.");
                    return;
                }
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
        }

        public void RemoveStudent()
        {
            using (var context = new SchoolDbContext())
            {
                Console.WriteLine("Vilken elev vill du ta bort? Ange elevens Elev ID.");
                int studentId = int.Parse(Console.ReadLine());
                var student = context.Students.FirstOrDefault(s => s.StudentId == studentId);

                

                if (student == null)
                {
                    Console.WriteLine("Kunde inte hitta eleven.");
                    return;
                }

                context.Students.Remove(student);
                context.SaveChanges();
            }
        }

        public void AddStudent()
        {
            using (var context = new SchoolDbContext())
            {
                Console.WriteLine("Vänligen ange förnamn på eleven.");
                string? firstName = Console.ReadLine();
                if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Förnamn kan inte vara tomt.");
                    return;
                }

                Console.WriteLine("Vänligen ange efternamn på eleven.");
                string? lastName = Console.ReadLine();
                if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Efternamn kan inte vara tomt.");
                    return;
                }

                Random random = new Random();
                string[] classes = { "NAKB2", "SASAMB2", "TEKNB1", "ESB1" };


                string assignedClassName = classes[random.Next(classes.Length)];

                var assignedClass = context.Classes.FirstOrDefault(c => c.ClassName == assignedClassName);
                if (assignedClass == null)
                {
                    Console.WriteLine("Kunde inte hitta klassen.");
                    return;
                }

                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Class = assignedClass,
                    
                };

                context.Students.Add(student);
                context.SaveChanges();
                Console.WriteLine("Eleven var tillagd!");
            }
        }
        
        //public void AddCourse()
        //{
        //    using (var context = new SchoolDbContext())
        //    {
        //        Console.WriteLine("Skriv namn på kursen du vill lägga till.");
        //        string courseName = Console.ReadLine();
        //        if (string.IsNullOrEmpty(courseName))
        //        {
        //            Console.WriteLine("Kursnamn kan inte vara tomt.");
        //            return;
        //        }
        //        else
        //        {
        //            var course = new Course
        //            {
        //                CourseName = courseName
        //            };
        //            context.Courses.Add(course);
        //            context.SaveChanges();

        //            Console.WriteLine("Kursen var tillagd!");

        //        }
        //    }
            
    }
           

}






