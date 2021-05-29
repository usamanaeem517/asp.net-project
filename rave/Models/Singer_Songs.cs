using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rave.Models
{
    public class Singer_Songs
    {
        public Singer singer { get; set; }
        public List<Song> songs { get; set; }

        public List<Concert> concerts_upcoming { get; set; }
        public List<Concert> concerts_finished { get; set; }
    }

}