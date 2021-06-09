using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class ReportesAsistencia
    {
        public int idReporte { get; set; }
        public string idUsuario { get; set; }
        public string rutaReporte { get; set; }
        public List<ReportesAsistencia> retornarListaReportes(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<ReportesAsistencia> ListaReportes = new List<ReportesAsistencia>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaReportes", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                ReportesAsistencia RA1 = new ReportesAsistencia
                {
                    idReporte = (Int32)rdr["idReporteAsistencia"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    rutaReporte = rdr["rutaReporte"].ToString()
                };
                ListaReportes.Add(RA1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó toda la lista de reportes de asistencia");
            return ListaReportes;
        }
    }
}