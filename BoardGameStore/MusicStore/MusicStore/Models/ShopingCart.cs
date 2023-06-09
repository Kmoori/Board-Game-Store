﻿using MusicStore.EntityContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace MusicStore.Models
{
    public class ShoppingCart
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        string ShoppingCartId { get; set; }
        //存在Session中的键值 保存ShoppingCartId
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Termekek termek)
        {
            // Get the matching cart and album instances
            var cartItem = storeDB.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.AlbumId == termek.Id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart()
                {
                    AlbumId=termek.Id,
                    CartId=ShoppingCartId,
                    Count=1,
                    DateCreated=DateTime.Now
                };
                storeDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            storeDB.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = storeDB.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    storeDB.Carts.Remove(cartItem);
                }
                storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                storeDB.Carts.Remove(cartItem);
            }
            storeDB.SaveChanges();
        }
        public List<Cart> GetCartItems()
        {
            //return storeDB.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
            List<Cart> Kosar = new List<Cart>();
            string sqlkategoriak = "select * from Carts;";
            SqlDataReader datareader2 = data_read(sqlkategoriak);
            while (datareader2.Read())
            {
                Kosar.Add(new Cart { CartId = datareader2["CartId"].ToString()});
            }
            return Kosar;
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

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up

            // Return 0 if all entries are null
            return 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItem in storeDB.Carts
                              where cartItem.CartId == ShoppingCartId
                              select (int?)cartItem.Count * cartItem.Album.Price)
                                .Sum();
            return total ?? 0;
        }
        public int CreateOrder(Order order)
        {
            //order have create and is going to update information
            decimal orderTotal = 0;
            var cartItem = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach(var item in cartItem)
            {
                var orderDetail=new OrderDetail()
                {
                    AlbumId=item.AlbumId,
                    OrderId=order.OrderId,
                    UnitPrice=item.Album.Price,
                    Quantity=item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);
                storeDB.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            // Save the order
            storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;

        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrEmpty(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            //var shoppingCart = storeDB.Carts.Where(c => c.CartId == ShoppingCartId);
            //foreach (Cart item in shoppingCart)
            //{
            //    item.CartId = userName;
            //}
            //storeDB.SaveChanges();
        }
    }
}