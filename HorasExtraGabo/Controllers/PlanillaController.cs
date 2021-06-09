using HorasExtraGabo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitios.Controllers
{
    public class PlanillaController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        void Conexionstring()
        {
            con.ConnectionString = "Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
            //con.ConnectionString = "server=25.84.109.208 ; database=HorasExtra ; User ID= BancoHabana; Password= progra5";
            //con.ConnectionString = "Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;";
        }
        // GET: Planilla
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult planilla()
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec informesPlanillas '','',''";
            dr = com.ExecuteReader();
            int i = 0, j;
            j = dr.VisibleFieldCount;
            string[,] dato = new string[j, 5];

            while (dr.Read())
            {
                dato[i, 0] = dr.GetString(0).ToString();
                dato[i, 1] = dr.GetString(1);
                dato[i, 2] = dr.GetString(2).ToString();
                dato[i, 3] = dr.GetInt32(3).ToString();
                dato[i, 4] = dr.GetString(4).ToString();
                i++;
            }
            ViewBag.datos = dato;
            return View();
        }

        [HttpPost]
        public ActionResult planillaS(Planillas p)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec informesPlanillas '" + p.fechaInicio + "','" + p.fechaFinal + "','" + p.departamento + "'";
            dr = com.ExecuteReader();
            int i = 0, j;
            j = dr.VisibleFieldCount;
            string[,] dato = new string[j, 5];

            while (dr.Read())
            {
                dato[i, 0] = dr.GetString(0).ToString();
                dato[i, 1] = dr.GetString(1);
                dato[i, 2] = dr.GetString(2).ToString();
                dato[i, 3] = dr.GetInt32(3).ToString();
                dato[i, 4] = dr.GetString(4).ToString();
                i++;
            }
            ViewBag.datos = dato;
            return View("~/Views/Planilla/Planilla.cshtml");
        }

    }
}