using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class Persona
    {
        public string idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Tecnico { get; set; }
        public decimal SalarioxHora { get; set; }

        public bool ConsultarPersona()
        {
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("ConsultarFuncionario", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            SqlDataReader rdr = query.ExecuteReader();
            if (rdr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Persona> retornarListaUsuarios(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<Persona> ListaPersonas = new List<Persona>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarListaFuncionarios", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);
            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                Persona P1 = new Persona();
                P1.idUsuario = rdr["idUsuario"].ToString();
                P1.Nombre = rdr["Nombre"].ToString();
                P1.Correo = rdr["Correo"].ToString();
                P1.Tecnico = rdr["Tecnico"].ToString();
                P1.SalarioxHora = (Decimal)rdr["SalarioxHora"];
                ListaPersonas.Add(P1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó la lista de funcionarios a su cargo");
            return ListaPersonas;
        }
    }
}