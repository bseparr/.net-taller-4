using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class MODELOController : Controller
    {
        private testEntities2 db = new testEntities2();

        public ActionResult Index()
        {
            var mODELO = db.MODELO.Include(m => m.MARCA);
            return View(mODELO.ToList());
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MODELO mODELO = db.MODELO.Find(id);
            if (mODELO == null)
            {
                return HttpNotFound();
            }
            return View(mODELO);
        }

        public ActionResult Create()
        {
            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_MODELO,ID_MARCA,DESCRIPCION_MODELO")] MODELO mODELO)
        {
            if (ModelState.IsValid)
            {
                var patejemplo = db.MODELO.Where(x => x.DESCRIPCION_MODELO == mODELO.DESCRIPCION_MODELO);
               
                if (patejemplo == null || patejemplo.Count() == 0)
                {
                    db.MODELO.Add(mODELO);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mODELO.ID_MARCA);
                    ModelState.AddModelError("DESCRIPCION_MODELO", "Modelo ya registrado");
                    return View(mODELO);
                }
                
            }

            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mODELO.ID_MARCA);
            return View(mODELO);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MODELO mODELO = db.MODELO.Find(id);
            if (mODELO == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mODELO.ID_MARCA);
            return View(mODELO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_MODELO,ID_MARCA,DESCRIPCION_MODELO")] MODELO mODELO)
        {
            if (ModelState.IsValid)
            {
                var patejemplo = db.MODELO.Where(x => x.DESCRIPCION_MODELO == mODELO.DESCRIPCION_MODELO);

                if (patejemplo.Count() < 1)
                {
                    db.Entry(mODELO).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mODELO.ID_MARCA);
                    ModelState.AddModelError("DESCRIPCION_MODELO", "Modelo ya registrado");
                    return View(mODELO);
                }
                
            }
            ViewBag.ID_MARCA = new SelectList(db.MARCA, "ID_MARCA", "DESCRIPCION", mODELO.ID_MARCA);
            return View(mODELO);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MODELO mODELO = db.MODELO.Find(id);
            if (mODELO == null)
            {
                return HttpNotFound();
            }
            return View(mODELO);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MODELO mODELO = db.MODELO.Find(id);
            db.MODELO.Remove(mODELO);
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
