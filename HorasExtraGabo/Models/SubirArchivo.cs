using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HorasExtraGabo.Models
{
    public class SubirArchivo
    {
        public String confirmacion { get; set; }
        public Exception error { get; set; }

        public void subirarchivo(String ruta, HttpPostedFileBase file)
        {
            try
            {
                file.SaveAs(ruta);
                this.confirmacion = "Evidencia guardada";

            }
            catch (Exception ex)
            {
                this.error = ex;
            }
        }
    }
}