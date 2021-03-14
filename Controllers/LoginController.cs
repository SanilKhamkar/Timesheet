using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using emptime.Models;

namespace emptime.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login        

        // Login form
        public ActionResult LoginPage()
        {
            return View();
        }
       
        // Timesheet Page
        public ActionResult Timesheet()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult LoginPage([Bind] Login lg)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["databasestring"].ConnectionString);
            try            {
               
                if (ModelState.IsValid)
                {
                    connection.Open();

                    // Check Username and Password from database 
                    string selectquery = "Select * from Registration where Username = @username and Password = @password";
                    using (SqlCommand cmd = new SqlCommand(selectquery, connection))
                    {                       
                        cmd.Parameters.AddWithValue("@username", lg.Username);
                        cmd.Parameters.AddWithValue("@password", lg.Password);
                        SqlDataReader adapter = cmd.ExecuteReader();
                        if (adapter.Read())
                        {
                            Session["Username"] = lg.Username;
                            Session["Emailaddress"] = adapter.GetString(5);
                            return RedirectToAction("Timesheet");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Username or Password.");
                        }
                    }                        
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            return View();
        }
    }
}