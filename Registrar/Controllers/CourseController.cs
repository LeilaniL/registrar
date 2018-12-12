using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System;

namespace Registrar.Controllers
{
    public class CourseController : Controller
    {

        [HttpGet("/courses")]
        public ActionResult Index()
        {
            List<Course> allCourses = Course.GetAllCourses();
            return View(allCourses);
        }
        [HttpGet("/courses/new")]
        public ActionResult New()
        {
            return View();
        }
        [HttpPost("/courses")]
        public ActionResult Create(string courseName, string courseNumber)
        {
            Course newCourse = new Course(courseName, courseNumber);
            newCourse.Save();
            List<Course> allCourses = Course.GetAllCourses();
            return View("Index", allCourses);
        }
        [HttpGet("/courses/{courseNumber}")]
        public ActionResult Show(string courseNumber)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Course selectedCourse = Course.Find(courseNumber);
            List<Student> courseStudents = selectedCourse.GetStudents();
            List<Student> allStudents = Student.GetAll();
            model.Add("courseStudents", courseStudents);
            model.Add("allStudents", allStudents);
            model.Add("course", selectedCourse);

            return View("Show", model);
        }

        [HttpPost("/enrollStudent")]
        public ActionResult AddStudent(string courseNumber, int studentRegistration)
        {
            Dictionary<string, object> model = new Dictionary<string, object>();
            Console.WriteLine("Update was called");
            Course selectedCourse = Course.Find(courseNumber);
            Student student = Student.Find(studentRegistration);
            Console.WriteLine(student.GetName());
            selectedCourse.AddStudent(student);
            List<Student> courseStudents = selectedCourse.GetStudents();
            List<Student> allStudents = Student.GetAll();
            model.Add("courseStudents", courseStudents);
            model.Add("allStudents", allStudents);
            model.Add("course", selectedCourse);
            return View("Show", model);
        }

    }
}
