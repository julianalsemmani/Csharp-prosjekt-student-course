using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExerciseFour
{
    class CourseAssignment : DbContext {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        public string DbPath { get; }

        public CourseAssignment() {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "studentcourse.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Course {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public ICollection<Student> Students { get; set; }
    }

    public class Student {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
