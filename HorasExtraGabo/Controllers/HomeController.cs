using HorasExtraGabo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HorasExtraGabo.Controllers
{
    
    public class HomeController : Controller
    {
        #region GABRIEL
        private static List<Persona> listaFuncionariosGlobal = new List<Persona>();
        private static List<SolicitudHorasExtra> solicitudesPendGlobal = new List<SolicitudHorasExtra>();
        private static List<SolicitudHorasExtra> solicitudesAprob = new List<SolicitudHorasExtra>();
        private static List<SolicitudHorasExtra> solicitudesRech = new List<SolicitudHorasExtra>();
        private static List<String> Actividades = new List<String>();
        private static int cantidadActividades = 0;
        private static int cantidadHoras = 0;

        public ActionResult bandeja_solicitudes()
        {
            SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
            ViewData["listaSolicitudesPendientes"] = SHE1.retornarSolicitudesPendientes(Session["usuario"].ToString());
            return View();
        }

        public ActionResult solicitud_jefatura()
        {
            if (Session["usuario"] != null)
            {
                Persona P2 = new Persona();
                ViewData["listaFuncionarios"] = P2.retornarListaUsuarios(Session["usuario"].ToString());
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
                ViewData["listaSolicitudes"] = SHE1.retornarSolicitudes(Session["usuario"].ToString());
                ViewData["listaSolicitudesRechazadas"] = SHE1.retornarSolicitudesRechazadas(Session["usuario"].ToString());
                ViewData["listaSolicitudesAceptadas"] = SHE1.retornarSolicitudesAprobadas(Session["usuario"].ToString());
                return View();
            }
            else
            {
                ViewData["listaFuncionarios"] = listaFuncionariosGlobal;
                ViewData["listaSolicitudes"] = solicitudesPendGlobal;
                ViewData["listaSolicitudesRechazadas"] = solicitudesRech;
                ViewData["listaSolicitudesAceptadas"] = solicitudesAprob;
                return View();
            }

        }

        public ActionResult evidencias_jefatura()
        {
            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            Evidencias E1 = new Evidencias();
            ViewData["listaEvidencias"] = E1.retornarListaEvidenciasJefatura(Session["usuario"].ToString());
            ViewData["evidenciasRechazadas"] = E1.retornarListaEvidenciasJefaturaRechazadas(Session["usuario"].ToString());
            ViewData["evidenciasAprobadas"] = E1.retornarListaEvidenciasJefaturaAprobadas(Session["usuario"].ToString());
            return View();
        }

        public ActionResult comparaciones()
        {
            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            ReportesAsistencia RA1 = new ReportesAsistencia();
            ViewData["listaReportes"] = RA1.retornarListaReportes(Session["usuario"].ToString());
            FormulariosHorasExtra FHE1 = new FormulariosHorasExtra();
            ViewData["listaFormularios"] = FHE1.retornarListaFormularios(Session["usuario"].ToString());
            Evidencias E1 = new Evidencias();
            ViewData["listaEvidencias"] = E1.retornarListaEvidenciasJefaturaAprobadas(Session["usuario"].ToString());
            JornadasHorasExtra JHE1 = new JornadasHorasExtra();
            ViewData["listaJornadas"] = JHE1.retornarListaJornadas(Session["usuario"].ToString());
            return View();
        }

        public ActionResult bitacora()
        {
            Bitacora B1 = new Bitacora();
            ViewData["listaBitacora"] = B1.retornarListaBitacora();
            return View();
        }

        public ActionResult agregarActividad(string act)
        {
            if (act == "")
            {
                ViewData["txtActividad"] = "Ingrese una actividad...";
            }
            else
            {
                Actividades.Add(act);
                ViewData["txtActividad"] = "Actividad" + " '" + Actividades[cantidadActividades] + "' " + "agregada!";
                ViewData["hayActividades"] = "verde";
                cantidadActividades++;
            }
            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
            ViewData["listaSolicitudes"] = SHE1.retornarSolicitudes(Session["usuario"].ToString());
            ViewData["listaSolicitudesRechazadas"] = SHE1.retornarSolicitudesAprobadas(Session["usuario"].ToString());
            ViewData["listaSolicitudesAceptadas"] = SHE1.retornarSolicitudesRechazadas(Session["usuario"].ToString());
            return View("solicitud_jefatura");
        }

        public ActionResult agregarSolicitudJefatura(string idUsuario, DateTime FechaInicio, DateTime FechaFin, int HorasAprox, string TareaPrincipal)
        {
            ViewData["hayActividades"] = "";
            if (Actividades.Count > 0)
            {
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra
                {
                    Actividades = Actividades,
                    idUsuario = idUsuario,
                    fechaInicio = FechaInicio.ToString(),
                    fechaLimite = FechaFin.ToString(),
                    HorasAprox = HorasAprox,
                    TareaPrincipal = TareaPrincipal
                };

                if (SHE1.agregarSolicitud(Session["usuario"].ToString()))
                {
                    ViewData["haySolicitud"] = "verde";
                    ViewData["resultadoSolicitudJefatura"] = "Solicitud enviada!";
                }
                else
                {
                    ViewData["haySolicitud"] = "rojo";
                    ViewData["resultadoSolicitudJefatura"] = "Hubo un problema al agregar la solicitud...";
                }
            }
            else
            {
                ViewData["hayActividades"] = "rojo";
                ViewData["txtActividad"] = "Debe agregar al menos una actividad...";
            }
            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            SolicitudHorasExtra SHE2 = new SolicitudHorasExtra();
            ViewData["listaSolicitudes"] = SHE2.retornarSolicitudes(Session["usuario"].ToString());
            ViewData["listaSolicitudesRechazadas"] = SHE2.retornarSolicitudesAprobadas(Session["usuario"].ToString());
            ViewData["listaSolicitudesAceptadas"] = SHE2.retornarSolicitudesRechazadas(Session["usuario"].ToString());
            return View("solicitud_jefatura");
        }

        public ActionResult responderSolicitud(string comboIDSolicitud, string comboAprobReprob, string motivoRechazo)
        {
            /*ViewData["hayErrorSolicitud"]*/

            SolicitudHorasExtra SHE2 = new SolicitudHorasExtra
            {
                idSolicitud = comboIDSolicitud,
                Estado = comboAprobReprob,
                Motivo = motivoRechazo
            };
            if (SHE2.responderSolicitudPendiente(Session["usuario"].ToString()))
            {
                if (comboAprobReprob == "Aprobado")
                {
                    ViewData["respuestaResponderSolicitud"] = "¡Solicitud aprobada correctamente!";
                }
                else if (comboAprobReprob == "Rechazado")
                {
                    ViewData["respuestaResponderSolicitud"] = "¡Solicitud rechazada correctamente!";
                }
                ViewData["hayErrorSolicitud"] = "verde";
            }
            else
            {
                ViewData["respuestaResponderSolicitud"] = "Hubo un error inesperado, inténtelo nuevamente...";
                ViewData["hayErrorSolicitud"] = "rojo";
            }
            SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
            ViewData["listaSolicitudesPendientes"] = SHE1.retornarSolicitudesPendientes(Session["usuario"].ToString());
            return View("bandeja_solicitudes");
        }

        public  ActionResult actualizarEstadoEvidencia(int idEvidencia, string Estado, string idUsuario)
        {
            Evidencias E1 = new Evidencias
            {
                idUsuario = idUsuario,
                Estado = Estado,
                idEvidencia = idEvidencia
            };

            if (E1.actualizarEstadoEvidencia(Session["usuario"].ToString()))
            {
                ViewData["resultadoEvidencia"] = "¡Estado actualizado!";
                ViewData["colorEvidencia"] = "verde";
            }
            else
            {
                ViewData["resultadoEvidencia"] = "Hubo un problema... intentelo nuevamente.";
                ViewData["colorEvidencia"] = "rojo";
            }

            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
            ViewData["listaSolicitudes"] = SHE1.retornarSolicitudes(Session["usuario"].ToString());
            ViewData["listaSolicitudesRechazadas"] = SHE1.retornarSolicitudesRechazadas(Session["usuario"].ToString());
            ViewData["listaSolicitudesAceptadas"] = SHE1.retornarSolicitudesAprobadas(Session["usuario"].ToString());
            Response.Redirect("~/Home/solicitud_jefatura");
            return View("~/Views/Home/solicitud_jefatura.cshtml");
        }

        public ActionResult consultaJornada(string idJ, string Fecha, string HoraEntrada, string HoraSalida, string HorasValidas, string idFormulario, string idUsuario)
        {
            string[] ListaJornada = new string[7];
            ListaJornada[0] = idJ;
            ListaJornada[1] = Fecha;
            ListaJornada[2] = HoraEntrada;
            ListaJornada[3] = HoraSalida;
            ListaJornada[4] = HorasValidas;
            ListaJornada[5] = idFormulario;
            ListaJornada[6] = idUsuario;
            ViewData["mostrarJornada"] = ListaJornada;

            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            ReportesAsistencia RA1 = new ReportesAsistencia();
            ViewData["listaReportes"] = RA1.retornarListaReportes(Session["usuario"].ToString());
            FormulariosHorasExtra FHE1 = new FormulariosHorasExtra();
            ViewData["listaFormularios"] = FHE1.retornarListaFormularios(Session["usuario"].ToString());
            Evidencias E1 = new Evidencias();
            ViewData["listaEvidencias"] = E1.retornarListaEvidenciasJefatura(Session["usuario"].ToString());
            JornadasHorasExtra JHE1 = new JornadasHorasExtra();
            ViewData["listaJornadas"] = JHE1.retornarListaJornadas(Session["usuario"].ToString());
            return View("comparaciones");
        }

        public ActionResult validarHorasJornada(string Fecha, string HoraInicio, string HoraFin, int HorasTrabajadas, int idFormulario, int idJornada, string idUsuario)
        {
            if (HorasTrabajadas > 5)
            {
                HorasTrabajadas = 5;
            }
            cantidadHoras += HorasTrabajadas;

            JornadasHorasExtra JHE2 = new JornadasHorasExtra
            {
                idUsuario = idUsuario,
                Fecha = Convert.ToDateTime(Fecha),
                HoraEntrada = HoraInicio,
                HoraSalida = HoraFin,
                HorasValidas = HorasTrabajadas,
                idJornada = idJornada,
                idFormularioHorasExtra = idFormulario
            };

            JHE2.validarJornada(Session["usuario"].ToString());

            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            ReportesAsistencia RA1 = new ReportesAsistencia();
            ViewData["listaReportes"] = RA1.retornarListaReportes(Session["usuario"].ToString());
            FormulariosHorasExtra FHE1 = new FormulariosHorasExtra();
            ViewData["listaFormularios"] = FHE1.retornarListaFormularios(Session["usuario"].ToString());
            Evidencias E1 = new Evidencias();
            ViewData["listaEvidencias"] = E1.retornarListaEvidenciasJefatura(Session["usuario"].ToString());
            JornadasHorasExtra JHE1 = new JornadasHorasExtra();
            ViewData["listaJornadas"] = JHE1.retornarListaJornadas(Session["usuario"].ToString());
            return View("comparaciones");
        }

        public ActionResult seleccionarEvidencia(string idEvidencia, string idUsuario)
        {
            Persona P1 = new Persona();
            ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
            Evidencias E1 = new Evidencias();
            ViewData["listaEvidencias"] = E1.retornarListaEvidenciasJefatura(Session["usuario"].ToString());
            ViewData["EvidenciaSeleccionada"] = idEvidencia;
            ViewData["UsuarioSeleccionado"] = idUsuario;
            ViewData["evidenciasRechazadas"] = E1.retornarListaEvidenciasJefaturaRechazadas(Session["usuario"].ToString());
            ViewData["evidenciasAprobadas"] = E1.retornarListaEvidenciasJefaturaAprobadas(Session["usuario"].ToString());
            return View("evidencias_jefatura");
        }
        #endregion

        #region ABNER
        /*CONTROLADOR ABNER*/
        public ActionResult funcionariostareas()
        {
            Conexion conn = new Conexion();
            F_Tareas f = new F_Tareas();
            try
            {
                var resultado = conn.pruebaProcAlmacenado(Session["usuario"].ToString(), "Jefatura");
                //
                ViewData["idsolicitud"] = resultado.idsolicitud;

                ViewData["idActividad"] = resultado.idactividad;
                //
                ViewData["fechainicio"] = resultado.fechainicio;
                ViewData["fechafin"] = resultado.fechafin;
                ViewData["horas"] = resultado.horas;

                ViewData["actividad"] = resultado.actividad;
                ViewData["tarea"] = resultado.tarea;


                var resultado2 = conn.CargarActividadesTareas(resultado.idsolicitud);
                ViewData["actividades"] = resultado2;
                //Hasta Aquí
                return View("funcionariostareas");
            }
            catch (Exception e)
            {
                Response.Write("<script language=javascript>alert('No tiene tareas pendientes');</script>");


            }
            return View("funcionariostareas");
        }

        ///total
        ///
        public ActionResult totalhoras()
        {
            Conexion conn = new Conexion();
            FormularioHorasExtra2 f = new FormularioHorasExtra2();
            try
            {
                var resultado = conn.CargarHoras(Session["usuario"].ToString());
                //
                ViewData["total"] = resultado.totalhoras;

                return View("FormulariosyReportes");
            }
            catch (Exception e)
            {
                Response.Write("<script language=javascript>alert('No tiene tareas pendientes');</script>");


            }
            return View("FormulariosyReportes");
        }
        ///

        /// Cargar formulariohorasextra

        public ActionResult FormulariosAvalados()
        {
            Conexion conn = new Conexion();

            try
            {
                var resultado = conn.CargarTodosFormularios(Session["usuario"].ToString());

                ViewData["formus"] = resultado;

                return View("FormulariosAvalados");
            }
            catch (Exception e)
            {
                Response.Write("<script language=javascript>alert('No tiene formularios a revisar');</script>");


            }
            return View("FormulariosAvalados");
        }
        //
        public ActionResult Evidencias()
        {
            Conexion conn = new Conexion();

            try
            {
                var resultado = conn.CargarActividades(Session["usuario"].ToString());
                //
                //ViewData["idAct"] = resultado.id_actividad;
                //ViewData["idSol"] = resultado.idsolicitud;
                //ViewData["actividad"] = resultado.actividad;
                ////
                ViewData["activi"] = resultado;

                return View("Evidencias");
            }
            catch (Exception e)
            {
                Response.Write("<script language=javascript>alert('No tiene Actividades que evidenciar');</script>");


            }
            return View("Evidencias");
        }
        //

        public ActionResult Horasextra()
        {
            return View("Horasextra");
        }



        [HttpPost]
        public ActionResult Evidencias(HttpPostedFileBase file)
        {
            Bitacora B1 = new Bitacora();
            Correos cor = new Correos();
            string Jefe = cor.retornarJefe(Session["usuario"].ToString());
            string nombreUsuario = cor.retornarNombre(Session["usuario"].ToString());
            SubirArchivo modelo = new SubirArchivo();

            if (file != null && file.ContentLength > 0)
            {
                string NombreArchivo = DateTime.Now.ToString().Replace(" ", "_").Replace("/", "_").Replace(":", "_") + "_" + Path.GetFileName(file.FileName);
                string ruta = Server.MapPath("..\\Evidencias") + "\\" + NombreArchivo;
                ViewData["rutaArchivo"] = "/Evidencias/" + NombreArchivo;   /*ESTO SE SUBE A BD*/
                modelo.subirarchivo(ruta, file);
                ViewBag.Error = modelo.error;
                ViewBag.Correcto = modelo.confirmacion;
                //Guardar en bd
                string iduser = Session["usuario"].ToString();
                string ruta1 = "/Evidencias/" + NombreArchivo;
                string idactividad = Request.Form["idactividad"].ToString();
                string horas = Request.Form["horas"].ToString();
                ///Correo
                cor.EnviarCorreo(Jefe, "Envio de evidencias", "Que tenga un excelente día, se le informa que el usuario " + nombreUsuario + " ha enviado evidencia de las actividades" +
                    " compruebe el sistema para más información");
                ///Correo arreglar
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario sube evidencia de la actividad  " + idactividad + " " + " que duró" + horas + " horas realizándola");

                Clase_Evidencias insertarevidencias = new Clase_Evidencias();
                if (insertarevidencias.agregarevidencia(iduser, ruta1, idactividad, horas) == false)
                {
                    Response.Write("<script language='javascript'>window.alert('Error al agregar evidencia');window.location='Evidencias';</script>");
                    return null;
                }
                //
            }
            return View("Evidencias");
        }

        //


        //
        //public ActionResult FormulariosAvalados()
        //{
        //    Bitacora B1 = new Bitacora();
        //    try
        //    {
        //        Conexion conn = new Conexion();
        //        FormularioHorasExtra f = new FormularioHorasExtra();
        //        int idFormulario = Convert.ToInt32(Request.Form["idFormulario"]);
        //        var resultado = conn.cargarestado(Session["usuario"].ToString());
        //        var estadoFormulario = conn.EstadoFormulario(idFormulario);
        //        ViewData["id"] = estadoFormulario.id_formulario;
        //        ViewData["EstadoFormulario"] = estadoFormulario.estado;
        //        ViewData["Motivo"] = estadoFormulario.motivo;
        //        B1.usarBitacora(Session["usuario"].ToString(), "El usuario revisa el estado del formulario el cual es " + estadoFormulario.estado);
        //        return View("FormulariosAvalados");
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return View("FormulariosAvalados");
        //}

        //Pruebas
        [HttpPost]
        public ActionResult AceptarTarea(F_Tareas f)
        {
            Correos Co1 = new Correos();
            string Jefe = Co1.retornarJefe(Session["usuario"].ToString());
            string nombreUsuario = Co1.retornarNombre(Session["usuario"].ToString());
            Bitacora B1 = new Bitacora();
            try
            {
                Conexionstring();
                con.Open();
                com.Connection = con;
                com.CommandText = "exec Sp_AceptarTareaFuncionario '" + f.idsolicitud + "'";
                com.ExecuteNonQuery();

                Co1.EnviarCorreo(Jefe, "Solicitud Respondida", "Que tenga un buen día, el(la) funcionario(a) " + nombreUsuario + " ha contestado la solicitud para realizar horas extra, para más información, ingrese al sistema.");
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario acepta la tarea asignada con solicitud " + f.idsolicitud);
                return View("funcionariostareas");


            }
            catch (Exception ex)
            {

            }
            return View("funcionariostareas");
        }

        //Actividades


        //Pruebas
        [HttpPost]
        public ActionResult NegarTarea(F_Tareas f)
        {
            Correos Co1 = new Correos();
            string Jefe = Co1.retornarJefe(Session["usuario"].ToString());
            string nombreUsuario = Co1.retornarNombre(Session["usuario"].ToString());
            Bitacora B1 = new Bitacora();
            try
            {
                Conexionstring();
                con.Open();
                com.Connection = con;

                com.CommandText = "exec Sp_NegarTareaFuncionario '" + f.idsolicitud + "'";

                com.ExecuteNonQuery();

                Co1.EnviarCorreo(Jefe, "Solicitud Respondida", "Que tenga un buen día, el(la) funcionario(a) " + nombreUsuario + " ha contestado la solicitud para realizar horas extra, para más información, ingrese al sistema.");
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario se niega a realizar la tarea asignada con solicitud " + f.idsolicitud);
                return View("funcionariostareas");

            }
            catch (Exception ex)
            {

            }
            return View("funcionariostareas");
        }
        //


        public ActionResult FormulariosyReportes(HttpPostedFileBase file)
        {
            Correos Co1 = new Correos();
            string Jefe = Co1.retornarJefe(Session["usuario"].ToString());
            string nombreUsuario = Co1.retornarNombre(Session["usuario"].ToString());
            Bitacora B1 = new Bitacora();
            SubirArchivo modelo = new SubirArchivo();
            if (file != null && file.ContentLength > 0)
            {
                string NombreArchivo = DateTime.Now.ToString().Replace(" ", "_").Replace("/", "_").Replace(":", "_") + "_" + Path.GetFileName(file.FileName);
                string ruta = Server.MapPath("..\\Evidencias") + "\\" + NombreArchivo;
                ViewData["rutaArchivo"] = "/Evidencias/" + NombreArchivo;   /*ESTO SE SUBE A BD*/
                modelo.subirarchivo(ruta, file);
                ViewBag.Error = modelo.error;
                ViewBag.Correcto = modelo.confirmacion;
                //
                string iduser = Session["usuario"].ToString();
                string ruta1 = "/Evidencias/" + NombreArchivo;
                Co1.EnviarCorreo(Jefe, "Nuevo reporte de asistencia", "Que tenga un buen día, le informamos que el usuario " + nombreUsuario + " ha agregado un reporte de asistencia, para más información, compruebe el sistema.");
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario sube el reporte de asistencia con el nombre " + NombreArchivo);
                Clase_Evidencias insertarevidencias = new Clase_Evidencias();
                if (insertarevidencias.agregarevidenreporte(iduser, ruta1) == false)
                {
                    Response.Write("<script language='javascript'>window.alert('Error al agregar el formulario');window.location='FormulariosyReportes';</script>");
                    return null;
                }
                //

                //
            }

            return View("FormulariosyReportes");
        }

        //insertar formulario
        [HttpPost]
        public ActionResult InsertarFormulario()
        {
            Correos Co1 = new Correos();
            string Jefe = Co1.retornarJefe(Session["usuario"].ToString());
            string nombreUsuario = Co1.retornarNombre(Session["usuario"].ToString());
            Bitacora B1 = new Bitacora();
            //recibir los datos desde la vista
            string iduser = Session["usuario"].ToString(); //aquí iría la variable global user
            string fecha = Request.Form["fecha"].ToString();
            string totalhoras = Request.Form["horast"].ToString();
            string estado = "Pendiente";
            string motivo = Request.Form["motivo"].ToString();

            Clase_Evidencias formulario = new Clase_Evidencias();
            if (formulario.Formulariotiemposextra(iduser, fecha, totalhoras, estado, motivo) == false)
            {
                Response.Write("<script language='javascript'>window.alert('Error al insertar formulario');window.location='FormulariosyReportes';</script>");
                return null;
            }
            else
            {
                Co1.EnviarCorreo(Jefe, "Creación de un nuevo formulario", "Que tenga un buen día, el funcionario " + nombreUsuario + " ha agregado un nuevo formulario de horas extra, para más información compruebe el sistema.");
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario llena el formulario de horas extra, validando " + totalhoras + " horas");
                Response.Write("<script language='javascript'>window.alert('Formulario agregado exitosamente!!!');window.location='FormulariosyReportes';</script>");
            }
            return View("FormulariosyReportes");
        }
        //insertar jornada
        [HttpPost]
        public ActionResult InsertarJornada()
        {
            //recibir los datos desde la vista
            Bitacora B1 = new Bitacora();

            string fecha = Request.Form["fecha"].ToString();
            string horaentrada = Request.Form["horaentrada"].ToString();
            string horasalida = Request.Form["horasalida"].ToString();
            string hvalidas = Request.Form["hvalidas"].ToString();

            Clase_Evidencias formulario = new Clase_Evidencias();
            if (formulario.jornadatiemposextra(fecha, horaentrada, horasalida, hvalidas) == false)
            {
                Response.Write("<script language='javascript'>window.alert('Error al insertar formulario');window.location='FormulariosyReportes';</script>");
                return null;
            }
            else
            {
                B1.usarBitacora(Session["usuario"].ToString(), "El usuario llena el formulario de jornadas de tiempo extraordinario, con " + hvalidas + " válidas");
                Response.Write("<script language='javascript'>window.alert('Jornada agregada exitosamente!!!');window.location='FormulariosyReportes';</script>");
            }
            return View("FormulariosyReportes");
        }
        //solicitud funcionario horas extra
        #endregion

        #region STEVEN
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;

        public ActionResult Index()
        {
            return View();
        }

        void Conexionstring()
        {
            con.ConnectionString = "Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
            //con.ConnectionString = "server=25.84.109.208 ; database=HorasExtra ; User ID= BancoHabana; Password= progra5";
            //con.ConnectionString = "Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;";
        }

        [HttpPost]
        public ActionResult Index(string user, string password)
        {
            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec LogUsuario '" + user + "', '" + password + "' ";
            dr = com.ExecuteReader();

            if (dr.Read())
            {
                Session["Rol"] = dr.GetString(4);
                Session["nombre"] = dr.GetString(2);
                Session["correo"] = dr.GetString(3);
                Session["Departamento"] = dr.GetString(1);
                Session["usuario"] = dr.GetString(0);
                ViewData["User"] = Session["nombre"];
                ViewData["Rol"] = Session["Rol"];

                if ((string)Session["Rol"] == "Funcionario")
                {


                    //
                    Conexion conn = new Conexion();
                    F_Tareas f = new F_Tareas();
                    try
                    {
                        var resultado = conn.pruebaProcAlmacenado(Session["usuario"].ToString(), "Jefatura");
                        //
                        ViewData["idsolicitud"] = resultado.idsolicitud;

                        ViewData["idActividad"] = resultado.idactividad;
                        //
                        ViewData["fechainicio"] = resultado.fechainicio;
                        ViewData["fechafin"] = resultado.fechafin;
                        ViewData["horas"] = resultado.horas;

                        ViewData["actividad"] = resultado.actividad;
                        ViewData["tarea"] = resultado.tarea;

                        //Esto lo agregué
                        var resultado2 = conn.CargarActividadesTareas(resultado.idsolicitud);
                        ViewData["actividades"] = resultado2;
                        //Hasta Aquí
                        Response.Redirect("~/Home/funcionariostareas");
                        return View("~/Views/Home/funcionariostareas.cshtml");
                    }
                    catch (Exception e)
                    {
                        Response.Write("<script language=javascript>alert('No tiene tareas pendientes');</script>");


                    }
                    Response.Redirect("~/Home/funcionariostareas");
                    return View("~/Views/Home/funcionariostareas.cshtml");
                    //

                }
                if ((string)Session["Rol"] == "Jefatura")
                {
                    Persona P1 = new Persona();
                    ViewData["listaFuncionarios"] = P1.retornarListaUsuarios(Session["usuario"].ToString());
                    SolicitudHorasExtra SHE1 = new SolicitudHorasExtra();
                    ViewData["listaSolicitudes"] = SHE1.retornarSolicitudes(Session["usuario"].ToString());
                    ViewData["listaSolicitudesRechazadas"] = SHE1.retornarSolicitudesRechazadas(Session["usuario"].ToString());
                    ViewData["listaSolicitudesAceptadas"] = SHE1.retornarSolicitudesAprobadas(Session["usuario"].ToString());
                    Response.Redirect("~/Home/solicitud_jefatura");
                    return View("~/Views/Home/solicitud_jefatura.cshtml");
                }
                if ((string)Session["Rol"] == "Director Administrativo")
                {
                    Response.Redirect("~/Director/DaAprobacion");
                    return View("~/Views/Director/DaAprobacion.cshtml");
                }
                if ((string)Session["Rol"] == "Control Tiempo Extraordinario")
                {
                    Response.Redirect("/Tiempo/Pendientes");
                    return View("~/Views/Tiempo/Pendientes.cshtml");
                }
                if ((string)Session["Rol"] == "Planillas")
                {

                    return View("~/Views/Planilla/Planilla.cshtml");
                }

                return View();
            }
            ViewData["err"] = "Usuario no registrado";

            return View();
        }

        [HttpPost]
        public ActionResult enviarDA()
        {
            Correos Co1 = new Correos();
            string nombreUsuario = Co1.retornarNombre(Session["usuario"].ToString());

            Conexionstring();
            con.Open();
            com.Connection = con;
            com.CommandText = "exec ProcesoPagos 'Enviar','','','','','PendienteDA','','','" + (string)Session["usuario"] + "'";
            dr = com.ExecuteReader();

            Co1.EnviarCorreo("Daf001", "Nuevo formulario recibido", "Que tenga un buen día, le informamos que el funcionario " + nombreUsuario + " le ha enviado un formulario para su revisión, para más información compruebe el sistema de horas extra.");
            ViewData["msg"] = "Formularios enviados";

            //AGREGADO GABO
            Conexion conn = new Conexion();
            try
            {
                var resultado = conn.CargarTodosFormularios(Session["usuario"].ToString());
                ViewData["formus"] = resultado;
                return View("FormulariosAvalados");
            }
            catch (Exception e)
            {
                Response.Write("<script language=javascript>alert('No tiene formularios a revisar');</script>");
            }
            return View("FormulariosAvalados");
        }
        #endregion
    }
}