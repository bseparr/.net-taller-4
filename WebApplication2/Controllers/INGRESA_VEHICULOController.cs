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
    public class INGRESA_VEHICULOController : Controller
    {
        private testEntities2 db = new testEntities2();

        public ActionResult Index()
        {
            
            var iNGRESA_VEHICULO = db.INGRESA_VEHICULO.Include(i => i.AUTOS);


            return View(iNGRESA_VEHICULO.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INGRESA_VEHICULO iNGRESA_VEHICULO = db.INGRESA_VEHICULO.Find(id);
            if (iNGRESA_VEHICULO == null)
            {
                return HttpNotFound();
            }
            return View(iNGRESA_VEHICULO);
        }

        public ActionResult Create()
        {
            ViewData["servicios"] = db.SERVICIOS.ToList();
            ViewBag.ID_AUTO = new SelectList(db.AUTOS, "ID_AUTO", "PATENTE");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "INGR_ID,ID_AUTO,INGR_FECHA_INGRESO,INGR_RUT_CLIENTE")] INGRESA_VEHICULO iNGRESA_VEHICULO, string[] servicios)
        {
            if (ModelState.IsValid)
            {
                foreach(string value in servicios)
                {
                    SERVICIOS servi = new SERVICIOS();
                    int idaux = int.Parse(value);
                    servi = db.SERVICIOS.Where(x => x.SERVI_ID == idaux).First();

                    iNGRESA_VEHICULO.SERVICIOS.Add(servi);

                }

                db.INGRESA_VEHICULO.Add(iNGRESA_VEHICULO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_AUTO = new SelectList(db.AUTOS, "ID_AUTO", "ID_MODELO", iNGRESA_VEHICULO.ID_AUTO);
            return View(iNGRESA_VEHICULO);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INGRESA_VEHICULO iNGRESA_VEHICULO = db.INGRESA_VEHICULO.Find(id);
            if (iNGRESA_VEHICULO == null)
            {
                return HttpNotFound();
            }
            List<SERVICIOS> servi_autos = iNGRESA_VEHICULO.SERVICIOS.ToList();
            List<SERVICIOS> servicios = db.SERVICIOS.ToList();
            List<ServiciosB> resultado = new List<ServiciosB>();

            foreach(SERVICIOS item in servicios)
            {
                ServiciosB serviAux = new ServiciosB();
                serviAux.SERVI_ID = item.SERVI_ID;
                serviAux.SERVI_DESCRIPCION = item.SERVI_DESCRIPCION;
                serviAux.existe = servi_autos.Exists(x => x.SERVI_ID == item.SERVI_ID);
                resultado.Add(serviAux);
            }

            ViewData["servicios"] = resultado;
            ViewBag.ID_AUTO = new SelectList(db.AUTOS, "ID_AUTO", "ID_MODELO", iNGRESA_VEHICULO.ID_AUTO);
            return View(iNGRESA_VEHICULO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "INGR_ID,ID_AUTO,INGR_FECHA_INGRESO,INGR_RUT_CLIENTE")] INGRESA_VEHICULO iNGRESA_VEHICULO)
        {
            if (ModelState.IsValid)
            {
             

                db.Entry(iNGRESA_VEHICULO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_AUTO = new SelectList(db.AUTOS, "ID_AUTO", "ID_MODELO", iNGRESA_VEHICULO.ID_AUTO);
            return View(iNGRESA_VEHICULO);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INGRESA_VEHICULO iNGRESA_VEHICULO = db.INGRESA_VEHICULO.Find(id);
            if (iNGRESA_VEHICULO == null)
            {
                return HttpNotFound();
            }
            return View(iNGRESA_VEHICULO);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            INGRESA_VEHICULO iNGRESA_VEHICULO = db.INGRESA_VEHICULO.Find(id);
            db.INGRESA_VEHICULO.Remove(iNGRESA_VEHICULO);
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
