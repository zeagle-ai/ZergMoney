using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZergMoney.Helpers;
using ZergMoney.Models;

namespace ZergMoney.Controllers
{
    public class InvitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invites
        public ActionResult Index()
        {
            var invites = db.Invites.Include(i => i.Household).Include(i => i.InvitedBy);
            return View(invites.ToList());
        }

        // GET: Invites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        // GET: Invites/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            ViewBag.InvitedById = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Invites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Email,HHToken,InviteDate,InvitedById,HasBeenUsed")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                db.Invites.Add(invite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            ViewBag.InvitedById = new SelectList(db.Users, "Id", "FirstName", invite.InvitedById);
            return View(invite);
        }

        // GET: Invites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            ViewBag.InvitedById = new SelectList(db.Users, "Id", "FirstName", invite.InvitedById);
            return View(invite);
        }

        // POST: Invites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Email,HHToken,InviteDate,InvitedById,HasBeenUsed")] Invite invite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invite.HouseholdId);
            ViewBag.InvitedById = new SelectList(db.Users, "Id", "FirstName", invite.InvitedById);
            return View(invite);
        }

        // GET: Invites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invite invite = db.Invites.Find(id);
            if (invite == null)
            {
                return HttpNotFound();
            }
            return View(invite);
        }

        public ActionResult Invite()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> Invite(string email)
        {
            var code = Guid.NewGuid();
            var callbackUrl = Url.Action("CreateJoinHousehold", "Home", new { code }, protocol: Request.Url.Scheme);
            
            EmailService ems = new EmailService();
            IdentityMessage msg = new IdentityMessage
            {
                Body = "You are invited to join my household!!!" + Environment.NewLine + "Please click the following link to join <a href=\"" + callbackUrl + "\">JOIN</a>",
                Destination = email,
                Subject = "You are invited to join Zerg Money"
            };

            await ems.SendMailAsync(msg);

            //Create record in the Invites table
            Invite model = new Invite
            {
                Email = email,
                HHToken = code,
                HouseholdId = User.Identity.GetHouseholdId().Value,
                InviteDate = DateTime.Now,
                InvitedById = User.Identity.GetUserId()
            };

            db.Invites.Add(model);
            db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
        }
        
        public ActionResult Leavehousehold()
        {
            Household model = db.Households.Find(User.Identity.GetHouseholdId().Value);
            
            return View(model);
        }

        // POST: Invites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invite invite = db.Invites.Find(id);
            db.Invites.Remove(invite);
            db.SaveChanges();
            return RedirectToAction("Index");
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
