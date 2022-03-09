using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ExerciseFour
{
    class Program
    {
        static void Main(string[] args) {
            Application();
        }

        static void Application() {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            using (var db = new CourseAssignment()) {
                Console.WriteLine($"Database Path: {db.DbPath}");

                Console.WriteLine("\nSelect your choice from the list:");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. Add Student");
                Console.WriteLine("3. Assign Course to student");
                Console.WriteLine("4. View all students");
                Console.WriteLine("5. View all courses");

                Console.Write("\nSelection: ");
                int selection = int.Parse(Console.ReadLine());

                if (selection == 1) {
                    Console.Clear();
                    Console.WriteLine("Add a new course");

                    Console.Write("\nCourse Name: ");
                    string courseName = Console.ReadLine();

                    db.Add(new Course { CourseName = courseName });
                    db.SaveChanges();

                    Console.WriteLine($"Successfully added {courseName} as a course.");

                    Console.WriteLine("\nPlease wait..");
                    Thread.Sleep(2000);
                    Program.Main(new string[] { });
                } else if (selection == 2) {
                    Console.Clear();
                    Console.WriteLine("Add a new student");

                    Console.Write("\nFirst Name: ");
                    string firstName = Console.ReadLine();
                    db.Add(new Student { FirstName = firstName });
                    db.SaveChanges();

                    Console.WriteLine($"Successfully added {firstName} as a student.");

                    Console.WriteLine("\nPlease wait..");
                    Thread.Sleep(2000);
                    Program.Main(new string[] { });
                } else if (selection == 3) {
                    Console.Clear();
                    Console.WriteLine("Assign Course to student");

                    Console.WriteLine("\nFirst name: ");
                    string firstName = Console.ReadLine();

                    var students = db.Students
                        .ToArray();

                    bool studentFound = false;
                    bool courseFound = false;

                    foreach (Student student in students) {
                        if (firstName == student.FirstName) {
                            studentFound = true;
                            Console.WriteLine("Course name: ");
                            string courseName = Console.ReadLine();

                            var courses = db.Courses
                                .ToArray();

                            foreach (Course course in courses) {
                                if (courseName == course.CourseName) {
                                    courseFound = true;
                                    student.Courses = new List<Course>();
                                    student.Courses.Add(course);
                                    db.SaveChanges();
                                    Console.WriteLine($"Added {courseName} to {firstName}");
                                    break;
                                }
                            }
                            if (courseFound == false) {
                                Console.WriteLine("The course you're looking for does not exist.");
                            }
                        }
                    }
                    if (studentFound == false) {
                        Console.WriteLine("The student you're looking for does not exist.");
                    }

                    Console.WriteLine("\nPlease wait..");
                    Thread.Sleep(2000);
                    Program.Main(new string[] { });

                } else if (selection == 4) {
                    Console.Clear();
                    Console.WriteLine("Viewing all students");
                    using var context = new CourseAssignment();
                    int counter = 1;
                    var students = db.Students
                        .Include(c => c.Courses)
                        .OrderBy(b => b.StudentId);


                    foreach (Student student in students) {
                        if (student.Courses == null) {
                            Console.WriteLine($"{counter}. Name: {student.FirstName}, Courses: Cannot find any course.");
                        } else {
                            Array courseList = student.Courses.ToArray();
                            Console.WriteLine($"{counter}. Name: {student.FirstName}, \nCourses: ");
                            if (courseList.Length > 0) {
                                foreach (Course course in courseList) {
                                    Console.WriteLine($"\t- {course.CourseName}");
                                }
                            } else {
                                Console.WriteLine($"\t- No courses assigned to this student.");
                            }
                            Console.WriteLine("\n");
                        }
                        counter++;
                    }

                    Console.WriteLine("\nWrite \"e\" to go back.");
                    Console.Write("Input: ");
                    string exit = Console.ReadLine().ToLower();
                    if (exit == "e") {
                        Program.Main(new string[] { });
                    }
                } else if (selection == 5) {
                    Console.Clear();
                    Console.WriteLine("Viewing all courses");
                    int counter = 1;
                    var courses = db.Courses
                        .OrderBy(c => c.CourseId)
                        .ToArray();
                    foreach (Course course in courses) {
                        Console.WriteLine($"{counter}. {course.CourseName}");
                        counter++;
                    }

                    Console.WriteLine("\nWrite \"e\" to go back.");
                    Console.Write("Input: ");
                    string exit = Console.ReadLine().ToLower();
                    if (exit == "e") {
                        Program.Main(new string[] { });
                    }
                }
            }
        }
    }
}
