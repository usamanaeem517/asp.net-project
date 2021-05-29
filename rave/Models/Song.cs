using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rave.Models
{
    public class Song
    {
        public string ID { get; set; }
        public string album_name { get; set; }
        public string song_name { get; set; }
        public string singer_name { get; set; }
        public string duration { get; set; }
        public string release_date { get; set; }
        public string genre { get; set; }

        public string v_link { get; set; }
        public string rating { get; set; }

        public string toJSON() 
        { 
            
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}