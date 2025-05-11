using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class HistoricoEstado
    {
        private DateTime fecha;
        private Estado venEst;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public Estado VenEst
        {
            get { return venEst; }
            set { venEst = value; }
        }
        public HistoricoEstado(DateTime pfecha, Estado pvenEst)
        {
            Fecha = pfecha;
            VenEst = pvenEst;
        }
        public HistoricoEstado() { }
        public void Validar()
        {
            if (this.VenEst == null)
                throw new Exception("Error en Estado - Intente nuevamente");
        }
    }
}
