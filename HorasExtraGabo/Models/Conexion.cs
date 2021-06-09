using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace HorasExtraGabo.Models
{
    public class Conexion
    {
        string urlDominio = "http://localhost:64044";

        //private string connString = "server=25.84.109.208 ; database=HorasExtra ; User ID = BancoHabana; Password= progra5";
        //private string connString = "Server = SQL5026.site4now.net; Database = DB_A64FCE_gaboFN; User Id = DB_A64FCE_gaboFN_admin; Password = Saprissa007;";
        //private string connString = "Data Source = PEREIRACOTO-PC\\PEREIRASERVER; Initial Catalog = HorasExtra; User ID = sa; Password = Datos.2020;";
        private string connString = "Server = A2NWPLSK14SQL-v03.shr.prod.iad2.secureserver.net; Database = ph179300922261_horasextra; User Id = horasextra; Password = Datos.2020;";

        public F_Tareas pruebaProcAlmacenado(string usuario, string enviado)
        {


            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_cargartareas @usuario,@enviado";
                myCmd.Parameters.Add("@usuario", SqlDbType.Char, 30).Value = usuario;
                myCmd.Parameters.Add("@enviado", SqlDbType.Char, 30).Value = enviado;

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        F_Tareas tarea = new F_Tareas
                        {
                            idsolicitud = dr["idsolicitud"].ToString(),
                            idactividad = dr["idActividad"].ToString(),
                            fechainicio = Convert.ToDateTime(dr["FechaInicio"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            fechafin = Convert.ToDateTime(dr["FechaLimite"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            horas = dr["Horas"].ToString(),
                            actividad = dr["Actividad"].ToString(),
                            tarea = dr["TareaPrincipal"].ToString()

                        };
                        myConn.Close();
                        return tarea;
                    }
                }
                return null;
            }
        }

        //
        public List<Actividades> CargarActividades(string usuario)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                List<Actividades> formulario = new List<Actividades>();
                myCmd.CommandText = "Execute Sp_CargarActividadesUsuario @usuario";
                myCmd.Parameters.Add("@usuario", SqlDbType.Char, 30).Value = usuario;

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var act = new Actividades();
                        act.id_actividad = Convert.ToInt16(dr["idActividad"]);
                        act.idsolicitud = Convert.ToInt16(dr["idSolicitud"].ToString());
                        act.actividad = dr["Actividad"].ToString();
                        formulario.Add(act);
                    }
                    myConn.Close();
                    return formulario;
                }
            }
        }

        //esto lo agregué
        public List<Actividades> CargarActividadesTareas(string idsoli)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                List<Actividades> formulario = new List<Actividades>();
                myCmd.CommandText = "Execute Sp_CargarActividades @idsolicitud";
                myCmd.Parameters.Add("@idsolicitud", SqlDbType.Char, 30).Value = idsoli;

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var act = new Actividades();
                        act.id_actividad = Convert.ToInt16(dr["idActividad"]);
                        act.actividad = dr["Actividad"].ToString();
                        formulario.Add(act);
                    }
                    myConn.Close();
                    return formulario;
                }
            }
        }
        //horas

        public FormularioHorasExtra2 CargarHoras(string usuario)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute sp_totalhoras @usuario";
                myCmd.Parameters.Add("@usuario", SqlDbType.Char, 30).Value = usuario;

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        FormularioHorasExtra2 tarea = new FormularioHorasExtra2
                        {
                            totalhoras = dr["TotalHoras"].ToString(),

                        };
                        myConn.Close();
                        return tarea;
                    }
                }
                return null;
            }
        }

        //


        public List<FormularioHorasExtra2> CargarTodosFormularios(string usuario)
        {
            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_ConsultarFormularioFuncionario @usuario";
                myCmd.Parameters.Add("@usuario", SqlDbType.Char, 30).Value = usuario;

                List<FormularioHorasExtra2> form = new List<FormularioHorasExtra2>();

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {

                        var formula = new FormularioHorasExtra2();
                        formula.id_formulario = Convert.ToInt16(dr["idFormularioHorasExtra"]);
                        formula.estado = dr["Estado"].ToString();
                        formula.fechaformulario = Convert.ToDateTime(dr["FechaFormulario"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
                        formula.motivo = dr["Motivo"].ToString();
                        form.Add(formula);
                    }
                    myConn.Close();
                    return form;
                }
            }
        }

        //

        public FormularioHorasExtra2 cargarestado(string usuario)
        {


            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute Sp_ConsultarFormularioFuncionario @usuario";
                myCmd.Parameters.Add("@usuario", SqlDbType.Char, 30).Value = usuario;


                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        FormularioHorasExtra2 formulario = new FormularioHorasExtra2
                        {
                            id_formulario = Convert.ToInt16(dr["idFormularioHorasExtra"]),
                            estado = dr["Estado"].ToString(),
                            motivo = dr["Motivo"].ToString(),
                        };

                        myConn.Close();
                        return formulario;
                    }

                }
                return null;
            }

        }

        public FormularioHorasExtra2 EstadoFormulario(int id)
        {


            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                myCmd.CommandText = "Execute SpConsultarEstadoFormulario @id";
                myCmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                using (SqlDataReader dr = myCmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        FormularioHorasExtra2 formulario = new FormularioHorasExtra2
                        {
                            id_formulario = Convert.ToInt16(dr["idFormularioHorasExtra"]),
                            estado = dr["Estado"].ToString(),
                            motivo = dr["Motivo"].ToString(),
                        };

                        myConn.Close();

                        return formulario;
                    }
                }
                return null;
            }

        }

        //Pruebas
        public FormularioHorasExtra2 AceptarTareaFuncionario(string id_solicitud)
        {
            F_Tareas f = new F_Tareas();

            using (SqlConnection myConn = new SqlConnection(connString))
            {
                myConn.Open();
                var myCmd = myConn.CreateCommand();
                SqlCommand query = new SqlCommand("Sp_AceptarTareaFuncionario", myConn);
                query.CommandType = CommandType.StoredProcedure;
                query.Parameters.AddWithValue("@idsolicitud", id_solicitud);
                try
                {
                    query.ExecuteNonQuery();
                }
                catch
                {

                }

                return null;
            }

        }


    }
}