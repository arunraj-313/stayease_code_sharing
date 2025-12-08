using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using StayEasePG.Models;

namespace StayEasePG.Controllers
{
    public class RoomController : Controller
    {
        private string cs = ConfigurationManager.ConnectionStrings["StayEasePGConn"].ConnectionString;
        // ------------------ ADD ROOM (GET) ------------------
        [HttpGet]
        public ActionResult AddRoom()
        {
            ViewBag.Amenities = GetAmenities();
            return View();
        }
        // ------------------ ADD ROOM (POST) ------------------
        [HttpPost]
        public ActionResult AddRoom(RoomDetails model, int[] SelectedAmenities)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                       INSERT INTO RoomDetails
                       (PGID, RoomType, TotalRooms, AvailableRooms, PricePerDay, PricePerWeek, PricePerMonth, MaintenanceCharges, AdvanceAmount, DepositAmount)
                       VALUES
                       (@pgid, @type, @total, @available, @day, @week, @month, @maintenance, @advance, @deposit);
                       SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@pgid", model.PGID);
                    cmd.Parameters.AddWithValue("@type", model.RoomType);
                    cmd.Parameters.AddWithValue("@total", model.TotalRooms);
                    cmd.Parameters.AddWithValue("@available", model.AvailableRooms);
                    cmd.Parameters.AddWithValue("@day", model.PricePerDay);
                    cmd.Parameters.AddWithValue("@week", model.PricePerWeek);
                    cmd.Parameters.AddWithValue("@month", model.PricePerMonth);
                    cmd.Parameters.AddWithValue("@maintenance", model.MaintenanceCharges ?? 0);
                    cmd.Parameters.AddWithValue("@advance", model.AdvanceAmount ?? 0);
                    cmd.Parameters.AddWithValue("@deposit", model.DepositAmount ?? 0);
                    con.Open();
                    int roomID = Convert.ToInt32(cmd.ExecuteScalar());
                    // Insert amenities
                    if (SelectedAmenities != null)
                    {
                        foreach (var aid in SelectedAmenities)
                        {
                            SqlCommand cmdAmenity = new SqlCommand(
                                "INSERT INTO RoomAmenities (RoomID, AmenityID) VALUES (@roomid, @aid)", con);
                            cmdAmenity.Parameters.AddWithValue("@roomid", roomID);
                            cmdAmenity.Parameters.AddWithValue("@aid", aid);
                            cmdAmenity.ExecuteNonQuery();
                        }
                    }
                }
                TempData["msg"] = "Room added successfully!";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
            }
            return RedirectToAction("RoomList");
        }
        // ------------------ SHOW ROOM LIST ------------------
        public ActionResult RoomList()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                   SELECT r.*,
                       STUFF((SELECT ',' + a.AmenityName
                              FROM RoomAmenities ra
                              JOIN Amenities a ON ra.AmenityID = a.AmenityID
                              WHERE ra.RoomID = r.RoomID
                              FOR XML PATH('')), 1, 1, '') AS Amenities
                   FROM RoomDetails r";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.Fill(dt);
            }
            return View(dt);
        }
        // ------------------ EDIT ROOM (GET) ------------------
        [HttpGet]
        public ActionResult EditRoom(int id)
        {
            RoomDetails model = new RoomDetails();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM RoomDetails WHERE RoomID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    model.RoomID = Convert.ToInt32(dr["RoomID"]);
                    model.PGID = Convert.ToInt32(dr["PGID"]);
                    model.RoomType = dr["RoomType"].ToString();
                    model.TotalRooms = Convert.ToInt32(dr["TotalRooms"]);
                    model.AvailableRooms = Convert.ToInt32(dr["AvailableRooms"]);
                    model.PricePerDay = Convert.ToDecimal(dr["PricePerDay"]);
                    model.PricePerWeek = Convert.ToDecimal(dr["PricePerWeek"]);
                    model.PricePerMonth = Convert.ToDecimal(dr["PricePerMonth"]);
                    model.MaintenanceCharges = dr["MaintenanceCharges"] != DBNull.Value ? Convert.ToDecimal(dr["MaintenanceCharges"]) : (decimal?)null;
                    model.AdvanceAmount = dr["AdvanceAmount"] != DBNull.Value ? Convert.ToDecimal(dr["AdvanceAmount"]) : (decimal?)null;
                    model.DepositAmount = dr["DepositAmount"] != DBNull.Value ? Convert.ToDecimal(dr["DepositAmount"]) : (decimal?)null;
                }
            }
            model.SelectedAmenities = GetSelectedAmenities(model.RoomID);
            ViewBag.Amenities = GetAmenities();
            return View(model);
        }
        // ------------------ EDIT ROOM (POST) ------------------
        [HttpPost]
        public ActionResult EditRoom(RoomDetails model, int[] SelectedAmenities)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = @"
                       UPDATE RoomDetails SET
                           PGID=@pgid,
                           RoomType=@type,
                           TotalRooms=@total,
                           AvailableRooms=@available,
                           PricePerDay=@day,
                           PricePerWeek=@week,
                           PricePerMonth=@month,
                           MaintenanceCharges=@maintenance,
                           AdvanceAmount=@advance,
                           DepositAmount=@deposit
                       WHERE RoomID=@id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@pgid", model.PGID);
                    cmd.Parameters.AddWithValue("@type", model.RoomType);
                    cmd.Parameters.AddWithValue("@total", model.TotalRooms);
                    cmd.Parameters.AddWithValue("@available", model.AvailableRooms);
                    cmd.Parameters.AddWithValue("@day", model.PricePerDay);
                    cmd.Parameters.AddWithValue("@week", model.PricePerWeek);
                    cmd.Parameters.AddWithValue("@month", model.PricePerMonth);
                    cmd.Parameters.AddWithValue("@maintenance", model.MaintenanceCharges ?? 0);
                    cmd.Parameters.AddWithValue("@advance", model.AdvanceAmount ?? 0);
                    cmd.Parameters.AddWithValue("@deposit", model.DepositAmount ?? 0);
                    cmd.Parameters.AddWithValue("@id", model.RoomID);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    // Update amenities
                    SqlCommand delCmd = new SqlCommand("DELETE FROM RoomAmenities WHERE RoomID=@id", con);
                    delCmd.Parameters.AddWithValue("@id", model.RoomID);
                    delCmd.ExecuteNonQuery();
                    if (SelectedAmenities != null)
                    {
                        foreach (var aid in SelectedAmenities)
                        {
                            SqlCommand cmdAmenity = new SqlCommand(
                                "INSERT INTO RoomAmenities (RoomID, AmenityID) VALUES (@roomid, @aid)", con);
                            cmdAmenity.Parameters.AddWithValue("@roomid", model.RoomID);
                            cmdAmenity.Parameters.AddWithValue("@aid", aid);
                            cmdAmenity.ExecuteNonQuery();
                        }
                    }
                }
                TempData["msg"] = "Room updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
            }
            return RedirectToAction("RoomList");
        }
        // ------------------ DELETE ROOM ------------------
        public ActionResult DeleteRoom(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    con.Open();
                    SqlCommand delAmenity = new SqlCommand("DELETE FROM RoomAmenities WHERE RoomID=@id", con);
                    delAmenity.Parameters.AddWithValue("@id", id);
                    delAmenity.ExecuteNonQuery();
                    SqlCommand delRoom = new SqlCommand("DELETE FROM RoomDetails WHERE RoomID=@id", con);
                    delRoom.Parameters.AddWithValue("@id", id);
                    delRoom.ExecuteNonQuery();
                }
                TempData["msg"] = "Room deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error: " + ex.Message;
            }
            return RedirectToAction("RoomList");
        }
        // ------------------ HELPERS ------------------
        private List<Amenity> GetAmenities()
        {
            List<Amenity> list = new List<Amenity>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Amenities";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Amenity
                    {
                        AmenityID = Convert.ToInt32(dr["AmenityID"]),
                        AmenityName = dr["AmenityName"].ToString()
                    });
                }
            }
            return list;
        }
        private List<int> GetSelectedAmenities(int roomId)
        {
            List<int> list = new List<int>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT AmenityID FROM RoomAmenities WHERE RoomID=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", roomId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(Convert.ToInt32(dr["AmenityID"]));
                }
            }
            return list;
        }
    }
}