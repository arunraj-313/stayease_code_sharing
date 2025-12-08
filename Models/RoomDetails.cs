using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StayEasePG.Models
{
    public class RoomDetails
    {
        [Key]
        public int RoomID { get; set; }
        [Required]
        [Display(Name = "PG")]
        public int PGID { get; set; } // Foreign key to PG table
        [Required]
        [Display(Name = "Room Type")]
        public string RoomType { get; set; } // 1 Sharing, 2 Sharing, etc.
        [Required]
        [Display(Name = "Total Rooms")]
        public int TotalRooms { get; set; }
        [Required]
        [Display(Name = "Available Rooms")]
        public int AvailableRooms { get; set; }
        [Required]
        [Display(Name = "Price per Day")]
        public decimal PricePerDay { get; set; }
        [Required]
        [Display(Name = "Price per Week")]
        public decimal PricePerWeek { get; set; }
        [Required]
        [Display(Name = "Price per Month")]
        public decimal PricePerMonth { get; set; }
        [Display(Name = "Maintenance Charges")]
        public decimal? MaintenanceCharges { get; set; }
        [Display(Name = "Advance Amount")]
        public decimal? AdvanceAmount { get; set; }
        [Display(Name = "Deposit Amount")]
        public decimal? DepositAmount { get; set; }
        // For selected amenities in Add/Edit views
        [Display(Name = "Amenities")]
        public List<int> SelectedAmenities { get; set; } = new List<int>();
    }

    public class Amenity
    {
        public int AmenityID { get; set; }
        public string AmenityName { get; set; }
    }
    
}
