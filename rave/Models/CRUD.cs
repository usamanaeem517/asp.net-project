using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;

namespace Rave.Models
{
    public class CRUD
    {
        public static string connectionString = "data source=localhost; Initial Catalog=rave;Integrated Security=true";
        public static int SignUp(string name, string email, string pass, string account, out string message)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("signup", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 20).Value = name;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 20).Value = pass;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                cmd.Parameters.Add("@acc_type", SqlDbType.NVarChar, 20).Value = account;
                cmd.ExecuteNonQuery();
                message = "Sign Up Successful!";
               // utilities.sendMail(email,"Sign-Up" ,"Hello! Welcome to RAVE-DB.");
               /* This code is comented to avoid sending unnecessary emails */

            }

            catch (SqlException ex)
            {
                message = (ex.Message.ToString());
                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int Login(string email, string pass, string account, out string message)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(connectionString);
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("signin", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                int acc_type=0;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 20).Value = pass;
                if (String.Equals(account, "User"))
                {
                    acc_type = 1;
                }
                else if (String.Equals(account, "Singer"))
                {
                    acc_type = 2;
                }
                else {
                    acc_type = 3;
                }
                cmd.Parameters.Add("@acc_type", SqlDbType.Int).Value = acc_type;
                cmd.ExecuteNonQuery();
                message = "Sign In Successful!";
             

            }

            catch (SqlException ex)
            {
                message = (ex.Message.ToString());
                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static int add_song(string name, string sID, string album, string duration, string link, string rdate, string genre) {
            int result = 0;
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("addingsong", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 20).Value = name;
                cmd.Parameters.Add("@singerID", SqlDbType.Int).Value = sID;
                cmd.Parameters.Add("@Album_Title", SqlDbType.NVarChar, 50).Value = album;
                cmd.Parameters.Add("@Duration", SqlDbType.Time).Value = duration;
                cmd.Parameters.Add("@Link_Video", SqlDbType.NVarChar, 2083).Value = link;
                cmd.Parameters.Add("@Release_Date", SqlDbType.Date).Value = rdate;
                cmd.Parameters.Add("@Genre", SqlDbType.NVarChar, 20).Value = genre;
                
                cmd.ExecuteNonQuery();

            }

            catch (SqlException ex)
            {
              
                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;

        }

        public static int add_concert(string name, string cID, string date, string time, string venue, string price, string seats,string location, string s_email)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(connectionString);
           
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("add_concert", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 20).Value = name;
                cmd.Parameters.Add("@cID", SqlDbType.Int).Value = cID;
                cmd.Parameters.Add("@date", SqlDbType.Date).Value = date;
                cmd.Parameters.Add("@time", SqlDbType.Time).Value = time;
                cmd.Parameters.Add("@venue", SqlDbType.NVarChar, 20).Value = venue;
                cmd.Parameters.Add("@price", SqlDbType.Int).Value = price;
                cmd.Parameters.Add("@seats", SqlDbType.Int).Value = seats;
                cmd.Parameters.Add("@location", SqlDbType.NVarChar, 2083).Value = location;
                cmd.Parameters.Add("@s_email", SqlDbType.NVarChar, 40).Value = s_email;


                cmd.ExecuteNonQuery();


            }

            catch (SqlException ex)
            {

                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;

        }
        public static Singer fetch_singer(string email) {
            SqlConnection con = new SqlConnection(connectionString);
            Singer details = new Singer();
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_singer_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details.id = reader["ID"].ToString();
                    details.name = reader["Name"].ToString();
                    details.account_type=reader["acc_type"].ToString();

                }
                reader.Close();
                con.Close();

                return details;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static User fetch_user(string email)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            User details = new User();
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("fetch_user_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details.id = reader["ID"].ToString();
                    details.name = reader["Name"].ToString();
                    details.account_type = reader["acc_type"].ToString();

                }
                reader.Close();
                con.Close();

                return details;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }
        public static Manager fetch_manager(string email)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            Manager details = new Manager();
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("fetch_manager_details", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50).Value = email;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    details.id = reader["ID"].ToString();
                    details.name = reader["Name"].ToString();
                    details.account_type = reader["acc_type"].ToString();

                }
                reader.Close();
                con.Close();

                return details;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static int get_songID(string sID, string sname, string album, out string songID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            int result = 0;
            songID = "0";
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("get_songID", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sID", SqlDbType.Int).Value = sID;
                cmd.Parameters.Add("@song_name", SqlDbType.NVarChar,20).Value = sname;
                cmd.Parameters.Add("@album_title", SqlDbType.NVarChar,20).Value = album;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    songID = reader["ID"].ToString();

                }
                reader.Close();
                con.Close();
                return result;

            }

            catch (SqlException ex)
            {
                result = -1;
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                    
            }
            return result;
        }

        public static List<Song> get_songs(string sID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_songs", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sID", SqlDbType.Int).Value = sID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Song> list = new List<Song>();
                while (rdr.Read())
                {
                    Song song = new Song();
                    song.ID = rdr["id"].ToString();
                    song.song_name = rdr["name"].ToString();
                    song.album_name = rdr["album_title"].ToString();
                    song.singer_name = rdr["sname"].ToString();
                    song.release_date = rdr["Release_Date"].ToString();
                    song.duration = rdr["duration"].ToString();
                    song.genre = rdr["genre"].ToString();
                    song.rating = rdr["rating"].ToString();
                    song.v_link = rdr["Link_Video"].ToString();
                    list.Add(song);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }


        public static List<string> get_emails()
        {
            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("get_emails", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<string> list = new List<string>();
                while (rdr.Read())
                {
                    string email = rdr["email"].ToString();
                    list.Add(email);
                }
                rdr.Close();
                con.Close();

                return list;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Concert> get_all_up_concerts()
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("get_all_up_concerts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.ID = rdr["ID"].ToString();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString();
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Concert> fetch_attending_concerts(string ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_attending_concerts", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = ID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString(); 
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }


        public static List<Concert> get_up_concerts_for_singer(string ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("concert_upcoming_singer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sID", SqlDbType.Int).Value = ID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString();
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Concert> get_up_concerts_for_manager(string ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("concert_upcoming_manager", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@mID", SqlDbType.Int).Value = ID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString();
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }
        public static List<Concert> get_fin_concerts_for_singer(string ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("concert_finished_singer", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sID", SqlDbType.Int).Value = ID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString();
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Concert> get_fin_concerts_for_manager(string ID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("concert_finished_manager", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@mID", SqlDbType.Int).Value = ID;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Concert> list = new List<Concert>();
                while (rdr.Read())
                {
                    Concert obj = new Concert();
                    obj.name = rdr["Name"].ToString();
                    obj.date = rdr["date"].ToString();
                    obj.time = rdr["time"].ToString();
                    obj.venue = rdr["venue"].ToString();
                    obj.price = rdr["price"].ToString();
                    obj.seatstaken = rdr["seatstaken"].ToString();
                    obj.seatsleft = rdr["seatsleft"].ToString();
                    obj.location_link = rdr["location"].ToString();
                    list.Add(obj);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Song> get_all_songs()
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_all_songs", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Song> list = new List<Song>();
                while (rdr.Read())
                {
                    Song song = new Song();
                    song.ID = rdr["id"].ToString();
                    song.song_name = rdr["name"].ToString();
                    song.album_name = rdr["album_title"].ToString();
                    song.singer_name = rdr["sname"].ToString();
                    song.release_date = rdr["Release_Date"].ToString();
                    song.duration = rdr["duration"].ToString();
                    song.genre = rdr["genre"].ToString();
                    song.rating = rdr["rating"].ToString();
                    song.v_link = rdr["Link_Video"].ToString();
                    list.Add(song);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<Song> fetch_trending()
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_trending", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Song> list = new List<Song>();
                while (rdr.Read())
                {
                    Song song = new Song();
                    song.ID = rdr["id"].ToString();
                    song.song_name = rdr["name"].ToString();
                    song.album_name = rdr["album_title"].ToString();
                    song.singer_name = rdr["sname"].ToString();
                    song.release_date = rdr["Release_Date"].ToString();
                    song.duration = rdr["duration"].ToString();
                    song.genre = rdr["genre"].ToString();
                    song.rating = rdr["rating"].ToString();
                    song.v_link = rdr["Link_Video"].ToString();
                    list.Add(song);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static int add_to_concert_attend(string c_id, string u_id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("add_to_concert_attend", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = u_id;
                cmd.Parameters.Add("@concertID", SqlDbType.Int).Value = c_id;

                cmd.ExecuteNonQuery();
                return 1;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return -1;

            }
            finally
            {
                con.Close();
            }
        }

        public static int add_to_playlist(string s_id, string u_id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("add_to_playlist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = u_id;
                cmd.Parameters.Add("@songID", SqlDbType.Int).Value = s_id;

                cmd.ExecuteNonQuery();
                return 1;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return -1;

            }
            finally
            {
                con.Close();
            }
        }

        public static List<Song> fetch_to_playlist(string id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("fetch_to_playlist", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = id;
                SqlDataReader rdr = cmd.ExecuteReader();

                List<Song> list = new List<Song>();
                while (rdr.Read())
                {
                    Song song = new Song();
                    song.ID = rdr["id"].ToString();
                    song.song_name = rdr["name"].ToString();
                    song.album_name = rdr["album_title"].ToString();
                    song.singer_name = rdr["sname"].ToString();
                    song.release_date = rdr["Release_Date"].ToString();
                    song.duration = rdr["duration"].ToString();
                    song.genre = rdr["genre"].ToString();
                    song.rating = rdr["rating"].ToString();
                    song.v_link = rdr["Link_Video"].ToString();
                    list.Add(song);
                }
                rdr.Close();
                con.Close();

                return list;


            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }
        public static int rate_song(string userID, string songID, string rating)
        {
            int result = 0;
            SqlConnection con = new SqlConnection(connectionString);
            
            try
            {
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("rate_song", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@songID", SqlDbType.Int).Value = songID;
                cmd.Parameters.Add("@rating", SqlDbType.Float).Value = rating;
                cmd.ExecuteNonQuery();


            }

            catch (SqlException ex)
            {

                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;

        }

    }

    public class utilities {
        public static void sendMail(string to, string body ,string emailText)
        {
            MailMessage message = new MailMessage("rave.database@gmail.com", to);
            message.Subject = body;
            message.Body = emailText;
            SmtpClient sC = new SmtpClient("smtp.gmail.com", 587);
            sC.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "rave.database@gmail.com",
                Password = "rave@gmail"
            };
            sC.EnableSsl = true;
            sC.Send(message);
        }
    }
}