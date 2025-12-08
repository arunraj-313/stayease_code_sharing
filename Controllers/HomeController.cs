using StayEasePG.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StayEasePG.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["StayEasePGConn"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT PGID, PGName, Address, City FROM PG", con);
                SqlDataReader dr = cmd.ExecuteReader();
                List<PG> pgList = new List<PG>();
                while (dr.Read())
                {
                    pgList.Add(new PG
                    {
                        PGID = Convert.ToInt32(dr["PGID"]),
                        PGName = dr["PGName"].ToString(),
                        Address = dr["Address"].ToString(),
                        City = dr["City"].ToString()
                    });
                }
                return View(pgList);   // ✔ SEND MODEL
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}