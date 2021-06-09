using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class Correos
    {
        public bool EnviarCorreo(string idUsuarioReceptor, string EncabezadoCorreo, string ResumenCorreo)
        {
            string emailDestino = retornarCorreo(idUsuarioReceptor);
            string usuario = retornarNombre(idUsuarioReceptor);
            string emailOrigen = "progra5gsa@gmail.com";
            string password = "correo.123";

            MailMessage oMailMessage = new MailMessage(emailOrigen, emailDestino, "Horas Extra " + "Mensaje del sistema",
               "<div style='width:100%;height:100px;background-color: #641220; color:#fff; text-align:center;'><h1>Horas Extra</h1><h3>" + EncabezadoCorreo + "<h3></div>" +
               "<div style='background-color:#fff;height:200px;'><h3>Estimado(a) usuario(a) </h3>" + usuario +
               "<br><br><p>" + ResumenCorreo + "</p> <br>");
            oMailMessage.IsBodyHtml = true;
            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(emailOrigen, password);

            oSmtpClient.Send(oMailMessage);
            return true;
        }

        private string retornarCorreo(string idUsuario)
        {
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("select correo from Usuarios where idUsuario = @idUsuario", Conexion);
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            SqlDataReader rdr = query.ExecuteReader();
            if (rdr.Read())
            {
                return rdr.GetString(0);
            }
            else
            {
                return null;
            }
        }

        public string retornarNombre(string idUsuario)
        {
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("select Nombre from Usuarios where idUsuario = @idUsuario", Conexion);
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            SqlDataReader rdr = query.ExecuteReader();
            if (rdr.Read())
            {
                return rdr.GetString(0);
            }
            else
            {
                return null;
            }
        }

        public string retornarJefe(string idUsuario)
        {
            ConexionBD C1 = new ConexionBD();
            SqlConnection Conexion = C1.AbrirConexion();
            SqlCommand query = new SqlCommand("select Jefe from Usuarios where idUsuario = @idUsuario", Conexion);
            query.Parameters.AddWithValue("@idUsuario", idUsuario);
            SqlDataReader rdr = query.ExecuteReader();
            if (rdr.Read())
            {
                return rdr.GetString(0);
            }
            else
            {
                return null;
            }
        }
    }
}