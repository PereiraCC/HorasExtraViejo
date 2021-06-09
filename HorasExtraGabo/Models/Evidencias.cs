using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class Evidencias
    {
        public int idEvidencia { get; set; }
        public string idUsuario { get; set; }
        public string RutaDocumento { get; set; }
        public int idActividad { get; set; }
        public string Estado { get; set; }
        public int Horas { get; set; }
        public string Actividad { get; set; }
        public string nombreFuncionario { get; set; }

        public List<Evidencias> retornarListaEvidenciasJefatura(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<Evidencias> ListaEvidencias = new List<Evidencias>(); 
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaEvidencias", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                Evidencias E1 = new Evidencias
                {
                    nombreFuncionario = rdr["Nombre"].ToString(),
                    idEvidencia = (Int32)rdr["idEvidencia"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    RutaDocumento = rdr["RutaDocumento"].ToString(),
                    idActividad = (Int32)rdr["idActividad"],
                    Estado = rdr["Estado"].ToString(),
                    Horas = (Int32)rdr["Horas"],
                    Actividad = rdr["Actividad"].ToString()
                };
            ListaEvidencias.Add(E1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó la lista de evidencias");
            return ListaEvidencias;
        }

        public List<Evidencias> retornarListaEvidenciasJefaturaAprobadas(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<Evidencias> ListaEvidencias = new List<Evidencias>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaEvidenciasEstado", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);
            query.Parameters.AddWithValue("@Estado", "Aprobado");

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                Evidencias E1 = new Evidencias
                {
                    nombreFuncionario = rdr["Nombre"].ToString(),
                    idEvidencia = (Int32)rdr["idEvidencia"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    RutaDocumento = rdr["RutaDocumento"].ToString(),
                    idActividad = (Int32)rdr["idActividad"],
                    Estado = rdr["Estado"].ToString(),
                    Horas = (Int32)rdr["Horas"],
                    Actividad = rdr["Actividad"].ToString()
                };
                ListaEvidencias.Add(E1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó la lista de evidencias");
            return ListaEvidencias;
        }

        public List<Evidencias> retornarListaEvidenciasJefaturaRechazadas(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<Evidencias> ListaEvidencias = new List<Evidencias>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaEvidenciasEstado", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);
            query.Parameters.AddWithValue("@Estado", "Rechazado");

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                Evidencias E1 = new Evidencias
                {
                    nombreFuncionario = rdr["Nombre"].ToString(),
                    idEvidencia = (Int32)rdr["idEvidencia"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    RutaDocumento = rdr["RutaDocumento"].ToString(),
                    idActividad = (Int32)rdr["idActividad"],
                    Estado = rdr["Estado"].ToString(),
                    Horas = (Int32)rdr["Horas"],
                    Actividad = rdr["Actividad"].ToString()
                };
                ListaEvidencias.Add(E1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó toda la lista de evidencias");
            return ListaEvidencias;
        }


        public bool actualizarEstadoEvidencia(string _Jefatura)
        {
            Correos CO1 = new Correos();
            Bitacora B1 = new Bitacora();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("actualizarEstadoEvidencias", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@idEvidencia", this.idEvidencia);
            query.Parameters.AddWithValue("@Estado", this.Estado);
            try
            {
                query.ExecuteNonQuery();
                CO1.EnviarCorreo(this.idUsuario, "¡Su evidencia ha sido revisada!", "Que tenga un buen día, una de sus evidencias ha sido revisada, consulte el sistema para comprobar el estado.");
                B1.usarBitacora(_Jefatura, "La evidencia " + this.idEvidencia + " del usuario " + this.idUsuario.Trim() + " fue " + this.Estado.Trim() + " por el jefe " + _Jefatura.Trim());
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}