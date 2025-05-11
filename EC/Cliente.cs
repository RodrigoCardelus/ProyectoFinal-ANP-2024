using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EC
{
    public class Cliente
    {
        private string cliCi;
        private string cliNom;
        private string cliNumTar;
        private string cliTel;

        [DisplayName("Cédula")]
        public string CliCi
        {
            get { return cliCi; }
            set { cliCi = value; }
        }
        [DisplayName("Nombre")]
        public string CliNom
        {
            get { return cliNom; }
            set { cliNom = value; }
        }

        [DisplayName("N. Tarjeta")]
        public string CliNumTar
        {
            get { return cliNumTar; }
            set { cliNumTar = value; }
        }

        [DisplayName("Teléfono")]
        public string CliTel
        {
            get { return cliTel; }
            set { cliTel = value; }
        }

        public Cliente(string pcliCi, string pcliNom, string pcliNumTar, string pcliTel)
        {
            CliCi = pcliCi;
            CliNom = pcliNom;
            CliNumTar = pcliNumTar;
            CliTel = pcliTel;
        }
        public Cliente() { }
        public void Validar()
        {
            if (string.IsNullOrEmpty(this.CliCi) || !Regex.IsMatch(this.CliCi.Trim(), "^[1-6][0-9]{7}$|^[1-9][0-9]{6}$"))
                throw new Exception("Error de cedula - Intente nuevamente");
            if (string.IsNullOrEmpty(this.CliNom) || this.CliNom.Trim().Length > 50)
                throw new Exception("Error de nombre - Intente nuevamente");
            if (string.IsNullOrEmpty(this.CliNumTar) || !Regex.IsMatch(this.CliNumTar.Trim(), "^[0-9]{16}$"))
                throw new Exception("Error de tarjeta - Intente nuevamente");
            if (string.IsNullOrEmpty(this.CliTel) || !Regex.IsMatch(this.CliTel.Trim(), "^[0][9][0-9]{7}$|^[24][0-9]{7}$"))
                throw new Exception("Error de telefono - Intente nuevamente");
        }
    }
}
