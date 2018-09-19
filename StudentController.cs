using AutoMapper;
using LYC.StdMgt.BusinessService.Contracts;
using LYC.StdMgt.Domain;
using LYC.StdMgt.Utils.Log.Contracts;
using LYC.StdMgt.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LYC.StdMgt.Web.Controllers
{
    public class StudentController : Controller
    {
        private IStudentManager _studentManager;
        private ILogger _logger;
        public StudentController(IStudentManager studentManager, ILogger logger)
        {
            _studentManager = studentManager;
            _logger = logger;
        }


        // GET: Student
        public ActionResult Index()
        {
            List<Student> student = new List<Student>();

            student = _studentManager.GetStudentList();

            List<StudentViewModel> studentViewModel = Mapper.Map<List<Student>, List<StudentViewModel>>(student);


            return View(studentViewModel);
        }

        public ActionResult Create(StudentViewModel studentViewModel)
        {
            if (studentViewModel.FirstName != null)
            {
                Student student = Mapper.Map<StudentViewModel, Student>(studentViewModel);
                _studentManager.AddStudent(student);
                return RedirectToAction("Index", "Student");
            }
          
            return View();

        }
       
    }
}