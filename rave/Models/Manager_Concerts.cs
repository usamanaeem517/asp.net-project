using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rave.Models
{
    public class Manager_Concerts
    {
        public Manager manager { get; set; }
        public List<Concert> concerts_upcoming { get; set; }
        public List<Concert> concerts_finished { get; set; }
    }
}