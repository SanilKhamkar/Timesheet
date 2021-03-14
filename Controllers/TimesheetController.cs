using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using emptime.Models;
using Microsoft.Ajax.Utilities;

namespace emptime.Controllers
{
    public class TimesheetController : Controller
    {
        // GET: PunchIn
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["databasestring"].ConnectionString);
        string user = ConfigurationManager.AppSettings["username"];
        string pass = ConfigurationManager.AppSettings["password"];
        string email = ConfigurationManager.AppSettings["email"];

        // Send Email when a user punches in
        public ActionResult Punchin()
        {            
            try
            {
                if (ModelState.IsValid)
                {                    
                    connection.Open();
                    
                    // Insert Punchin date/time into database
                    string insertquery = "Insert into Timesheet (Username, Punchin) values(@Username, @Punchin)";
                    using (SqlCommand cmd = new SqlCommand(insertquery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", Session["Username"]);
                        cmd.Parameters.AddWithValue("@Punchin", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }                    
                    
                    // Send email notifying the Punch in
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(email);
                        mail.To.Add(Session["Emailaddress"].ToString());
                        mail.Subject = "PunchIn Information";
                        mail.Body = "<h3>Hello " + Session["Username"] + "</h3>" + "\n" +
                            "<h3>Punchin saved at Current Time: " + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");                           
                        mail.IsBodyHtml = true;                       

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(user, pass);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }

                    var model = new Punch();
                    model.punchin = "punchin";
                    return View("~/Views/Timesheet/PunchDetail.cshtml", model);
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

            return View("~/Views/Timesheet/Error.cshtml");
        }

        // Send Email when a user punches out
        public ActionResult Punchout()
        {            
            try
            {
                if (ModelState.IsValid)
                {
                    connection.Open();

                    // Insert Punchout date/time into database
                    string updatequery = "Update Timesheet SET Punchout = @Punchout where Punchin = (Select max(Punchin) from Timesheet) AND Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(updatequery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", Session["Username"]);
                        cmd.Parameters.AddWithValue("@Punchout", DateTime.Now);

                        cmd.ExecuteNonQuery();
                    }

                    // Send email notifying the Punch out
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(email);
                        mail.To.Add(Session["Emailaddress"].ToString());
                        mail.Subject = "Punchout Information";
                        mail.Body = "<h3>Hello " + Session["Username"] + "</h3>" + "\n" +
                            "<h3>Punchout saved at Current Time: " + DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential(user, pass);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }

                    var model = new Punch();
                    model.punchout = "punchout";
                    return View("~/Views/Timesheet/PunchDetail.cshtml", model);
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
            return View("~/Views/Timesheet/Error.cshtml");
        }
    }
}