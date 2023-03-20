using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace MusicStore.Controllers
{
    public class KosarController : Controller
    {
        // GET: Kosar
        public ActionResult Index()
        {
            //Termékek
            List<Termekek> Termek = new List<Termekek>();
            string termekek = "select * from Termekek join Carts on Termekek.Id = Carts.TermekId where Carts.TermekId = Termekek.Id ;";
            SqlDataReader datareader = data_read(termekek);
            while (datareader.Read())
            {
                Termek.Add(new Termekek { Id = Convert.ToInt32(datareader["Id"].ToString()), nev = datareader["nev"].ToString(), ar = Convert.ToInt32(datareader["ar"].ToString()), tipus = datareader["tipus"].ToString(), kep = datareader["kep"].ToString() });
            }
            ViewBag.vb_Termek = Termek;

            //Kosar
            List<Carts> kosarTart = new List<Carts>();
            string kosarban = "select * from Carts;";
            SqlDataReader datareader2 = data_read(kosarban);
            while (datareader2.Read())
            {
                kosarTart.Add(new Carts { CartId = Convert.ToInt32(datareader2["CartId"].ToString()), TermekId = Convert.ToInt32(datareader2["TermekId"].ToString()), Mennyiseg = Convert.ToInt32(datareader2["Mennyiseg"].ToString()) });
            }
            ViewBag.vb_kosarban = kosarTart;

            int Ar = 0;
            string ara = "select sum(ar) as osszeg from Termekek join Carts on Termekek.Id = Carts.TermekId where Carts.TermekId = Termekek.Id;";
            SqlDataReader datareader3 = data_read(ara);
            while (datareader3.Read())
            {
                Ar = Convert.ToInt32(datareader3["osszeg"].ToString());
            }
            ViewBag.Ar = Ar;

            return View();
        }

        public ActionResult Kosarba(string TermekId)
        {
            string sql_cmd = "insert into Carts(TermekId,Mennyiseg) values("  + TermekId + "," + 1 + ");";
            sql_parancsok(sql_cmd);

            return RedirectToAction("Index", "Kosar");
        }

        public ActionResult Torles(string torlendoId)
        {
            string sql_cmd = "delete from Carts where CartId = " + torlendoId + ";";
            sql_parancsok(sql_cmd);

            return RedirectToAction("Index", "Kosar");
        }

        public void sql_parancsok(string sql)
        {
            string con_str = ConfigurationManager.ConnectionStrings["MusicStoreEntities"].ConnectionString;
            SqlConnection connection = new SqlConnection(con_str);
            SqlCommand sql_cmd = new SqlCommand(sql, connection);
            sql_cmd.Connection.Open();
            sql_cmd.ExecuteNonQuery();
            sql_cmd.Connection.Close();
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

