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
    public class Articulo
    {
        private string artCod;
        private string artNom;
        private string artTipo;
        private int artTam;
        private double artPre;
        private DateTime artFechVen;
        private Categoria artCatCod;

        [DisplayName("Codigo")]
        public string ArtCod
        {
            get { return artCod; }
            set { artCod = value; }
        }

        [DisplayName("Nombre")]
        public string ArtNom
        {
            get { return artNom; }
            set { artNom = value; }
        }
        [DisplayName("Presentación")]
        public string ArtTipo
        {
            get { return artTipo; }
            set { artTipo = value; }
        }
        [DisplayName("Tamaño")]
        public int ArtTam
        {
            get { return artTam; }
            set { artTam = value; }
        }

        [DisplayName("Precio")]
        public double ArtPre
        {
            get { return artPre; }
            set { artPre = value; }
        }

        [DisplayName("Vencimiento")]
        public DateTime ArtFechVen
        {
            get { return artFechVen; }
            set { artFechVen = value; }
        }

        [DisplayName("Categoria")]
        public Categoria ArtCatCod
        {
            get { return artCatCod; }
            set { artCatCod = value; }
        }
        public Articulo(string partCod, string partNom, string partTipo, int partTam, double partPre, DateTime partFechVen, Categoria partCatCod)
        {
            ArtCod = partCod;
            ArtNom = partNom;
            ArtTipo = partTipo;
            ArtTam = partTam;
            ArtPre = partPre;
            ArtFechVen = partFechVen;
            ArtCatCod = partCatCod;
        }
        public Articulo() { }
        public void Validar()
        {
            if (string.IsNullOrEmpty(this.ArtCod) || !Regex.IsMatch(this.ArtCod.Trim(), "^[A-Za-z0-9]{10}$"))
                throw new Exception("Error de Codigo - Intente nuevamente");
            if (string.IsNullOrEmpty(this.ArtNom) || !Regex.IsMatch(this.ArtNom, "[a-zA-Z0-9 ]{8,50}$"))
                throw new Exception("Error de Nombre - Intente nuevamente");
            if (this.ArtTipo.Trim().ToLower() != "frasco"  && 
                this.ArtTipo.Trim().ToLower() != "blíster"  &&
                this.ArtTipo.Trim().ToLower() != "sobre"  &&
                this.ArtTipo.Trim().ToLower() != "unidad")
                throw new Exception("Error tipo de presentación - Intente nuevamente");
            if (this.ArtTam <= 0)
                throw new Exception("Error de Tamaño - Intente nuevamente");
            if (this.ArtPre <= 0)
                throw new Exception("Error de Precio - Intente nuevamente");
            if (this.ArtCatCod == null )
                throw new Exception("Error de Categoria - Intente nuevamente");
        }
    }
}
