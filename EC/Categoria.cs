using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC
{
    public class Categoria
    {
        private string catCod;
        private string catNom;

        [DisplayName("Codigo")]
        public string CatCod
        {
            get { return catCod; }
            set { catCod = value; }
        }
        [DisplayName("Nombre")]
        public string CatNom
        {
            get { return catNom; }
            set { catNom = value; }
        }
        public Categoria(string pcatCod, string pcatNom)
        {
            CatCod = pcatCod;
            CatNom = pcatNom;
        }
        public Categoria() { }
        public void Validar()
        {
            if (!Regex.IsMatch(this.CatCod.Trim(), "^[A-Za-z0-9]{6}$"))
                throw new Exception("Error de Codigo - Intente nuevamente");
            if (string.IsNullOrEmpty(this.CatNom) || this.CatNom.Trim().Length > 30)
                throw new Exception("Error de Nombre - Intente nuevamente");
        }
    }
}
