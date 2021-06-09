using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class F_Tareas
    {
        public string idsolicitud { get; set; }
        public string idactividad { get; set; }
        public string fechainicio { get; set; }
        public string fechafin { get; set; }
        public string horas { get; set; }
        public string actividad { get; set; }
        public string tarea { get; set; }


    }

    public class FormularioHorasExtra2
    {
        public int id_formulario { get; set; }
        public string idusuario { get; set; }
        public string fechaformulario { get; set; }
        public string totalhoras { get; set; }
        public string estado { get; set; }
        public string motivo { get; set; }
    }


    public class Actividades
    {
        public int idsolicitud { get; set; }
        public string idusuario { get; set; }
        public int id_actividad { get; set; }
        public string actividad { get; set; }
        public string estado { get; set; }
    }
}