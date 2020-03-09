using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZergMoney.Helpers;
using ZergMoney.Models;

namespace ZergMoney.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? id)
        { 
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            household.Accounts = db.PersonalAccounts.Where(h => h.HouseholdId == id).ToList();
            ViewBag.AccountId = new SelectList(household.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name");
            ViewBag.Transactions = db.Transactions.Where(i => i.HouseHoldId == id);
            ViewBag.Users = db.Users.Where(i => i.HouseholdId == id);
            return View(household);
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
                var id = User.Identity.GetHouseholdId();
                return RedirectToAction("Index", "Home", new { id });
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
                    vm.HouseHoldId = result.HouseholdId;
                    vm.HHName = result.Household.Name;
                    
                    //Set USED flag to true for this invite                  
                    result.HasBeenUsed = true;
                    db.SaveChanges();

                    return RedirectToAction("InvRegister", "Account", new { HouseholdId = vm.HouseHoldId, Email = result.Email });  
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