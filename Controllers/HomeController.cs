using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZergMoney.Helpers;
using ZergMoney.Models;

namespace ZergMoney.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateJoinHousehold(Guid? code)
        {
            //If the current user accessing tis page already has a HouseholdId, send them to their dashboard
            if (User.Identity.IsInHousehold())
            {
                return RedirectToAction("Index", "Home");
            }
            
            HouseholdViewModel vm = new HouseholdViewModel();
            
            //Determine whether the user has been sent an invite and set property values 
            if (code != null)
            {
                string msg = "";
                if (ValidInvite(code, ref msg))
                {
                    Invite result = db.Invites.FirstOrDefault(i => i.HHToken == code);
                    
                    vm.IsJoinHouse = true;
                    vm.HHId = result.HouseholdId;
                    vm.HHName = result.Household.Name;
                    
                    //Set USED flag to true for this invite
                    
                    result.HasBeenUsed = true;
                    
                    ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
                    user.InviteEmail = result.Email;
                    db.SaveChanges();
                }
                else
                {
                    return RedirectToAction("InviteError", new { errMsg = msg });
                }
            }
            return View(vm);
        }
        
        private bool ValidInvite(Guid? code, ref string message)
        {
            
            if ((DateTime.Now - db.Invites.FirstOrDefault(i => i.HHToken == code).InviteDate).TotalDays < 6)
            {
                bool result = db.Invites.FirstOrDefault(i => i.HHToken == code).HasBeenUsed;
                if (result)
                {
                    message = "invalid";
                }
                else
                {
                    message = "valid";
                }
                
                return !result;
            }
            else
            {
                message = "expired";
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}