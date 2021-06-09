using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class JornadasHorasExtra
    {
        public string idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public int idJornada { get; set; }
        public int idFormularioHorasExtra { get; set; }
        public DateTime Fecha { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSalida { get; set; }
        public int HorasValidas { get; set; }
        

        public List<JornadasHorasExtra> retornarListaJornadas(string _Jefatura)
        {
            Bitacora B1 = new Bitacora();
            List<JornadasHorasExtra> listaJornadas = new List<JornadasHorasExtra>();
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("retornarJornadasHorasExtra", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@Jefatura", _Jefatura);

            SqlDataReader rdr = query.ExecuteReader();
            while (rdr.Read())
            {
                JornadasHorasExtra JHE1 = new JornadasHorasExtra
                {
                    idJornada = (Int32)rdr["idJornada"],
                    Fecha = (DateTime)rdr["Fecha"],
                    HoraEntrada = rdr["HoraEntrada"].ToString(),
                    HoraSalida = rdr["HoraSalida"].ToString(),
                    HorasValidas = (Int32)rdr["HorasValidas"],
                    idFormularioHorasExtra = (Int32)rdr["idFormularioHorasExtra"],
                    idUsuario = rdr["idUsuario"].ToString(),
                    nombreUsuario = rdr["Nombre"].ToString()
                };
                listaJornadas.Add(JHE1);
            }
            B1.usarBitacora(_Jefatura, "Visualizó toda la lista de jornadas de todos los usuarios a su cargo.");
            return listaJornadas;
        }

        public bool validarJornada(string _Jefatura)
        {
            Correos Co1 = new Correos();
            Bitacora B1 = new Bitacora();
            bool bandera = false;
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            if (this.HorasValidas > 5)
            {
                this.HorasValidas = 5;
            }
            SqlCommand query = new SqlCommand("validarJornada", Conexion);
            query.CommandType = CommandType.StoredProcedure;
            query.Parameters.AddWithValue("@HorasValidas", this.HorasValidas);
            query.Parameters.AddWithValue("@idJornada", this.idJornada);
            query.ExecuteNonQuery();

            SqlCommand query2 = new SqlCommand("validarEstadoJornadas", Conexion);
            query2.CommandType = CommandType.StoredProcedure;
            query2.Parameters.AddWithValue("@idFormulario", this.idFormularioHorasExtra);
            SqlDataReader rdr = query2.ExecuteReader();

            if (!rdr.Read())
            {
                bandera = true;
            }
            else
            {
                bandera = false;
            }
            rdr.Close();
            
            if (bandera)
            {
                int sumaHoras = 0;
                SqlCommand query4 = new SqlCommand("sumarJornadas", Conexion);
                query4.CommandType = CommandType.StoredProcedure;
                query4.Parameters.AddWithValue("@idFormulario", this.idFormularioHorasExtra);
                rdr = query4.ExecuteReader();
                while (rdr.Read())
                {
                    sumaHoras = (Int32)rdr.GetInt32(0);
                }

                rdr.Close();
                SqlCommand query3 = new SqlCommand("actualizarEstadoFormularioAprobado", Conexion);
                query3.CommandType = CommandType.StoredProcedure;
                query3.Parameters.AddWithValue("@idFormulario", this.idFormularioHorasExtra);
                query3.Parameters.AddWithValue("@HorasTotales", sumaHoras);
                query3.ExecuteNonQuery();
            }

            Co1.EnviarCorreo(this.idUsuario, "Verificación de jornadas de horas extra", "Que tenga un buen día, sus jornadas de horas extra han sido revisadas, compruebe el sistema para más información.");
            B1.usarBitacora(_Jefatura, _Jefatura.Trim() + " ha validado " + this.HorasValidas + " del usuario " + this.idUsuario);
            return true;
        }
    }
}