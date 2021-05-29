using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rave.Models
{
    public class User_Songs
    {
        public User user { get; set; }
        public List<Song> songs { get; set; }

        public List<Song> playlist_songs { get; set; }

        public List<Song> trending_songs { get; set; }

        public List<Concert> concerts_upcoming { get; set; }
        public List<Concert> concerts_attending { get; set; }
    }

}