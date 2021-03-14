using emptime.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace emptime.Controllers
{
    public class RegistrationController : Controller
    {      
        // GET: Registration
        
        // Registration form
        public ActionResult RegistrationPage()
        {
            return View();
        }

        // Registration Success Page
        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        // Insert Registration records to database
        [HttpPost]
        public ActionResult RegistrationPage([Bind] RegistrationTable rt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["databasestring"].ConnectionString);
            try
            {
                if (ModelState.IsValid)
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * from Registration where EmpID = @empid", connection);
                    command.Parameters.AddWithValue("@empid", rt.EmpID);
                    SqlDataReader rd = command.ExecuteReader();
                    
                    // Check if EmpId exists else insert data in table
                    if (rd.HasRows)
                    {
                        ModelState.AddModelError("EmpID", "EmpID already in use. Please use another one.");
                        return View();
                    }
                    else
                    {
                        string insertquery = "Insert into Registration values(@FirstName, @LastName, @EmpID, @Username, @Password, @Email)";
                        using (SqlCommand cmd = new SqlCommand(insertquery, connection))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", rt.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", rt.LastName);
                            cmd.Parameters.AddWithValue("@EmpID", rt.EmpID);
                            cmd.Parameters.AddWithValue("@Username", rt.Username);
                            cmd.Parameters.AddWithValue("@Password", rt.Password);
                            cmd.Parameters.AddWithValue("@Email", rt.Email);

                            cmd.ExecuteNonQuery();
                        }

                        return RedirectToAction("RegistrationSuccess");
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