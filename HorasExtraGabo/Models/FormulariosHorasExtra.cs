using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class FormulariosHorasExtra
    {
        public int idFormularioHorasExtra { get; set; }
        public string idUsuario { get; set; }
        public DateTime FechaFormulario { get; set; }
        public Double totalHoras { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }

        public List<FormulariosHorasExtra> retornarListaFormularios(string _Jefatura)
        {
            List<FormulariosHorasExtra> ListaFormularios = new List<FormulariosHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaFormularios", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                FormulariosHorasExtra FHE1 = new FormulariosHorasExtra
                {
                    idFormularioHorasExtra = (Int32)rdr["idFormularioHorasExtra"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    FechaFormulario = (DateTime)rdr["FechaFormulario"],
                    totalHoras = (Double)rdr["TotalHoras"],
                    Motivo = rdr["Motivo"].ToString(),
                    Estado = rdr["Estado"].ToString()
                };
                ListaFormularios.Add(FHE1);
            }
            return ListaFormularios;
        }
    }
}