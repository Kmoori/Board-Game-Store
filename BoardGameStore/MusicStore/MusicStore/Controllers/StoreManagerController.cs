using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using MusicStore.EntityContext;
using System.Data.SqlClient;
using System.Configuration;

namespace MusicStore.Controllers
{
    [Authorize]
    public class StoreManagerController : Controller
    {
        private MusicStoreEntities db = new MusicStoreEntities();

        // GET: /StoreManager/
        public ActionResult Index()
        {
            List<Termekek> minden = new List<Termekek>();
            string sqlkategoriak = "select * from Termekek;";
            SqlDataReader datareader2 = data_read(sqlkategoriak);
            while (datareader2.Read())
            {
                minden.Add(new Termekek { Id = Convert.ToInt32(datareader2["Id"].ToString()), nev = datareader2["nev"].ToString(), ar = Convert.ToInt32(datareader2["ar"].ToString()), tipus = datareader2["tipus"].ToString(), kep = datareader2["kep"].ToString() });
            }
            ViewBag.minden = minden;

            return View();
        }
        public SqlDataReader data_read(string sql)
        {
            string con_str = ConfigurationManager.ConnectionStrings["MusicStoreEntities"].ConnectionString;
            SqlConnection connection = new SqlConnection(con_str);
            SqlCommand sql_cmd = new SqlCommand(sql, connection);
            sql_cmd.Connection.Open();
            SqlDataReader data_reader = sql_cmd.ExecuteReader();
            return data_reader;
        }

        // GET: /StoreManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: /StoreManager/Create
        public ActionResult Create()
        {
 
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {

            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // GET: /StoreManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // GET: /StoreManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
            if (album == null)
            {
                //404方法
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
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
