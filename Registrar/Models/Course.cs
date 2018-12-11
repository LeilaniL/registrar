using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;

namespace Registrar.Models
{
    public class Course
    {
        private string _name;
        private string _course_number;

        public Course(string name, string courseNumber)
        {
            _name = name;
            _course_number = courseNumber;
        }

        public string GetName()
        {
            return _name;
        }
        public string GetNumber()
        {
            return _course_number;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO courses (name, course_number) VALUES (@name, @course_number);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            MySqlParameter course_number = new MySqlParameter();
            course_number.ParameterName = "@course_number";
            course_number.Value = this._course_number;
            cmd.Parameters.Add(course_number);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Course> GetAllCourses()
        {
            List<Course> allCourses = new List<Course> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                string CourseName = rdr.GetString(0);
                string CourseNumber = rdr.GetString(1);
                Course newCourse = new Course(CourseName, CourseNumber);
                allCourses.Add(newCourse);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCourses;
        }
        public static Course Find(string courseNumber)
        {
            Console.WriteLine("Course Find was called");
            Console.WriteLine(courseNumber);


            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM courses WHERE course_number = (@courseNumber);";
            MySqlParameter course_number = new MySqlParameter();
            course_number.ParameterName = "@courseNumber";
            course_number.Value = courseNumber;
            cmd.Parameters.Add(course_number);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            string CourseName = "";
            string CourseNumber = "";
            while (rdr.Read())
            {
                CourseName = rdr.GetString(0);
                CourseNumber = rdr.GetString(1);

            }
            Course foundCourse = new Course(CourseName, CourseNumber);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Console.WriteLine(foundCourse.GetName());
            return foundCourse;
        }

        public void AddStudent(Student newStudent)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO registrations (student_id, course_number) VALUES (@StudentId, @CourseNumber);";
            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = newStudent.GetId();
            cmd.Parameters.Add(student_id);
            MySqlParameter course_number = new MySqlParameter();
            course_number.ParameterName = "@CourseNumber";
            course_number.Value = _course_number;
            cmd.Parameters.Add(course_number);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Student> GetStudents()
        {
            Console.WriteLine("I tried to get items");
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT student_id FROM registrations WHERE course_number = (@CourseNumber);";
            MySqlParameter courseNumberParameter = new MySqlParameter();
            courseNumberParameter.ParameterName = "@CourseNumber";
            courseNumberParameter.Value = _course_number;
            cmd.Parameters.Add(courseNumberParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<int> studentIds = new List<int> { };
            while (rdr.Read())
            {
                int studentId = rdr.GetInt32(0);
                studentIds.Add(studentId);
            }
            rdr.Dispose();
            List<Student> students = new List<Student> { };
            foreach (int studentId in studentIds)
            {
                Console.WriteLine("GetItems was called and empty Student list started");

                var itemQuery = conn.CreateCommand() as MySqlCommand;
                itemQuery.CommandText = @"SELECT * FROM students WHERE id = @StudentId;";
                MySqlParameter studentIdParameter = new MySqlParameter();
                studentIdParameter.ParameterName = "@StudentId";
                studentIdParameter.Value = studentId;
                itemQuery.Parameters.Add(studentIdParameter);
                var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
                while (itemQueryRdr.Read())
                {
                    int thisstudentId = itemQueryRdr.GetInt32(1);
                    string thisstudentName = itemQueryRdr.GetString(0);
                    // DateTime itemDueDate = rdr.GetDateTime(2);
                    Student foundStudent = new Student(thisstudentName, thisstudentId);
                    students.Add(foundStudent);
                }
                itemQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Console.WriteLine(students.Count);
            return students;
        }
    }
}
