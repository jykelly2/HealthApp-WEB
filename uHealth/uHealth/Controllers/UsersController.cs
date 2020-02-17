using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uHealth.Models;
using System.Windows;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Mail;

namespace uHealth.Controllers
{
    public class UsersController : Controller
    {
        private UserDatabase _context;
       

        public UsersController(UserDatabase context)
        {
            _context = context;
        }

        public IActionResult UserFormView()
        {

            return View();
        }
        public IActionResult ExistingAccountView()
        {

            return View();
        }



        [HttpPost]
        //saves new user to database if it's input email doesn't exist in database
        //otherwise update and save updated information for existing user

        public async Task<IActionResult> Save(User user)
        {
           

            var existingUser = await _context
               .Users
               .FirstOrDefaultAsync(s => s.Email== user.Email);

            var userInDb = await _context.Users.FindAsync(user.Id);


            if (user.Id == 0  && existingUser == null)
                //if (user.Id == 0 )
                {
                _context.Users.Add(user);

                await _context.SaveChangesAsync();
                return View("ConfirmationView", user);

            }
            
            else if (userInDb != null)//edit a users
            {
               
                await TryUpdateModelAsync<User>(
                    userInDb,
                    "",
                    u => u.FirstName, u => u.LastName, u => u.Age,
                    u => u.Email, u => u.Gender, u => u.Height, u => u.Weight,
                    u => u.HealthFocus, u => u.InterestLevel
                );

                await _context.SaveChangesAsync();
                return RedirectToAction("ConfirmationView", userInDb);

            }

            else
            {

                //System.Windows.Forms.MessageBox.("My message here");
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                // System.Web.HttpContext.Current.Response.Write("<SCRIPT LANGUAGE=""JavaScript"">alert("Hello this is an Alert")</SCRIPT>")

                return View("UserFormView", user);
            }

           
        }

        //return confirmation view with user information
        public  IActionResult ConfirmationView(User user)
        {
         
            return View(user);
        }

        //gets user by id and return userform view with user informatino filled
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _context
               .Users
               .FirstOrDefaultAsync(s => s.Id == id);


            if (user == null)
            {
                return NotFound(new { error = "id is not found" });
            }

            return View("UserFormView", user);
        }

        //gets existing user by email and return userform view with user information filled

        public async Task<IActionResult> EditExisting(string email)
        {
            var user = await _context
               .Users
               .FirstOrDefaultAsync(s => s.Email == email);


            if (user == null)
            {
                return NotFound(new { error = "email is not found" });
            }

            return View("UserFormView", user);
        }

        //delete user from database 
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context
               .Users
               .FirstOrDefaultAsync(s => s.Id == id);

            if (user == null)
            {
                return NotFound(new { error = "id is not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        //gets user information with id, calculate BMI, add recommendation website 
        //and send email of results to user email
        public async Task<IActionResult> Submit(int id)
        {
            var user = await _context
              .Users
              .FirstOrDefaultAsync(s => s.Id == id);

            var fromAddress = new MailAddress("uhealthcontanct@gmail.com", "uHealth");
            var toAddress = new MailAddress(user.Email, user.FirstName);
            const string fromPassword = "example15631";
            const string subject = "uHealth - Feedback Results!";
            string body = "";



            double BMI = user.Weight / Math.Pow(user.Height / 100.0, 2);

            BMI = Math.Round(BMI, 2);

            if (user.Gender == "Female")
            {
                if (BMI < 19)
                { body += "BMI result is: " + BMI.ToString() + ", Category: " + " Underweight\n"; }

                else if (BMI >= 19 & BMI <= 24)
                { body += "BMI result is: " + BMI.ToString() + ", Category: " + " Normal\n"; }

                else
                { body += "BMI result is: " + BMI.ToString() + ", Category: " + " Overweight\n"; }

            }

            else
            {
                if (BMI < 20)
                { body += "Your BMI result is: " + BMI.ToString()+ ", Category: " + " Underweight\n"; }

                else if (BMI >= 20 & BMI <= 25)
                { body += "BMI result is: " + BMI.ToString() + ", Category: " + " Normal\n"; }

                else
                { body += "BMI result is: " + BMI.ToString() + ", Category: " + " Overweight\n" ; }

            }
 
            if (user.HealthFocus == "Nutrition")
            {
                body += "Recommended Website:\n"+ "https://www.allrecipes.com/recipes/84/healthy-recipes/ \n";
            }
            else if(user.HealthFocus == "Fitness")
            {
                body += "Recommended Website:\n " + "https://www.muscleandfitness.com/workouts/workout-routines \n";
            }

            else
            {
                body += "Recommended Website:\n" + "https://consumer.healthday.com/general-health-information-16/ \n";

            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }


            return RedirectToAction("Index", "Home");
        }

     
        public IActionResult Create()
        {
            return View("UserViewForm");
        }

       
    }
}
