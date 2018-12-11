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
    }
}
