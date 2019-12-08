using DormitoryManagement.Models;
using DormitoryManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers
{
    [Authorize]
    public class RentController : Controller
    {
        private WebContext db = new WebContext();
        // GET: Rent
        public ActionResult RentManage()
        {
            List<Rent> rents = db.Rents.Select(d => d).ToList();

            List<ListRentHomeViewModel> lstRent = new List<ListRentHomeViewModel>();
            if (rents.Count > 0)
            {
                // set list to send to View
                foreach (Rent rent in rents)
                {
                    // Get Name by Id
                    string student = GetStudentNameById(rent.Renter);
                    string room = GetRoomNameById((int)rent.Room);
                    int homeFleetOfRoom = GetHomeFleetOfRoomByRoomId((int)rent.Room);
                    string homeFleet = GetHomeFleetNameById(homeFleetOfRoom);
                    string paidStatus = GetStatusPaid((double)rent.TotalFee, (double)rent.Paid);

                    // Create new record
                    ListRentHomeViewModel listMember = new ListRentHomeViewModel
                    {
                        ID = rent.ID,
                        Student = student,
                        Room = room,
                        HomeFleet = homeFleet,
                        StartDate = ((DateTime)rent.StartDate).ToString("dd/MM/yyyy"),
                        EndDate = ((DateTime)rent.EndDate).ToString("dd/MM/yyyy"),
                        Paid = paidStatus,
                        TheLease = "Admin"
                    };
                    // add record to list
                    lstRent.Add(listMember);
                }
            }

            ViewBag.lstRent = lstRent;
            return View();
        }

        public ActionResult AddRent()
        {
            ViewBag.students = db.Students.Where(d => d.Status == 1).ToList();
            List<HomeFleet> homeFleets = db.HomeFleets.Where(d => d.Status == 1).ToList();
            ViewBag.homefleets = homeFleets;
            int hfId = homeFleets.ElementAt(0).HFID;
            List<Room> rooms = db.Rooms.Where(d => d.Homefleet == hfId).ToList();
            ViewBag.rooms = rooms;
            return View();
        }
        [HttpPost]
        public JsonResult AddRentToDatabase(AddRentViewModel model)
        {
            try
            {
                Rent rent = new Rent
                {
                    Renter = model.Student,
                    Room = model.Room,
                    Homefleet = model.Homefleet,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    CreateDate = DateTime.Now,
                    TotalFee = getPricePerMonth(model.Homefleet),
                    Paid = model.Paid,
                    Status = 0
                };

                db.Rents.Add(rent);

                Student student = db.Students.Where(s => s.StudentId.Equals(model.Student)).FirstOrDefault();
                student.Status = 0;
                db.Entry(student).State = System.Data.Entity.EntityState.Modified;

                Room room = db.Rooms.Where(r => r.RoomId == model.Room).FirstOrDefault();
                room.BedEmpty = room.BedEmpty - 1;
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json("Error: "+ ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult EditRent(int rentId)
        {
            if(rentId == null)
            {
                return View("RentManage");
            }
            Rent rent = db.Rents.Where(d => d.ID == rentId).FirstOrDefault();
            List<HomeFleet> homeFleets = db.HomeFleets.Where(d => d.Status == 1).ToList();
            
            List<Room> rooms = db.Rooms.Where(d => d.Homefleet == rent.Homefleet).ToList();
            ViewBag.homefleets = homeFleets;
            ViewBag.rooms = rooms;

            return View(rent);
        }
        [HttpPost]
        public JsonResult UpdateRent(EditRentViewModel model)
        {
            try
            {
                Rent rent = db.Rents.Where(d => d.ID == model.ID).FirstOrDefault();
                rent.StartDate = model.StartDate;
                rent.EndDate = model.EndDate;
                if(rent.Room != model.Room)
                {
                    Room oldRoom = db.Rooms.Where(r => r.RoomId == rent.Room).FirstOrDefault();
                    oldRoom.BedEmpty = oldRoom.BedEmpty + 1;
                    db.Entry(oldRoom).State = System.Data.Entity.EntityState.Modified;
                }
                rent.Homefleet = model.Homefleet;

                rent.Room = model.Room;
                Room newRoom = db.Rooms.Where(r => r.RoomId == rent.Room).FirstOrDefault();
                newRoom.BedEmpty = newRoom.BedEmpty - 1;
                db.Entry(newRoom).State = System.Data.Entity.EntityState.Modified;

                db.Entry(rent).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error " + ex.Message, JsonRequestBehavior.AllowGet);
            }       
        }

        [HttpGet]
        public JsonResult deleteRent(int rentId)
        {
            try
            {
                Rent rent = db.Rents.Where(d => d.ID == rentId).FirstOrDefault();
                db.Rents.Remove(rent);

                Student student = db.Students.Where(s => s.StudentId.Equals(rent.Renter)).FirstOrDefault();
                student.Status = 1;
                db.Entry(student).State = System.Data.Entity.EntityState.Modified;

                Room room = db.Rooms.Where(r => r.RoomId == rent.Room).FirstOrDefault();
                room.BedEmpty = room.BedEmpty + 1;
                db.Entry(room).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error " + ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult PayAll(int id)
        {
             try
            {
                Rent rent = db.Rents.Where(d => d.ID == id).FirstOrDefault();
                rent.TotalFee = 0;
                rent.Paid = 0;

                db.Entry(rent).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Exception " + ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Pay(int? id)
        {
            if (id != null)
            {
                Rent rent = db.Rents.Where(d => d.ID == id).FirstOrDefault();

                PayViewModel model = new PayViewModel();
                model.RentId = rent.ID;
                model.StudentCode = rent.Renter;
                model.StudentName = GetStudentNameById(rent.Renter);
                model.Debt = (double)(rent.TotalFee - rent.Paid);
                model.Status = GetStatusPaid((double)rent.TotalFee, (double)rent.Paid);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult Pay(PayViewModel model)
        {
            if (ModelState.IsValid)
            {
                Rent rent = db.Rents.Where(d => d.ID == model.RentId).FirstOrDefault();

                rent.Paid = rent.Paid + model.PayFee;

                db.Entry(rent).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DebtStatistical", "Rent");
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult DebtStatistical()
        {
            List<Rent> lstRent = new List<Rent>();
            lstRent = db.Rents.Where(d => d.TotalFee > d.Paid).ToList();

            List<DebtStatisticalViewModel> lstDebt = new List<DebtStatisticalViewModel>();
            int no = 0;
            if(lstRent.Count > 0)
            {
                foreach(Rent rent in lstRent)
                {
                    DebtStatisticalViewModel debt = new DebtStatisticalViewModel();
                    debt.RentId = rent.ID;
                    debt.StudentCode = rent.Renter;
                    debt.StudentName = GetStudentNameById(rent.Renter);
                    debt.Status = GetStatusPaid((double)rent.TotalFee, (double)rent.Paid);
                    debt.No = no + 1;

                    lstDebt.Add(debt);
                }
            }
            ViewBag.debts = lstDebt;
            return View();
        }


        #region common Functions
        [HttpGet]
        public PayViewModel getPayByStudentId(string studentId)
        {
            Rent rent = new Rent();
            PayViewModel model = new PayViewModel();

            rent = db.Rents.Where(d => d.Renter.Equals(studentId)).FirstOrDefault();

            model.RentId = rent.ID;
            model.StudentCode = rent.Renter;
            model.StudentName = GetStudentNameById(rent.Renter);
            model.Debt = (double)(rent.TotalFee - rent.Paid);
            model.Status = GetStatusPaid((double)rent.TotalFee, (double)rent.Paid);

            return model;
        }
        private string GetStatusPaid(double total, double paid)
        {
            if(total <= paid)
            {
                return "Đã thanh toán";
            }
            else
            {
                return "Còn thiếu " + (total - paid) + " đồng";
            }
        }
        private string GetStudentNameById(string Id)
        {
            return db.Students.Where(s => s.StudentId.Equals(Id)).Select(s => s.FullName).FirstOrDefault().ToString();
        }

        private string GetRoomNameById(int id)
        {
            return db.Rooms.Where(r => r.RoomId == id).Select(r => r.RoomName).FirstOrDefault().ToString();
        }
        private string GetHomeFleetNameById(int Id)
        {
            return db.HomeFleets.Where(h => h.HFID == Id).Select(h => h.HFName).FirstOrDefault().ToString();
        }
        private int GetHomeFleetOfRoomByRoomId(int id)
        {
            return (int)db.Rooms.Where(r => r.RoomId == id).Select(r => r.Homefleet).FirstOrDefault();
        }
        [HttpGet]
        public JsonResult getRoomsByHomefleetId(int hfId)
        {
            return Json(db.Rooms.Where(d => d.Homefleet == hfId && d.BedEmpty > 0).ToList(), JsonRequestBehavior.AllowGet);
        }

        public double getPricePerMonth(int HFID)
        {
            return (double)db.HomeFleets.Where(d => d.HFID == HFID).Select(d => d.PricePerRoom).FirstOrDefault();
        }

        #endregion
    }
}