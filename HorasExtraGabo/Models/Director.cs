using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitios.Models
{
    public class Director
    {
        public string idProc { get; set; }
        public string idFuncionario { get; set; }
        public string nombre { get; set; }
        public string formulario { get; set; }
        public string reporteAsistencia { get; set; }
        public string hora { get; set; }
        public string mes { get; set; }
        public string quincena { get; set; }

    }
}