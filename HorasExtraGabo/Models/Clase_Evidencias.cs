using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class Clase_Evidencias
    {
        //private string connString = "server=25.84.109.208 ; database=HorasExtra ; User ID= BancoHabana; Password= progra5";
        private string connString = "Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;";

        public string idusuario { get; set; }

        public string ruta1 { get; set; }

        public string idactividad { get; set; }

        public string Estado { get; set; }

        public string horas { get; set; }




        public bool agregarevidencia(string idusuario, string ruta1, string idactividad, string horas)
        {


            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_EvidenciasFuncionario @idusuario,@ruta, @idactividad, @horas";
                myCmd.Parameters.Add("@idusuario", SqlDbType.VarChar, 150).Value = idusuario;
                myCmd.Parameters.Add("@ruta", SqlDbType.VarChar, 150).Value = ruta1;
                myCmd.Parameters.Add("@idactividad", SqlDbType.VarChar, 150).Value = idactividad;
                myCmd.Parameters.Add("@horas", SqlDbType.VarChar, 150).Value = horas;


                var result = myCmd.ExecuteReader();

                if (result != null)
                {


                    myConn.Close();
                    return true;
                }
                else
                {
                    myConn.Close();
                    return false;
                }

            }
            return false;
        }

        //
        public bool agregarevidenreporte(string idusuario, string ruta1)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_ReporteAsistencia @idusuario,@ruta";
                myCmd.Parameters.Add("@idusuario", SqlDbType.VarChar, 150).Value = idusuario;
                myCmd.Parameters.Add("@ruta", SqlDbType.VarChar, 150).Value = ruta1;

                var result = myCmd.ExecuteReader();

                if (result != null)
                {
                    myConn.Close();
                    return true;
                }
                else
                {
                    myConn.Close();
                    return false;
                }

            }
            return false;
        }

        //
        public bool jornadatiemposextra(string fecha, string horaentrada, string horasalida, string horasvalidas)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_InsertarJornada @fecha,@horaentrada, @horasalida, @horasvalidas";
                myCmd.Parameters.Add("@fecha", SqlDbType.VarChar, 150).Value = fecha;
                myCmd.Parameters.Add("@horaentrada", SqlDbType.VarChar, 150).Value = horaentrada;
                myCmd.Parameters.Add("@horasalida", SqlDbType.VarChar, 150).Value = horasalida;
                myCmd.Parameters.Add("@horasvalidas", SqlDbType.VarChar, 150).Value = horasvalidas;


                var result = myCmd.ExecuteReader();

                if (result != null)
                {
                    myConn.Close();
                    return true;
                }
                else
                {
                    myConn.Close();
                    return false;
                }

            }
            return false;
        }

        //aquí quedé
        public bool Formulariotiemposextra(string iduser, string fecha, string totalhoras, string estado, string motivo)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_InsertarFormulario @idusuario,@fecha, @totalhoras, @estado, @motivo";
                myCmd.Parameters.Add("@idusuario", SqlDbType.VarChar, 150).Value = "Fun001";
                myCmd.Parameters.Add("@fecha", SqlDbType.VarChar, 150).Value = fecha;
                myCmd.Parameters.Add("@totalhoras", SqlDbType.VarChar, 150).Value = totalhoras;
                myCmd.Parameters.Add("@estado", SqlDbType.VarChar, 150).Value = "Pendiente";
                myCmd.Parameters.Add("@motivo", SqlDbType.VarChar, 150).Value = motivo;

                var result = myCmd.ExecuteReader();

                if (result != null)
                {
                    myConn.Close();
                    return true;
                }
                else
                {
                    myConn.Close();
                    return false;
                }

            }
            return false;
        }


        //
    }


    //
}