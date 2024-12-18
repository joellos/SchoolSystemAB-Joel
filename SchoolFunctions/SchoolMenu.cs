using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSystemAB.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;

namespace SchoolSystemAB.SchoolFunctions
{
    public class SchoolMenu
    {
        public void DisplayMenu()
        {
            Console.WriteLine("Välkommen till SchoolAB! Vänligen välj en av de följande alternativen för att navigera eleverna och personal på skolan.");
            while (true)
            {
                
                Console.WriteLine("1. Visa alla elever");
                Console.WriteLine("2. Visa alla betyg");
                Console.WriteLine("3. Visa alla lärare");
                Console.WriteLine("4. Visa alla anställda");
                Console.WriteLine("5. Visa alla kurser");
                Console.WriteLine("6. Visa alla snitt på betyg");
                Console.WriteLine("7. Logga ut ur programmet");
                Console.WriteLine("8. Avsluta programmet");

                switch (Console.ReadLine())
                {
                    case "1":
                        
                        Console.Clear();
                        Console.WriteLine("Här är alla elever:");
                        DisplayStudents();
                        
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("Här är alla satta betyg med dess elever:");
                        DisplayGrades();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Här är alla anställda lärare:");
                        DisplayTeachers();
                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("Här är alla anställda icke lärare:");
                        DisplayStaff();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Här är alla kurser:");
                        DisplayCourses();
                        break;
                    case "6":
                        Console.Clear();
                        Console.WriteLine("Här är alla snittbetyg:");
                        CalculateGrades();
                        break;
                    case "7":
                        Console.Clear();
                        SchoolLogin login = new SchoolLogin();
                        login.Login();
                        return;
                    case "8":
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
        public void DisplayStudents()
        {
            using (var context = new SchoolDbContext())
            {
                var students = from student in context.Students
                               join classEntity in context.Classes on student.ClassId equals classEntity.ClassId
                               select new
                               {
                                   student.FirstName,
                                   student.LastName,
                                   ClassName = classEntity.ClassName,
                                   student.StudentId
                               };

                foreach (var student in students)
                {
                    Console.WriteLine($"{student.FirstName} {student.LastName} {student.ClassName} {student.StudentId}");
                }
            }
        }


        public void DisplayGrades()
        {
            using (var context = new SchoolDbContext())
            {
                var grades = from grade in context.Grades
                             join student in context.Students on grade.StudentId equals student.StudentId
                             join course in context.Courses on grade.CourseId equals course.CourseId
                             select new
                             {
                                 StudentName = student.FirstName + " " + student.LastName,
                                 course.CourseName,
                                 grade.Grade1
                             };

                foreach (var item in grades)
                {
                    Console.WriteLine($"Student: {item.StudentName}, Kurs: {item.CourseName}, Betyg: {item.Grade1}");
                }
            }
        }

        public void DisplayTeachers()
        {
            using (var context = new SchoolDbContext())
            {
                var teachers = context.Employees.Where(e => e.Position == "Lärare");
                foreach (var teacher in teachers)
                {
                    Console.WriteLine(teacher.EmployeeName);
                }
            }
        }

        public void DisplayStaff()
        {
            using (var context = new SchoolDbContext())
            {
                var staff = context.Employees.Where(e => e.Position != "Lärare");
                foreach (var employee in staff)
                {
                    Console.WriteLine(employee.EmployeeName);
                }
            }
        }

        public void DisplayCourses()
        {
            using (var context = new SchoolDbContext())
            {
                var courses = context.Courses.ToList();
                foreach (var course in courses)
                {
                    Console.WriteLine(course.CourseName);
                }
            }
        }


        public void CalculateGrades()
        {
            var gradePoints = new Dictionary<string, double>
            {
                { "A", 4.0 },
                { "A-", 3.7 },
                { "B", 3.3 },
                { "B-", 3.0 },
                { "C", 2.7 },
                { "C-", 2.3 },
                { "D", 2.0 },
                { "D-", 1.7 },
                { "E", 1.0 },
                { "F", 0.0 }
            };

            using (var context = new SchoolDbContext())
            {
                var courseGrades = context.Courses
                    .Include(c => c.Grades)
                    .AsEnumerable() // Switch to client-side evaluation
                    .Select(course => new
                    {
                        CourseName = course.CourseName,
                        AverageGrade = course.Grades
                            .Where(g => gradePoints.ContainsKey(g.Grade1))
                            .Select(g => gradePoints[g.Grade1])
                            .DefaultIfEmpty(0) // Provide a default value
                            .Average(),
                        LowestGrade = course.Grades
                            .OrderBy(g => gradePoints[g.Grade1])
                            .Select(g => g.Grade1)
                            .FirstOrDefault(),
                        HighestGrade = course.Grades
                            .OrderByDescending(g => gradePoints[g.Grade1])
                            .Select(g => g.Grade1)
                            .FirstOrDefault()
                    })
                    .ToList();


                foreach (var course in courseGrades)
                {
                    Console.WriteLine($"Kurs: {course.CourseName}");
                    Console.WriteLine($"Medelbetyg: {course.AverageGrade:F2}");
                    Console.WriteLine($"Lägsta betyg: {course.LowestGrade}");
                    Console.WriteLine($"Högsta betyg: {course.HighestGrade}");
                    Console.WriteLine();
                }
            }

        }

        public void GradesLastMonth()
        {
            using (var context = new SchoolDbContext())
            {
              
                    var lastMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
                    var lastMonthYear = DateTime.Now.Month == 1 ? DateTime.Now.Year - 1 : DateTime.Now.Year;

                    var grades = from grade in context.Grades
                                 join student in context.Students on grade.StudentId equals student.StudentId
                                 join course in context.Courses on grade.CourseId equals course.CourseId
                                 where grade.GradeDate.Month == lastMonth && grade.GradeDate.Year == lastMonthYear
                                 select new
                                 {
                                     StudentName = student.FirstName + " " + student.LastName,
                                     course.CourseName,
                                     grade.Grade1
                                 };

                    foreach (var item in grades)
                    {
                        Console.WriteLine($"Student: {item.StudentName}, Kurs: {item.CourseName}, Betyg: {item.Grade1}");
                    }
            }
            
        }


    }
}
