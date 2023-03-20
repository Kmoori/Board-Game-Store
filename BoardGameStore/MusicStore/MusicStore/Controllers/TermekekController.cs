using MusicStore.EntityContext;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace MusicStore.Controllers
{
    public class TermekekController : Controller
    {
        private MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /Store/
        public ActionResult Index()
        {
            List<string> Kategoria = new List<string>();
            string sqlkategoriak = "select tipus from Termekek group by tipus;";
            SqlDataReader datareader2 = data_read(sqlkategoriak);
            while (datareader2.Read())
            {
                Kategoria.Add(datareader2[0].ToString());
            }
            ViewBag.vb_kategoriak = Kategoria;

            return View();
        }


        //
        // GET: /Store/Browse

        public ActionResult Browse(string tipus)
        {

            List<Termekek> termekek_list = new List<Termekek>();
            string sql = "Select * from Termekek where tipus ='" + tipus + "';";

            SqlDataReader datareader = data_read(sql);
            while (datareader.Read())
            {
                termekek_list.Add(new Termekek { Id = Convert.ToInt32(datareader["Id"].ToString()), nev = datareader["nev"].ToString(), ar = Convert.ToInt32(datareader["ar"].ToString()), tipus = datareader["tipus"].ToString(), kep = datareader["kep"].ToString() });
            }
            ViewBag.mukodj = termekek_list;
            return View();
        }
        // GET: /Store/Details

        public ActionResult Details(int? id)
        {

            List<Termekek> Termek = new List<Termekek>();
            string sql = "Select * from Termekek where Id ='" + id + "';";

            SqlDataReader datareader = data_read(sql);
            while (datareader.Read())
            {
                Termek.Add(new Termekek { Id = Convert.ToInt32(datareader["Id"].ToString()), nev = datareader["nev"].ToString(), ar = Convert.ToInt32(datareader["ar"].ToString()), tipus = datareader["tipus"].ToString(), kep = datareader["kep"].ToString() });
            }
            ViewBag.termek = Termek;

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
    }
}