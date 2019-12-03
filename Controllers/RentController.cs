using DormitoryManagement.Models;
using DormitoryManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers
{
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
            return View();
        }


        #region common Functions
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
        #endregion
    }
}