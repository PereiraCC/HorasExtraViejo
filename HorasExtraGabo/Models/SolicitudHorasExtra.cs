using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class SolicitudHorasExtra
    {
        public string idSolicitud { get; set; }
        public List<String> Actividades { get; set; }
        public string idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string fechaInicio { get; set; }
        public string fechaLimite { get; set; }
        public int HorasAprox { get; set; }
        public string TareaPrincipal { get; set; }
        public string Estado { get; set; }
        public string Motivo { get; set; }
        public bool agregarSolicitud(string _Jefatura)
        {
            Correos Co1 = new Correos();
            Bitacora B1 = new Bitacora();
            string idSolicitud = "";
            Persona P1 = new Persona { idUsuario = idUsuario };
            if (P1.ConsultarPersona())
            {
                ConexionBD C1 = new ConexionBD();
                SqlConnection Conexion = C1.AbrirConexion();
                SqlCommand query = new SqlCommand("agregarSolicitud", Conexion);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idUsuario", idUsuario);
                query.Parameters.AddWithValue("@FechaInicio", Convert.ToDateTime(fechaInicio));
                query.Parameters.AddWithValue("@FechaLimite", Convert.ToDateTime(fechaLimite));
                query.Parameters.AddWithValue("@Horas", HorasAprox);
                query.Parameters.AddWithValue("@TareaPrincipal", TareaPrincipal);
                query.Parameters.AddWithValue("@Enviadopor", "Jefatura");

                SqlDataReader rdr = query.ExecuteReader();
                if (rdr.Read())
                {
                    idSolicitud = rdr[0].ToString();
                }
                rdr.Close();

                for (int i = 0; i < Actividades.Count; i++)
                {
                    SqlCommand query2 = new SqlCommand("agregarActividad", Conexion);
                    query2.CommandType = CommandType.StoredProcedure;
                    query2.Parameters.AddWithValue("@Actividad", Actividades[i]);
                    query2.Parameters.AddWithValue("@idSolicitud", idSolicitud);

                    query2.ExecuteNonQuery();
                }
                Co1.EnviarCorreo(this.idUsuario, "Nueva solicitud", "Que tenga un buen día, usted tiene una nueva solicitud pendiente de revisión, compruebe el sistema para más información.");
                B1.usarBitacora(_Jefatura, "Agregada solicitud para realizar horas extra para el usuario con ID " + idUsuario.Trim());
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<SolicitudHorasExtra> retornarSolicitudes(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<SolicitudHorasExtra> ListaSolicitudes = new List<SolicitudHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarSolicitudes", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra
                {
                    idUsuario = rdr["idUsuario"].ToString(),
                    fechaInicio = Convert.ToDateTime(rdr["FechaInicio"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    fechaLimite = Convert.ToDateTime(rdr["FechaLimite"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    HorasAprox = (Int32)rdr["Horas"],
                    TareaPrincipal = rdr["TareaPrincipal"].ToString(),
                    nombreUsuario = rdr["Nombre"].ToString()
                };
                ListaSolicitudes.Add(SHE1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó toda lista de solicitudes realizadas");
            return ListaSolicitudes;
        }

        public List<SolicitudHorasExtra> retornarSolicitudesPendientes(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<SolicitudHorasExtra> ListaSolicitudes = new List<SolicitudHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarSolicitudesPendientes", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra
                {
                    idSolicitud = rdr["idSolicitud"].ToString(),
                    idUsuario = rdr["idUsuario"].ToString(),
                    fechaInicio = Convert.ToDateTime(rdr["FechaInicio"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    HorasAprox = (Int32)rdr["Horas"],
                    TareaPrincipal = rdr["TareaPrincipal"].ToString(),
                    nombreUsuario = rdr["Nombre"].ToString()
                };
                ListaSolicitudes.Add(SHE1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó toda lista de solicitudes pendientes de revisar");
            return ListaSolicitudes;
        }

        public List<SolicitudHorasExtra> retornarSolicitudesAprobadas(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<SolicitudHorasExtra> ListaSolicitudes = new List<SolicitudHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarSolicitudesEstado", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);
            query.Parameters.AddWithValue("@Estado", "Aceptada");

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra
                {
                    idSolicitud = rdr["idSolicitud"].ToString(),
                    idUsuario = rdr["idUsuario"].ToString(),
                    fechaInicio = Convert.ToDateTime(rdr["FechaInicio"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    fechaLimite = Convert.ToDateTime(rdr["FechaLimite"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    HorasAprox = (Int32)rdr["Horas"],
                    TareaPrincipal = rdr["TareaPrincipal"].ToString(),
                    nombreUsuario = rdr["Nombre"].ToString(),

                };
                ListaSolicitudes.Add(SHE1);
            }
            return ListaSolicitudes;
        }

        public List<SolicitudHorasExtra> retornarSolicitudesRechazadas(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<SolicitudHorasExtra> ListaSolicitudes = new List<SolicitudHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarSolicitudesEstado", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);
            query.Parameters.AddWithValue("@Estado", "Rechazada");

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                SolicitudHorasExtra SHE1 = new SolicitudHorasExtra
                {
                    idSolicitud = rdr["idSolicitud"].ToString(),
                    idUsuario = rdr["idUsuario"].ToString(),
                    fechaLimite = Convert.ToDateTime(rdr["FechaLimite"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    HorasAprox = (Int32)rdr["Horas"],
                    TareaPrincipal = rdr["TareaPrincipal"].ToString(),
                    nombreUsuario = rdr["Nombre"].ToString()
                };
                ListaSolicitudes.Add(SHE1);
            }
            return ListaSolicitudes;
        }

        public bool responderSolicitudPendiente(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("responderSolicitudJefatura", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@idSolicitud", idSolicitud);
            query.Parameters.AddWithValue("@Decision", Estado);
            query.Parameters.AddWithValue("@Motivo", Motivo);
            try
            {
                query.ExecuteNonQuery();
                B1.usarBitacora(_Jefatura, "Respondida solicitud pendiente número" + idSolicitud.Trim());
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
