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

    }
}
