using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EC
{
    public class Estado
    {
        private int estCod;
        private string estNom;

        public int EstCod
        {
            get { return estCod; }
            set { estCod = value; }
        }
        public string EstNom
        {
            get { return estNom; }
            set { estNom = value; }
        }
        public Estado(int pEstCod, string pEstNom)
        {
            EstCod = pEstCod;
            EstNom = pEstNom;
        }
        public Estado() { }
    }
}
