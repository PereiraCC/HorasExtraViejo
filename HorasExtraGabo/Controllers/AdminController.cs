using Sitios.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HorasExtraGabo.Controllers
{
    public class AdminController : Controller
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        void Conexionstring()
        {
            con.ConnectionString = "Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
            //con.ConnectionString = "Data Source = localhost; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
            //con.ConnectionString = "Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;";

        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string user, string password)
        {
            string rol;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec LogUsuario '" + user + "', '" + password + "' ";
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                Session["Rol"] = dr.GetString(4);
                rol = dr.GetString(4);
                if (rol == "Control Tiempo Extraordinario")
                {
                    Response.Redirect("~/Admin/Config");
                    return View("~/Views/Admin/Config.cshtml");
                }
                else
                {
                    ViewData["err"] = "Usuario no tiene permisos para ingresar";
                    return View();
                }
            }
            ViewData["err"] = "Usuario no registrado";
            return View();
        }

        public ActionResult Config()
        {
            string rol;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminConf 'nuevos','', '','','','',''";
            dr = com.ExecuteReader();

            int i = 0, j;
            j = dr.VisibleFieldCount;
            string[,] dato = new string[1, 8];

            while (dr.Read())
            {
                dato[i, 0] = dr.GetString(0).ToString();
                dato[i, 1] = dr.GetString(1);
                dato[i, 2] = dr.GetInt32(2).ToString();
                dato[i, 3] = dr.GetString(3);
                dato[i, 4] = dr.GetString(4).ToString();
                dato[i, 5] = dr.GetString(5).ToString();
                dato[i, 6] = dr.GetString(6).ToString();
                //    dato[i, 7] = dr.GetString(7);
                dato[i, 7] = dr.GetSqlMoney(8).ToString();
                i++;
            }
            ViewBag.datos = dato;
            con.Close();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminConf 'feriados','', '','','','',''";
            dr = com.ExecuteReader();
            string[,] datoF = new string[12, 3];
            i = 0;
            while (dr.Read())
            {
                datoF[i, 0] = dr.GetInt32(0).ToString();
                datoF[i, 1] = dr.GetString(1);
                datoF[i, 2] = dr.GetString(2);
                i++;
            }
            ViewBag.datoF = datoF;

            con.Close();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec fechaCorte 'select',''";
            dr = com.ExecuteReader();
            string fecha = "";
            while (dr.Read())
            {
                fecha = dr.GetInt32(0).ToString();
            }
            ViewBag.fechaC = fecha;

            return View();
        }

        public ActionResult registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult registro(string nombre, string idUser, string pass, string correo)
        {
            string rol;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminConf 'registro','" + nombre + "', '" + correo + "','" + pass + "',' " + idUser + "','',''";
            dr = com.ExecuteReader();
            ViewData["msg"] = "Usuario registrado correctamente";
            return View();
        }

        [HttpPost]
        public ActionResult actualizar(string departamento, string idUser, string Rol)
        {
            string rol;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminConf 'actUser','','','','" + idUser + "','" + departamento + "','" + Rol + "'";
            dr = com.ExecuteReader();
            ViewData["msg"] = "Usuario registrado correctamente";
            return View("Config");
        }

        public ActionResult Usuarios()
        {
            int i = 0, j = 0;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminUsuarios 'total','','','',''";
            dr = com.ExecuteReader();

            while (dr.Read())
            {
                j = dr.GetInt32(0);
            }
            con.Close();

            string rol;
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminUsuarios 'usuarios','','','',''";
            dr = com.ExecuteReader();
            string[,] dato = new string[j, 8];

            while (dr.Read())
            {
                dato[i, 0] = dr.GetString(0).ToString();
                dato[i, 1] = dr.GetString(1);
                dato[i, 2] = dr.GetString(2).ToString();
                dato[i, 3] = dr.GetString(3);
                dato[i, 4] = dr.GetString(4).ToString();
                i++;
            }
            ViewBag.j = j;
            ViewBag.datos = dato;
            con.Close();
            return View();
        }

        [HttpPost]
        public ActionResult editar(string nombre, string correo, string rol, string jefe, string idU)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec adminUsuarios 'editar','" + idU + "','" + nombre + "','" + correo + "','" + rol + "'";
            dr = com.ExecuteReader();

            return View("~/Views/Admin/Usuarios.cshtml");
        }

        [HttpPost]
        public ActionResult editarFecha(string fecha)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec fechaCorte 'edit','" + fecha + "'";
            dr = com.ExecuteReader();

            return View("~/Views/Admin/Config.cshtml");
        }

        [HttpPost]
        public ActionResult actualizarFeriado(string feriado, string idFeriado)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec feriado '" + feriado + "','" + idFeriado + "'";
            dr = com.ExecuteReader();

            return View("~/Views/Admin/Config.cshtml");
        }

    }
}