using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;

namespace SitioMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult Ingresar()
        {
            return View();
        }
        public ActionResult Deslogueo()
        {
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Ingresar(Empleado empleado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Empleado unEmp = Logica.FabricaLogica.GetLEmpleado().Logueo(empleado.EmpUsu.Trim().ToLower(), empleado.EmpPass.Trim());

                                      
                    
                    if (unEmp == null)
                    {
                        throw new Exception("Usuario o contraseña invalidos.");
                    }
                    else 
                    {
                        unEmp.Validar();
                        Session["EmpLogueado"] = unEmp;
                        return RedirectToAction("FormArticulosListar", "Articulo");

                    }
                        

                }
                else
                {
                    return View();

                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }


        }
    }
}