using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EC
{
    public class Empleado
    {
        private string empUsu;
        private string empNom;
        private string empPass;

        [DisplayName("Usuario")]
        [Required(ErrorMessage = "Debe ingresar un Usuario")]
        public string EmpUsu
        {
            get { return empUsu; }
            set { empUsu = value; }
        }
        public string EmpNom
        {
            get { return empNom; }
            set { empNom = value; }
        }

        [DisplayName("Contraseña")]
        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        public string EmpPass
        {
            get { return empPass; }
            set { empPass = value; }
        }

        public Empleado(string pempUsu, string pempNom, string pempPass)
        {
            EmpUsu = pempUsu;
            EmpNom = pempNom;
            EmpPass = pempPass;
        }
        public Empleado() { }
        public void Validar()
        {
            if ((this.EmpUsu.Trim().Length > 25) || (this.EmpUsu.Trim().Length <= 0))
                throw new Exception("Error de usuario - Intente nuevamente");
            if ((this.EmpNom.Trim().Length > 50) || (EmpNom.Trim().Length <= 0))
                throw new Exception("Error de nombre - Intente nuevamente");
            if ((this.EmpPass.Trim().Length > 25) || (this.EmpPass.Trim().Length <= 0))
                throw new Exception("Error de contraseña - Intente nuevamente");
        }
    }
}
