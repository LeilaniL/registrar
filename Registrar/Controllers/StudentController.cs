using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;

namespace Registrar.Controllers
{
    public class StudentController : Controller
    {

        [HttpGet("/students")]
        public ActionResult Index()
        {
            List<Student> allStudents = Student.GetAll();
            return View(allStudents);
        }
        [HttpGet("/students/new")]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost("/students")]
        public ActionResult Create(string name)
        {
            Student newStudent = new Student(name);
            newStudent.Save();
            List<Student> allStudents = Student.GetAll();
            return View("Index", allStudents);
        }

        [HttpGet("/students/{studentId}")]
        public ActionResult Show(int studentId)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Student selectedStudent = Student.Find(studentId);
            List<Course> studentCourses = selectedStudent.GetCourses();
            List<Course> allCourses = Course.GetAllCourses();
            model.Add("studentCourses", studentCourses);
            model.Add("allCourses", allCourses);
            model.Add("student", selectedStudent);

            return View("Show", model);
        }
        [HttpPost("/enrollCourse")]
        public ActionResult AddCourse(int studentId, string courseNumber)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();

            Student selectedStudent = Student.Find(studentId);
            Course course = Course.Find(courseNumber);

            selectedStudent.AddCourse(course);
            List<Course> studentCourses = selectedStudent.GetCourses();
            List<Course> allCourses = Course.GetAllCourses();
            model.Add("studentCourses", studentCourses);
            model.Add("allCourses", allCourses);
            model.Add("student", selectedStudent);
            return View("Show", model);
        }


    }
}
