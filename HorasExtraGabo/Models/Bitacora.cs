using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class Bitacora
    {
        public int idBitacora { get; set; }
        public string idUsuario { get; set; }
        public string Fecha { get; set; }
        public string Resumen { get; set; }
        
        /*NO TOCAR*/
        public List<Bitacora> retornarListaBitacora()
        {
            List<Bitacora> listaBitacora = new List<Bitacora>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarBitacora", Conexion);
            query.CommandType = CommandType.StoredProcedure;

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                Bitacora B1 = new Bitacora
                {
                    idBitacora = (Int32)rdr["idBitacora"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    Fecha = Convert.ToDateTime(rdr["Fecha"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                    Resumen = rdr["Resumen"].ToString()
                };
                listaBitacora.Add(B1);
            }
            return listaBitacora;
        }
        /*NO TOCAR*/

        public void usarBitacora(string idUsuario, string Resumen)
        {
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("usarBitacora", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            query.Parameters.AddWithValue("@Resumen", Resumen);
            query.ExecuteNonQuery();
        }
    }
}