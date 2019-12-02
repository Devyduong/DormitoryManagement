using DormitoryManagement.Models;
using DormitoryManagement.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DormitoryManagement.App_Start;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace DormitoryManagement.Controllers
{
    public class AccountController : Controller
    {
        private AppSignInManager _signInManager;
        private AppUserManager _userManager;

        private WebContext db = new WebContext();

        public AccountController()
        {
        }

        public AccountController(AppUserManager userManager, AppSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public AppSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<AppSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public AppUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new WebContext()));
                        AppUser user = db.Users.Where(u => u.UserName.Equals(model.Email)).FirstOrDefault();
                        string role = user.Roles.ToString();
                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateStudent(AddStudentViewModel model)
        {
            try
            {
                Student student = new Student
                {
                    StudentId = model.StudentId,
                    FullName = model.FullName,
                    Gender = model.Gender,
                    DOB = model.DOB,
                    CMND = model.CMND,
                    DATECMND = model.DATECMND,
                    IssuedBy = model.IssuedBy,
                    Address = model.Address,
                    Nation = model.Nation,
                    Folk = model.Folk,
                    Religion = model.Religion,
                    Department = model.Department,
                    StudentYear = model.StudentYear,
                    StudyAt = model.StudyAt,
                    Photo = model.Photo,
                    Status = 1
                };

                AppUser cridential = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Owner = model.StudentId
                };

                db.Students.Add(student);

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new WebContext()));
                if (!roleManager.RoleExists("Student"))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole("Student"));
                    if(!roleResult.Succeeded)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                    roleManager.Dispose();
                }

                var result = await UserManager.CreateAsync(cridential, model.Password);

                if (result.Succeeded)
                {
                    
                    string userId = db.Users.Where(u => u.UserName.Equals(model.Email)).Select(u => u.Id).FirstOrDefault().ToString();
                    await UserManager.AddToRoleAsync(userId, "Student");
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Student");

            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStudent(AddStudentViewModel model)
        {
            try
            {
                Student student = db.Students.Find(model.StudentId);

                student.FullName = model.FullName;
                student.Gender = model.Gender;
                student.DOB = model.DOB;
                student.CMND = model.CMND;
                student.DATECMND = model.DATECMND;
                student.IssuedBy = model.IssuedBy;
                student.Address = model.Address;
                student.Nation = model.Nation;
                student.Folk = model.Folk;
                student.Religion = model.Religion;
                student.Department = model.Department;
                student.StudentYear = model.StudentYear;
                student.StudyAt = model.StudyAt;
                student.Photo = model.Photo;

                db.Entry(student).State = System.Data.Entity.EntityState.Modified;

                AppUser user = db.Users.AsNoTracking().Where(x => x.Owner.Equals(student.StudentId)).FirstOrDefault();
                user.PhoneNumber = model.PhoneNumber;

                db.Entry(user).State = EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Student");
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RemoveStudent(DeleteStudentViewModel model)
        {
            try
            {
                Student student = db.Students.Find(model.StudentId);
                if (student == null)
                {
                    return Json("NotFound", JsonRequestBehavior.AllowGet);
                }

                string email = model.Email;
                AppUser user = await UserManager.FindByEmailAsync(email);

                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    db.Students.Remove(student);
                }
                await db.SaveChangesAsync();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}