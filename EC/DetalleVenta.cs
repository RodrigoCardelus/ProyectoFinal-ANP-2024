using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class DetalleVenta
    {
        private int detVenCant;
        private Articulo detVenArtCod;

        public int DetVenCant
        {
            get { return detVenCant; }
            set { detVenCant = value; }
        }
        public Articulo DetVenArtCod
        {
            get { return detVenArtCod; }
            set { detVenArtCod = value; }
        }
        public DetalleVenta(int pdetVenCant, Articulo pdetVenArtCod)
        {
            DetVenCant = pdetVenCant;
            DetVenArtCod = pdetVenArtCod;
        }
        public DetalleVenta() { }
        public void Validar()
        {
            if (this.DetVenCant <= 0)
                throw new Exception("Error en Cantidad - Intente nuevamente");
            if (this.DetVenArtCod == null)
                throw new Exception("Error en Articulo - Intente nuevamente");
        }
    }
}
