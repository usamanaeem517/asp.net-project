using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rave.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.CodeDom;
using System.Web.Services.Description;

namespace Rave.Controllers
{
   
    public class MyFirstController : Controller
    {
        

        public ActionResult authenticate_Login(String email, String pass, String account)
        {
            int result = CRUD.Login(email, pass, account, out string message);
            if (result == -1)
                return View("login", (object)message);
            if (String.Equals(message, "Sign In Successful!"))
            {
                if (String.Equals(account, "Singer")) { // check if login is for singer
                    Session["singer"] = CRUD.fetch_singer(email);
                    return this.RedirectToAction("singer_", "MyFirst");
                }
                if (String.Equals(account, "User")) // check if login is for user
                {
                    Session["user"] = CRUD.fetch_user(email);
                    return this.RedirectToAction("user_", "MyFirst");
                }
                if (String.Equals(account, "Manager"))
                {
                    Session["manager"] = CRUD.fetch_manager(email); // check if login is for manager
                    return this.RedirectToAction("manager_", "MyFirst");
                }
            }
            return View("login", (object)message);
        }
        public ActionResult login()
        {
            return View();
        }
        public ActionResult authenticate_SignUp(String name, String email, String pass, String account)
        {
            // authenticate sign up process and call sign up procedure from Database
            if (pass.Length < 8) 
                return View("rave_signup");
            
            int result = CRUD.SignUp(name, email, pass, account, out string message);

            if (result == -1)
            {
                return View("rave_signup", (object)message);
            }

            return View("login", (object)message);
        }

        public ActionResult Error(String msg)
        {
            // A view that displays an error in a way of exception handling
            return View("Error", (object)msg);
        }

        public ActionResult rave_signup() {
            return View();
        }
        
        public ActionResult singer_()
        {
            // singer controller which returns a list of songs, concerts and a session of the singer
            if (Session["singer"] == null)
            {
                return this.RedirectToAction("login");
            }
            Singer_Songs data_model = new Singer_Songs();
            data_model.singer = (Singer)Session["singer"];
            data_model.songs = CRUD.get_songs(data_model.singer.id);
            data_model.concerts_upcoming = CRUD.get_up_concerts_for_singer(data_model.singer.id);
            data_model.concerts_finished = CRUD.get_fin_concerts_for_singer(data_model.singer.id);
            return View(data_model);
        }

        public ActionResult manager_()
        {
            // main manager controller which returns the manager's concerts and session
            if (Session["manager"] == null)
            {
                return this.RedirectToAction("login");
            }
            Manager_Concerts data_model = new Manager_Concerts();
            data_model.manager = (Manager)Session["manager"];
            data_model.concerts_upcoming = CRUD.get_up_concerts_for_manager(data_model.manager.id);
            data_model.concerts_finished = CRUD.get_fin_concerts_for_manager(data_model.manager.id);
            return View(data_model);
        }


        public ActionResult user_()
        {
            // main user controller which returns songs, concerts, and session
            if (Session["user"] == null)
            {
                return this.RedirectToAction("login");
            }
            User_Songs data_model = new User_Songs();
            
            data_model.songs = CRUD.get_all_songs();
            data_model.user = (User)Session["user"];
            data_model.playlist_songs = CRUD.fetch_to_playlist(data_model.user.id);
            data_model.trending_songs = CRUD.fetch_trending();
            data_model.concerts_upcoming = CRUD.get_all_up_concerts();
            data_model.concerts_attending = CRUD.fetch_attending_concerts(data_model.user.id);
            return View(data_model);
        }

        public ActionResult add_concert(String c_name, String cID, String date, String time, String venue, String price, String seats,String location ,String s_email) {
            
            // calls the add concert procedure and the email function
            int result = CRUD.add_concert(c_name, cID, date, time, venue, price, seats, location,s_email);
            if (result == -1)
            {
                var message = "There was an issue adding concerts.";
                return View("Error", (object)message);
            }

            List<string> emails = CRUD.get_emails();

            Manager manager = (Manager)Session["manager"];

            for (var i = 0; i < emails.Count; i++)
            {
                utilities.sendMail(emails[i], "New Concert!", manager.name + " added a new concert: " + c_name);

            }
            return this.RedirectToAction("manager_", "MyFirst");
        }

        [HttpPost]
        public ActionResult upload(IEnumerable<HttpPostedFileBase> files, String sname,String sID, String album, String genre, String rdate, String v_link, String duration)
        {
            // the upload function which uses the post method to send files to server
            int temp = 0;
            string ID="";
            foreach (var file in files) {
                if (file != null && file.ContentLength > 0)
                    try
                    {
                        bool format = false;
                        string Fileformat = "";
                        
                        for (int i = 0; i < file.FileName.Length; i++) {
                            if (file.FileName[i] == '.')
                            {
                                format = true;
                            }
                            if (format)
                                Fileformat += file.FileName[i];
                        }

                        if (temp == 0)
                        {
                            int result = CRUD.add_song(sname, sID, album, duration, v_link, rdate, genre);
                            if (result == -1)
                            {
                                ViewBag.Message = "ERROR (SQL):";
                            }

                            result = CRUD.get_songID(sID, sname,album,out string songID);
                            ID = songID;
                            temp = 1;
                        }
                        string path = Path.Combine(Server.MapPath("~/Music"),
                        Path.GetFileName(ID + Fileformat));
                        file.SaveAs(path);

                        ViewBag.Message = "File uploaded successfully";

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR:" + ex.Message.ToString();
                    }
                else
                {
                    ViewBag.Message = "You have not specified a file.";
                }

               
            }

            /*List<string> emails = CRUD.get_emails();

            Singer singer = (Singer)Session["singer"];

            for (var i = 0; i < emails.Count; i++)
            {
                utilities.sendMail(emails[i], "New Song!", singer.name + " added a new song: " + sname);

            }*/
            // Commented as described in the note.

            return this.RedirectToAction("singer_");
        }

        [HttpPost]
        public ActionResult add_to_playlist(string id) {
            User u_id = (User)Session["user"];
            int result = CRUD.add_to_playlist(id, u_id.id);
            if (result == -1)
            {
                var message = "There was an issue adding to playlist.";
                return View("Error", (object)message);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            
        }

        [HttpPost]
        public ActionResult add_to_concert_att(string id)
        {
            User u_id = (User)Session["user"];
            int result = CRUD.add_to_concert_attend(id, u_id.id);
            if (result == -1)
            {
                var message = "There was an issue adding concert attendee.";
                return View("Error", (object)message);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);

        }

        [HttpPost]
        public ActionResult rate_song(string id, string rating)
        {
            User u_id = (User)Session["user"];
            int result = CRUD.rate_song(u_id.id,id ,rating);
            if (result == -1)
            {
                var message = "There was an issue in rating songs.";
                return View("Error", (object)message);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        
        public ActionResult logout()
        {
            Session.Abandon();
            return this.RedirectToAction("login", "MyFirst");
            
        }

    }
}
