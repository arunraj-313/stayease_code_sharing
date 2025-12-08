using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayEasePG.Models
{
    public class PGDetailsViewModel
    {
        public int PGID { get; set; }
        public string PGName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        // Room Details
        public List<RoomDetailsModel> Rooms { get; set; }
        // Amenities
        public List<string> Amenities { get; set; }
        // Rules
        public PG_Rules Rules { get; set; }
    }
}