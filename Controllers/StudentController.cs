using DormitoryManagement.Models;
using DormitoryManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers
{
    public class StudentController : Controller
    {
        private WebContext db = new WebContext();
        // GET: Student
        public ActionResult Index()
        {
            List<ListStudentViewModel> list = new List<ListStudentViewModel>();
            var lstStudents = db.Students.Select(s => s).ToList();
            foreach(var it in lstStudents)
            {
                AppUser user = db.Users.Where(d => d.Owner.Equals(it.StudentId)).FirstOrDefault();

                ListStudentViewModel lsv = new ListStudentViewModel
                {
                    StudentId = it.StudentId,
                    FullName = it.FullName,
                    DOB = ((DateTime)it.DOB).ToString("dd/MM/yyyy"),
                    CMND = it.CMND,
                    StudentYear = it.StudentYear,
                    StudyAt = it.StudyAt,
                    Department = it.Department,
                    Email = user.Email,
                    Gender = it.Gender
                };
                list.Add(lsv);
            }
            ViewBag.lstStudents = list;
            return View();
        }
        [HttpGet]
        public ActionResult AddStudent()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditStudent(string StudentId)
        {
            if(StudentId == null)
            {
                return View();
            }
            Student student = db.Students.Find(StudentId);
            AppUser appUser = db.Users.Where(d => d.Owner.Equals(student.StudentId)).FirstOrDefault();

            AddStudentViewModel model = new AddStudentViewModel
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Gender = student.Gender,
                CMND = student.CMND,
                DATECMND = student.DATECMND,
                IssuedBy = student.IssuedBy,
                DOB = student.DOB,
                Folk = student.Folk,
                Religion = student.Religion,
                Nation = student.Nation,
                StudentYear = student.StudentYear,
                StudyAt = student.StudyAt,
                Department = student.Department,
                Address = student.Address,
                Photo = student.Photo,
                PhoneNumber = appUser.PhoneNumber,
                Email = appUser.Email
            };
            return View(model);
        }
    }
}