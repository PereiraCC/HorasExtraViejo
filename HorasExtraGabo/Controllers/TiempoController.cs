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
    public class TiempoController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        void Conexionstring()
        {
            //con.ConnectionString = "Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
            //con.ConnectionString = "server=25.84.109.208 ; database=HorasExtra ; User ID= BancoHabana; Password= progra5";
            con.ConnectionString = "Server = A2NWPLSK14SQL-v03.shr.prod.iad2.secureserver.net; Database = ph179300922261_horasextra; User Id = horasextra; Password = Datos.2020;";

        }

        public ActionResult Pendientes()
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'MostrarCte','','','','','PendienteCTE','','','',''";
            dr = com.ExecuteReader();
            int i = 0, j;
            j = dr.VisibleFieldCount;
            string[,] dato = new string[j, 11];

            while (dr.Read())
            {
                dato[i, 0] = dr.GetInt32(0).ToString();
                dato[i, 1] = dr.GetString(1).ToString();
                dato[i, 2] = dr.GetString(2);
                dato[i, 3] = dr.GetString(3);
                dato[i, 4] = dr.GetString(4).ToString();
                dato[i, 5] = dr.GetString(5);
                dato[i, 6] = dr.GetString(6).ToString();
                dato[i, 7] = dr.GetInt32(7).ToString();
                dato[i, 8] = dr.GetString(8).ToString();
                dato[i, 9] = dr.GetString(9).ToString();
                dato[i, 10] = dr.GetInt32(10).ToString();
                i++;
            }
            ViewBag.datos = dato;
            return View();
        }


        [HttpGet]
        public ActionResult TeAprobacion(string idJornada, string opc, string fecha, string nombre)
        {
            if (opc == "cte")
            {
                Conexionstring();
                con.Open();
                com.Connection = con;
                com.CommandText = "exec ProcesoPagos 'MostrarForm','" + idJornada + "','','','','','','','',''";
                dr = com.ExecuteReader();
                int i = 0, j;
                j = dr.VisibleFieldCount;
                string[,] dato = new string[j, 6];
                while (dr.Read())
                {
                    dato[i, 0] = dr.GetString(0).ToString();
                    dato[i, 1] = dr.GetString(1);
                    dato[i, 2] = dr.GetInt32(2).ToString();
                    dato[i, 3] = dr.GetString(3).ToString();
                    dato[i, 4] = dr.GetString(4).ToString();
                   // dato[i, 5] = dr.GetString(5).ToString();
                    i++;
                }
                ViewBag.datos = dato;
                con.Close();
                return View("~/Views/Tiempo/TeAprobacion.cshtml");
            }

            if (opc == "da")
            {
                Conexionstring();
                con.Open();
                com.Connection = con;
                com.CommandText = "exec ProcesoPagos 'jornada','" + idJornada + "','','" + fecha + "','','','','','','" + nombre + "'";
                dr = com.ExecuteReader();
                int i = 0, j;
                j = dr.VisibleFieldCount;
                string[,] dato = new string[j, 6];
                while (dr.Read())
                {
                    dato[i, 0] = dr.GetString(0).ToString();
                    dato[i, 1] = dr.GetString(1);
                    dato[i, 2] = dr.GetInt32(2).ToString();
                    dato[i, 3] = dr.GetString(3).ToString();
                    dato[i, 4] = dr.GetString(4).ToString();
                    dato[i, 5] = dr.GetString(5).ToString();
                    i++;
                }
                ViewBag.datos = dato;
                con.Close();
                return View("~/Views/Tiempo/TeAprobacion.cshtml");

            }

            return View();
        }

        [HttpGet]
        public ActionResult TeAprobacion2(string idJornada, string opc, string fecha, string nombre)
        {
            if (opc == "cte")
            {
                Conexionstring();
                con.Open();
                com.Connection = con;
                com.CommandText = "exec ProcesoPagos 'MostrarForm','" + idJornada + "','','','','','','','',''";
                dr = com.ExecuteReader();
                string[] dato = new string[6];
                while (dr.Read())
                {
                    dato[0] = dr.GetString(0).ToString();
                    dato[1] = dr.GetString(1);
                    dato[2] = dr.GetInt32(2).ToString();
                    dato[3] = dr.GetString(3).ToString();
                    dato[4] = dr.GetString(4).ToString();
                }
                ViewBag.datos = dato;
                con.Close();
                return View("~/Views/Tiempo/TeAprobacion2.cshtml");
            }

            if (opc == "da")
            {
                Conexionstring();
                con.Open();
                com.Connection = con;
                com.CommandText = "exec ProcesoPagos 'jornada','" + idJornada + "','','" + fecha + "','','','','','','" + nombre + "'";
                dr = com.ExecuteReader();
                int i = 0, j;
                j = dr.VisibleFieldCount;
                string[,] dato = new string[j, 6];
                while (dr.Read())
                {
                    dato[i, 0] = dr.GetString(0).ToString();
                    dato[i, 1] = dr.GetString(1);
                    dato[i, 2] = dr.GetInt32(2).ToString();
                    dato[i, 3] = dr.GetString(3).ToString();
                    dato[i, 4] = dr.GetString(4).ToString();
                    dato[i, 5] = dr.GetString(5).ToString();
                    i++;
                }
                ViewBag.datos = dato;
                con.Close();
                return View("~/Views/Tiempo/TeAprobacion.cshtml");

            }

            return View();
        }

        [HttpPost]
        public ActionResult TeAprobaciona(string idProc, string tipoP, string hora)
        {
            string idFun = "", fecha = "", quincena = "", mes = "";
            Bitacora B1 = new Bitacora();
            Correos c1 = new Correos();
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'AprobarCTE','" + idProc + "','','','" + tipoP + "','Pagado','','','',''";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                idFun = dr.GetString(0).ToString();
                fecha = dr.GetString(1).ToString();
                quincena = dr.GetString(2).ToString();
                mes = dr.GetString(3).ToString();
            }
            B1.usarBitacora(Session["usuario"].ToString(), "El encargado de control de tiempos ha aprobado el pago como un tipo de pago " + tipoP + " para el proceso numero" + idProc);
            c1.EnviarCorreo(idFun, "Aprobación Tipo de Pago", "Se ha aprobado tu pago correspondiente a la fecha " + fecha + " como tiempo " + tipoP
                + ", el pago se realizará en " + quincena + " del mes de " + mes);
            return View("~/Views/Tiempo/Pendientes.cshtml");

        }

        [HttpPost]
        public ActionResult FormAprobar(Tiempos tp)
        {
            ViewData["idProc"] = tp.idProc;
            ViewData["Usuario"] = tp.nombre;
            ViewData["Formulario"] = tp.nombre;
            ViewData["Reporte"] = tp.nombre;
            ViewData["Horas"] = tp.hora;
            return View("~/Views/Tiempo/TeAprobacion.cshtml");

        }

        [HttpPost]
        public ActionResult TiempoPago(string fechaV, string idProc2, string idFuncion)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'TipoPago','" + idProc2 + "','','','','Pagado','','','" + idFuncion + "','" + fechaV + "'";
            dr = com.ExecuteReader();


            while (dr.Read())
            {
                Session["idProc"] = dr.GetInt32(0).ToString();
                Session["fecha"] = dr.GetString(1).ToString();
                //   dato[i, 2] = dr.GetString(2);
                //   dato[i, 3] = dr.GetString(3);
                //   dato[i, 4] = dr.GetString(4).ToString();
                //   dato[i, 5] = dr.GetString(5);
                Session["nombre"] = dr.GetString(6).ToString();
                Session["Prub"] = dr.GetString(7).ToString();
                Session["horas"] = dr.GetInt32(8).ToString();
                Session["reporte"] = dr.GetString(9).ToString();
            }

            Response.Redirect("~/Tiempo/Pendientes#openmodal");
            return View();
        }

        [HttpPost]
        public ActionResult rechazar(string idproce2, string txRech)
        {
            string idFun = "", fecha = "";
            Bitacora B1 = new Bitacora();
            Correos c1 = new Correos();
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec rechazarProceso " + idproce2 + " ";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                idFun = dr.GetString(0).ToString();
                fecha = dr.GetString(1).ToString();
            }
            B1.usarBitacora(Session["usuario"].ToString(), "El encargado de control de tiempos ha rechazado el pago del proceso # " + idproce2 + " debido a " + txRech);
            c1.EnviarCorreo(idFun, "Rechazo de Pago", "Estimad@, le informamos que su pago correspondiente a la fecha " + fecha + " fue rechazado debido a " + txRech + " <br/> Favor comunicarse con su jefe inmediato para encontrar solución <br/>Muchas gracias");
            return View("Pendientes");
        }

    }
}