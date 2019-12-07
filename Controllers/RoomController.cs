using DormitoryManagement.Enums;
using DormitoryManagement.Models;
using DormitoryManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DormitoryManagement.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private WebContext db = new WebContext();
        // GET: Room
        public ActionResult RoomManage()
        {
            var allRoom = db.Rooms.Select(d => d).ToList();
            ViewBag.lstRooms = allRoom; 
            return View();
        }
        [HttpGet]
        public ActionResult AddRoom()
        {
            List<HomeFleet> list = db.HomeFleets.Select(d => d).ToList();
            List<SelectListItem> homefleets = new List<SelectListItem>();
            foreach(HomeFleet it in list)
            {
                SelectListItem select = new SelectListItem();
                select.Value = it.HFID.ToString();
                select.Text = it.HFName;
                homefleets.Add(select);
            }
            ViewBag.homefleets = homefleets;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRoom(AddRoomViewModel model)
        {
            if(ModelState.IsValid)
            {
                Room room = new Room
                {
                    RoomName = model.RoomName,
                    Homefleet = model.Homefleet,
                    BedNumber = model.BedNumber,
                    BedEmpty = model.BedNumber,
                    Included = model.Included,
                    Status = 1
                };
                if (model.ForGender == "Nữ")
                    room.ForGender = (int)EnumGender.Nữ;
                else
                    room.ForGender = (int)EnumGender.Nam;

                db.Rooms.Add(room);

                HomeFleet homeFleet = db.HomeFleets.Find(model.Homefleet);
                homeFleet.NumberOfRoom = homeFleet.NumberOfRoom + 1;
                db.Entry(homeFleet).State = System.Data.Entity.EntityState.Modified;

                await db.SaveChangesAsync();

                return RedirectToAction("RoomManage", "Room");
            }
            return View(model);
        }

        public ActionResult EditRoom(int? roomId)
        {
            if(roomId == null)
            {
                return RedirectToAction("RoomManage", "Room");
            }
            List<HomeFleet> list = db.HomeFleets.Select(d => d).ToList();
            List<SelectListItem> homefleets = new List<SelectListItem>();
            foreach (HomeFleet it in list)
            {
                SelectListItem select = new SelectListItem();
                select.Value = it.HFID.ToString();
                select.Text = it.HFName;
                homefleets.Add(select);
            }
            ViewBag.homefleets = homefleets;

            Room room = db.Rooms.Find(roomId);
            if(room == null)
            {
                return RedirectToAction("RoomManage", "Room");
            }
            EditRoomViewModel model = new EditRoomViewModel
            {
                RoomId = room.RoomId,
                RoomName = room.RoomName,
                ForGender = room.ForGender.ToString(),
                Homefleet = room.Homefleet,
                BedNumber = room.BedNumber,
                Included = room.Included
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> EditRoom(EditRoomViewModel model)
        {
            if(ModelState.IsValid)
            {
                Room room = db.Rooms.Find(model.RoomId);

                room.RoomName = model.RoomName;
                room.Homefleet = model.Homefleet;
                room.Included = room.Included;

                if (model.ForGender == "Nữ")
                    room.ForGender = (int)EnumGender.Nữ;
                else
                    room.ForGender = (int)EnumGender.Nam;

                if (room.BedNumber <= model.BedNumber)
                {
                    room.BedEmpty = room.BedEmpty + (model.BedNumber - room.BedNumber);
                    room.BedNumber = model.BedNumber;
                }
                else
                {
                    if(room.BedEmpty < (room.BedNumber - model.BedNumber))
                    {
                        return View(model);
                    }
                    else
                    {
                        room.BedEmpty = room.BedEmpty - (room.BedNumber - model.BedNumber);
                        room.BedNumber = model.BedNumber;
                    }
                }

                db.Entry(room).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("RoomManage", "Room");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if(room == null)
            {
                return RedirectToAction("RoomManage", "Room");
            }
            db.Rooms.Remove(room);
            await db.SaveChangesAsync();
            return RedirectToAction("RoomManage", "Room");
        }
    }
}