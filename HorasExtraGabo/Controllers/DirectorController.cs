using HorasExtraGabo.Models;
using Sitios.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitios.Controllers
{
    public class DirectorController : Controller
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

        public ActionResult DaAprobacion()
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'MostrarDA','','','','','PendienteDA','','','',''";
            dr = com.ExecuteReader();
            int i = 0, j;
            j = dr.VisibleFieldCount;
            string[,] dato = new string[j, 8];

            /*  while (dr.Read())
              {
                  dato[i, 0] = dr.GetString(0).ToString();
                  dato[i, 1] = dr.GetString(1);
                  dato[i, 2] = dr.GetString(4).ToString();
                  dato[i, 3] = dr.GetString(5);
                  dato[i, 4] = dr.GetInt32(8).ToString();
                  dato[i, 5] = dr.GetString(9).ToString();
                  dato[i, 6] = dr.GetInt32(3).ToString();
                  dato[i, 7] = dr.GetString(6);
                  dato[i, 8] = dr.GetInt32(2).ToString();
                  dato[i, 9] = dr.GetString(10);
                  i++;
              }*/

            while (dr.Read())
            {
                dato[i, 0] = dr.GetString(0).ToString();
                dato[i, 1] = dr.GetString(1);
                dato[i, 2] = dr.GetString(2).ToString();
                dato[i, 3] = dr.GetString(3);
                dato[i, 4] = dr.GetInt32(4).ToString();
                dato[i, 5] = dr.GetString(5).ToString();
                dato[i, 6] = dr.GetString(6).ToString();
                dato[i, 7] = dr.GetString(7);
                /*dato[i, 8] = dr.GetInt32(2).ToString();
                dato[i, 9] = dr.GetString(10);*/
                i++;
            }

            ViewBag.datos = dato;
            return View();
        }

        [HttpPost]
        public ActionResult AprobarHoras(Director da)
        {
            Bitacora B1 = new Bitacora();
            Correos c1 = new Correos();
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'AprobarDA','" + da.idProc + "','','" + da.formulario + "','','PendienteDA','" + da.mes + "','" + da.quincena + "','','" + da.nombre + "'";
            dr = com.ExecuteReader();

            B1.usarBitacora(Session["usuario"].ToString(), "El director administrativo aprobó las horas para la " + da.quincena + "del mes");
            c1.EnviarCorreo(da.idFuncionario, "Aprobación jornadas extras", "El director administrativo ha aprobado su pago para la " + da.quincena + " del mes " + da.mes);
            return View("~/Views/Director/DaAprobacion.cshtml");
        }

        public ActionResult bitacora()
        {
            Bitacora B1 = new Bitacora();
            ViewData["listaBitacora"] = B1.retornarListaBitacora();
            return View();
        }

        public ActionResult jornada(string idJornada)
        {
            Bitacora B1 = new Bitacora();
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'jornada','" + idJornada + "','','','','','','','',''";
            dr = com.ExecuteReader();
            string[] dato = new string[6];

            while (dr.Read())
            {
                dato[0] = dr.GetInt32(0).ToString();
                dato[1] = dr.GetString(1);
                dato[2] = dr.GetString(2);
                dato[3] = dr.GetString(3).ToString();
                dato[4] = dr.GetString(4).ToString();
                dato[5] = dr.GetInt32(5).ToString();

            }
            ViewBag.datos = dato;
            return View("~/Views/Tiempo/TeAprobacion.cshtml");
        }


    }
}