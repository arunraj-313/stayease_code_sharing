using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StayEasePG.Models
{
    public class BookingViewModel
    {
        public int PGID { get; set; }
        public int RoomID { get; set; }
        public string PGName { get; set; }
        public string RoomType { get; set; }
        public DateTime CheckInDate { get; set; }
        public string BookingType { get; set; }  // Day, Week, Month
        public int Duration { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string BookingStatus { get; set; }
    }
}