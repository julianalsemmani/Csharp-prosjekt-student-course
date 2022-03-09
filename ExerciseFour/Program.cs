using System;
using System.Collections.Generic;
using System.Linq;

namespace ExerciseFour
{
    class Program
    {
        static void Main(string[] args) {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            using (var db = new CourseAssignment()) {
                Console.WriteLine($"Database Path: {db.DbPath}");

                Console.WriteLine("Select your choice from the list:");
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
                } else if (selection == 2) {
                    Console.Clear();
                    Console.WriteLine("Add a new student");

                    Console.Write("\nFirst Name: ");
                    string firstName = Console.ReadLine();
                    db.Add(new Student { FirstName = firstName });
                    db.SaveChanges();
                } else if (selection == 3) {
                    Console.Clear();
                    Console.WriteLine("Assign Course to student");
                    
                    Console.WriteLine("\nFirst name: ");
                    string firstName = Console.ReadLine();

                    var students = db.Students
                        .ToArray();

                    bool studentFound = false;
                    bool courseFound = false;

                    foreach(Student student in students) {
                        if (firstName == student.FirstName) {
                            studentFound = true;
                            Console.WriteLine("Course name: ");
                            string courseName = Console.ReadLine();

                            var courses = db.Courses
                                .ToArray();

                            foreach(Course course in courses) {
                                if (courseName == course.CourseName) {
                                    courseFound = true;
                                    student.Courses = new List<Course>();
                                    student.Courses.Add(new Course { CourseName = course.CourseName });
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
                    
                } else if (selection == 4) {
                    Console.Clear();
                    Console.WriteLine("Viewing all students");
                    int counter = 1;
                    var students = db.Students
                        .OrderBy(b => b.StudentId)
                        .ToArray();

                    foreach (Student student in students) {
                        if (student.Courses.ToList() == null) {
                            Console.WriteLine($"{counter}. Name: {student.FirstName}, Courses: Not assigned to any course");
                        } else {
                            Console.WriteLine($"{counter}. Name: {student.FirstName}, Courses: {string.Join(",", student.Courses.ToList())}");
                        }
                        counter++;
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
                }

                Console.Read();
            }
        }
    }
}
