using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class ConexionBD
    {
        public SqlConnection AbrirConexion()
        {
            try
            {
                SqlConnection Conexion = new SqlConnection("Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;");

                //SqlConnection Conexion = new SqlConnection("server=25.84.109.208 ; database=HorasExtra ; User ID= BancoHabana; Password= progra5");
                //SqlConnection Conexion = new SqlConnection("Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;");
                Conexion.Open();
                return Conexion;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}