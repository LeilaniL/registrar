using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Registrar;

namespace Registrar.Models
{
    public class Student
    {
        private string _name;
        private int _id;

        public Student(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public string GetName()
        {
            return _name;
        }
        public int GetId()
        {
            return _id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO students (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public static List<Student> GetAll()
        {
            List<Student> allStudents = new List<Student> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int StudentId = rdr.GetInt32(1);
                string StudentName = rdr.GetString(0);
                Student newStudent = new Student(StudentName, StudentId);
                allStudents.Add(newStudent);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allStudents;
        }

        public static Student Find(int studentId)
        {
            Console.WriteLine("Course Find was called");
            Console.WriteLine(studentId);


            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM students WHERE id = (@studentId);";
            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@studentId";
            student_id.Value = studentId;
            cmd.Parameters.Add(student_id);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            string StudentName = "";
            int StudentId = 0;
            while (rdr.Read())
            {
                StudentName = rdr.GetString(0);
                StudentId = rdr.GetInt32(1);

            }
            Student foundStudent = new Student(StudentName, StudentId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Console.WriteLine(foundStudent.GetName());
            return foundStudent;
        }

        public void AddCourse(Course newCourse)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO registrations (student_id, course_number) VALUES (@StudentId, @CourseNumber);";
            MySqlParameter student_id = new MySqlParameter();
            student_id.ParameterName = "@StudentId";
            student_id.Value = _id;
            cmd.Parameters.Add(student_id);
            MySqlParameter course_number = new MySqlParameter();
            course_number.ParameterName = "@CourseNumber";
            course_number.Value = newCourse.GetNumber();
            cmd.Parameters.Add(course_number);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List<Course> GetCourses()
        {
            Console.WriteLine("I tried to get items");
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT course_number FROM registrations WHERE student_id = (@StudentId);";
            MySqlParameter studentIdParameter = new MySqlParameter();
            studentIdParameter.ParameterName = "@StudentId";
            studentIdParameter.Value = _id;
            cmd.Parameters.Add(studentIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<string> courseNumbers = new List<string> { };
            while (rdr.Read())
            {
                string courseNumber = rdr.GetString(0);
                courseNumbers.Add(courseNumber);
            }
            rdr.Dispose();
            List<Course> courses = new List<Course> { };
            foreach (string courseNumber in courseNumbers)
            {
                Console.WriteLine("GetItems was called and empty Student list started");

                var itemQuery = conn.CreateCommand() as MySqlCommand;
                itemQuery.CommandText = @"SELECT * FROM courses WHERE course_number = @CourseNumber;";
                MySqlParameter CourseNumberParameter = new MySqlParameter();
                CourseNumberParameter.ParameterName = "@CourseNumber";
                CourseNumberParameter.Value = courseNumber;
                itemQuery.Parameters.Add(CourseNumberParameter);
                var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
                while (itemQueryRdr.Read())
                {
                    string thiscourseName = itemQueryRdr.GetString(0);
                    string thiscourseNumber = itemQueryRdr.GetString(1);
                    // DateTime itemDueDate = rdr.GetDateTime(2);
                    Course foundCourse = new Course(thiscourseName, thiscourseNumber);
                    courses.Add(foundCourse);
                }
                itemQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Console.WriteLine(courses.Count);
            return courses;
        }
    }
}
