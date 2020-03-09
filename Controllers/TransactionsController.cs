using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZergMoney.Helper;
using ZergMoney.Models;

namespace ZergMoney.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.Category).Include(t => t.EnteredBy);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.PersonalAccounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,CategoryId,Description,HouseHoldId,Void,IsDeleted")] Transaction transaction, HttpPostedFileBase image, string Deposit)
        {
            if (ModelState.IsValid)
            {
                if (Deposit == "Deposit")
                {
                    TR tr = new TR();
                    var fileName = Path.GetFileName(image.FileName);
                    var userIdString = User.Identity.GetUserId().ToString();
                    Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "/Uploads/" + userIdString);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/" + userIdString + "/"), fileName));
                    Image img = Image.FromStream(image.InputStream);
                    var result = tr.ReadText(img).Text;
                    var DTTotal = tr.Scan(result);
                    transaction.EnteredById = User.Identity.GetUserId();
                    transaction.Reconciled = true;
                    transaction.Date = DTTotal.Date;
                    transaction.Amount = DTTotal.Amount;
                    transaction.Deposit = "Deposit";
                    db.Transactions.Add(transaction);
                    var acc = db.PersonalAccounts.FirstOrDefault(a => a.Id == transaction.AccountId);
                    acc.Balance += transaction.Amount;
                    acc.ReconciledBalance += transaction.Amount;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home", new { id = transaction.HouseHoldId });
                }
                else
                {
                    TR tr = new TR();
                    var fileName = Path.GetFileName(image.FileName);
                    var userIdString = User.Identity.GetUserId().ToString();
                    Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "/Uploads/" + userIdString);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/" + userIdString + "/"), fileName));
                    Image img = Image.FromStream(image.InputStream);
                    var result = tr.ReadText(img).Text;
                    var DTTotal = tr.Scan(result);
                    transaction.EnteredById = User.Identity.GetUserId();
                    transaction.Reconciled = true;
                    transaction.Date = DTTotal.Date;
                    transaction.Amount = -Math.Abs(DTTotal.Amount);
                    transaction.Deposit = "Withdrawal";
                    db.Transactions.Add(transaction);
                    var acc = db.PersonalAccounts.FirstOrDefault(a => a.Id == transaction.AccountId);
                    acc.Balance += transaction.Amount;
                    acc.ReconciledBalance += transaction.Amount;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home", new { id = transaction.HouseHoldId });
                }
            }

            ViewBag.AccountId = new SelectList(db.PersonalAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.PersonalAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,Description,Date,Amount,Type,Void,CategoryId,EnteredById,Reconciled,ReconciledAmount,IsDeleted")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.PersonalAccounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            ViewBag.EnteredById = new SelectList(db.Users, "Id", "FirstName", transaction.EnteredById);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
