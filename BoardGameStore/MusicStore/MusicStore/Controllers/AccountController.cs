using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc3ToolsUpdateWeb_Default.Models;
using MusicStore.Models;
using MusicStore.EntityContext;
using System.Data.SqlClient;
using System.Configuration;

namespace Mvc3ToolsUpdateWeb_Default.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model,string UserName,string Password, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                #region 暂时无法使用
                //if (Membership.ValidateUser(model.nev, model.Password))
                //{
                //    FormsAuthentication.SetAuthCookie(model.nev, model.RememberMe);
                //    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                //        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                //    {
                //        return Redirect(returnUrl);
                //    }
                //    else
                //    {
                //        return RedirectToAction("Index", "Home");
                //    }
                //}
                if (ModelState.IsValid)
                {

                }


                #endregion

                string sql_cmd2 = "select nev,jelszo from Felhasznalok";
                SqlDataReader loginread = data_read(sql_cmd2);
                List<LogOnModel> logon_list = new List<LogOnModel>();
                while (loginread.Read())
                {
                    logon_list.Add(new LogOnModel { UserName = Convert.ToString(loginread["nev"].ToString()), Password = Convert.ToString(loginread["jelszo"].ToString()) });
                }
                int index = 0;
                bool igaz = false;
                for (int i = 0; i < logon_list.Count; i++)
                {
                    if (Convert.ToString(logon_list[i].UserName) == UserName && Convert.ToString(logon_list[i].Password) == Password)
                    {
                        index = i;
                        igaz = true;
                    }
                }
                if (Convert.ToString(logon_list[index].UserName) == UserName && Convert.ToString(logon_list[index].Password) == Password && igaz == true)
                {
                    MigrateShoppingCart(UserName);
                    FormsAuthentication.SetAuthCookie(UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Hibás felhasználó név vagy jelszó.");
                }

                MusicStoreEntities storeDB = new MusicStoreEntities();
                bool logintrue = storeDB.logOnModels.Any(u => model.UserName.ToLower() == u.UserName.ToLower() && u.Password == u.Password);
            }
            // If we got this far, something failed, redisplay form
            return View();
        }
        
        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        [HttpPost]
        public ActionResult Register(string UserName,string Email,string Password)
        {
            string sql_cmd = "insert into Felhasznalok(nev,jelszo,email) values('" + UserName + "','" + Password + "','" + Email + "'); ";
            sql_parancsok(sql_cmd);

            return RedirectToAction("LogOn", "Account");
        }

        public ActionResult Register()
        {
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
        public void sql_parancsok(string sql)//SQL parancsok futtatására az adatbázisban
        {
            string con_str = ConfigurationManager.ConnectionStrings["MusicStoreEntities"].ConnectionString;
            SqlConnection connection = new SqlConnection(con_str);
            SqlCommand sql_cmd = new SqlCommand(sql, connection);
            sql_cmd.Connection.Open();
            sql_cmd.ExecuteNonQuery();
            sql_cmd.Connection.Close();
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Guid tCardId = Guid.NewGuid();
            
            Session[ShoppingCart.CartSessionKey] = tCardId.ToString();
            FormsAuthentication.SignOut();
            return RedirectToAction("LogOn", "Account");
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "A jelenlegi vagy az új és jelszó nem megfelelő.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        /// <summary>
        /// 迁移购物车
        /// </summary>
        /// <param name="UserName"></param>
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
