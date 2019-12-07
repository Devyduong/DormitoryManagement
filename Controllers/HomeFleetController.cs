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
    public class HomeFleetController : Controller
    {
        private WebContext db = new WebContext();
        // GET: HomeFleet
        public ActionResult Index()
        {
            var lstHomeFleet = db.HomeFleets.Select(d => d).ToList();
            ViewBag.lstHomeFleet = lstHomeFleet;
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> addHomeFleet(AddHomeFleetViewModel model)
        {
            try
            {
                HomeFleet hf = new HomeFleet();
                hf.HFName = model.Name;
                hf.NumberOfRoom = 0;
                hf.PricePerRoom = model.Price;
                hf.Status = 1;

                db.HomeFleets.Add(hf);
                await db.SaveChangesAsync();

                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> getHFById(int Id)
        {
            try
            {
                HomeFleet hf = db.HomeFleets.Where(d => d.HFID == Id).FirstOrDefault();
                if (hf != null)
                    return Json(hf, JsonRequestBehavior.AllowGet);
                else
                    return Json("NotExist", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<JsonResult> deleteRecord(int Id)
        {
            try
            {
                HomeFleet hf = db.HomeFleets.Where(d => d.HFID == Id).FirstOrDefault();
                if(hf == null)
                    return Json("NotExist", JsonRequestBehavior.AllowGet);
                else
                {
                    List<Room> roomsBelongToHF = db.Rooms.Where(d => d.Homefleet == hf.HFID).ToList();
                    if(roomsBelongToHF.Count > 0)
                    {
                        foreach(Room room in roomsBelongToHF)
                        {
                            db.Rooms.Remove(room);
                            await db.SaveChangesAsync();
                        }
                    }
                    db.HomeFleets.Remove(hf);
                    await db.SaveChangesAsync();
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> modifyRecord(ModifyHomeFleetViewModel model)
        {
            try
            {
                HomeFleet hf = db.HomeFleets.Where(d => d.HFID == model.Id).FirstOrDefault();
                if (hf == null)
                    return Json("NotExist", JsonRequestBehavior.AllowGet);
                else
                {
                    hf.HFName = model.Name;
                    hf.PricePerRoom = model.Price;

                    db.Entry(hf).State = System.Data.Entity.EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
} 