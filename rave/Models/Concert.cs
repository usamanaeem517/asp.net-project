using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rave.Models
{
    public class Concert
    {
        public string ID { get; set; }
        public string name { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string venue { get; set; }
        public string price { get; set; }
        public string seatsleft { get; set; }
        public string seatstaken { get; set; }
        public string location_link { get; set; }

        public string toJSON()
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}