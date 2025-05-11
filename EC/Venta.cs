using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EC
{
    public class Venta
    {
        private int venNum;
        private DateTime venFec;
        private double venMon;
        private string venDir;
        
        private Cliente venCliCi;
        private Empleado venEmpUsu;

        private List<DetalleVenta> _listaDetVen;
        private List<HistoricoEstado> _listaHisEst;

        public int VenNum
        {
            get { return venNum; }
            set { venNum = value; }
        }
        public DateTime VenFec
        {
            get { return venFec; }
            set { venFec = value; }
        }
        public double VenMon
        {
            get { return venMon; }
            set { venMon = value; }
        }

        [DisplayName("Dirección de Entrega")]
        public string VenDir
        {
            get { return venDir; }
            set { venDir = value; }
        }
        public Cliente VenCliCi
        {
            get { return venCliCi; }
            set { venCliCi = value; }
        }
        public Empleado VenEmpUsu
        {
            get { return venEmpUsu; }
            set { venEmpUsu = value; }
        }

        public List<DetalleVenta> ListaDetVen
        {
            get { return _listaDetVen; }
            set { _listaDetVen = value; }
        }

        public List<HistoricoEstado> ListaHisEst
        {
            get { return _listaHisEst; }
            set { _listaHisEst = value; }
        }

        public Venta(int pNumVen, DateTime pvenFec, double pvenMon, string pvenDir, Cliente pvenCiCli, Empleado pvenEmpUsu, List<DetalleVenta> plistD, List<HistoricoEstado> plistH)
        {
            VenNum = pNumVen;
            VenFec = pvenFec;
            VenMon = pvenMon;
            VenDir = pvenDir;
            VenCliCi = pvenCiCli;
            VenEmpUsu = pvenEmpUsu;
            ListaDetVen = plistD;
            ListaHisEst = plistH;
        }
        public Venta() { }
        public void Validar()
        {
            if (this.VenMon <= 0)
                throw new Exception("Error de Monto - Intente nuevamente");
            if (!Regex.IsMatch(this.venDir, "[a-zA-Z0-9 ]{8,200}$"))
                throw new Exception("Error de direccion - Intente nuevamente");
            if (this.VenCliCi == null)
                throw new Exception("Error en Cliente - Intente nuevamente");
            if (this.VenEmpUsu == null)
                throw new Exception("Error en Empleado - Intente nuevamente");
            if (this.ListaDetVen == null || this.ListaDetVen.Count == 0)
                throw new Exception("Error en el Detalle Venta - Intente nuevamente");
            if (this.ListaHisEst == null || this.ListaHisEst.Count == 0)
                throw new Exception("Error en el Historico Estado - Intente nuevamente");
        }
    }
}
