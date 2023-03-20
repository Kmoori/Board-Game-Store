using MusicStore.EntityContext;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using MusicStore.ViewModels;



namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            //// Retrieve the album from the database
            //var addedAlbum = storeDB.Termekek
            //.Single(u => u.Id == id);


            //// Add it to the shopping cart
;



            List<Termekek> Termek = new List<Termekek>();
            string sql = "Select * from Termekek where Id ='" + id + "';";

            SqlDataReader datareader = data_read(sql);
            while (datareader.Read())
            {
                Termek.Add(new Termekek { Id = Convert.ToInt32(datareader["Id"].ToString()), nev = datareader["nev"].ToString(), ar = Convert.ToInt32(datareader["ar"].ToString()), tipus = datareader["tipus"].ToString(), kep = datareader["kep"].ToString() });
            }
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(Termek[0]);
            
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
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
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string albumName = storeDB.Carts
            .Single(item => item.RecordId == id).Album.Title;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel()
            {
                Message = Server.HtmlEncode(albumName) +
                "has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        //返回的一个子视图  
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView(cart);
        }



       
    }
}