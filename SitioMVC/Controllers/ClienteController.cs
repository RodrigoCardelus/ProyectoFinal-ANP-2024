using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;
using Logica;

namespace SitioMVC.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult FormClienteListar(string nombre)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                List<Cliente> lista = FabricaLogica.GetLCliente().Listar(usuL);

                if (lista.Count >= 1)
                {
                    if (String.IsNullOrEmpty(nombre))
                        return View(lista);
                    else
                        lista = (from unC in lista
                                 where unC.CliNom.Trim().ToLower().StartsWith(nombre.Trim().ToLower())
                                 select unC).ToList();
                    return View(lista);
                }
                else
                    throw new Exception("No hay clientes activos");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Cliente>());
            }
        }
        [HttpGet]
        public ActionResult FormAltaCliente()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormAltaCliente(Cliente C)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                C.Validar();

                FabricaLogica.GetLCliente().AgregarCliente(C, usuL);

                return RedirectToAction("FormClienteListar", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Cliente());
            }
        }
        [HttpGet]
        public ActionResult FormModificarCliente(string CliCi)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                Cliente C = FabricaLogica.GetLCliente().BuscarCliente(CliCi, usuL);

                if (C != null)
                    return View(C);
                else
                    throw new Exception("No existe el cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Cliente());
            }
        }
        [HttpPost]
        public ActionResult FormModificarCliente(Cliente C)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                C.Validar();

                FabricaLogica.GetLCliente().ModificarCliente(C, usuL);

                return RedirectToAction("FormClienteListar", "Cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(C);
            }
        }
        [HttpGet]
        public ActionResult FormClienteBuscar(string CliCi)
        {
            try
            {

                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                Cliente c = FabricaLogica.GetLCliente().BuscarCliente(CliCi, usuL);

                if (c != null)
                    return View(c);
                else
                    throw new Exception("No existe el cliente");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Cliente());
            }
        }
        public ActionResult ListadoCompleto(string nombre)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                List<Cliente> listCli = Logica.FabricaLogica.GetLCliente().Listar(usuL);

                listCli = (from unC in listCli
                           orderby unC.CliNom
                           select unC).ToList();

                if (listCli.Count == 0)
                    throw new Exception("No hay clientes para mostrar");

                if (listCli.Count >= 1)
                {
                    if (String.IsNullOrEmpty(nombre))
                        return View(listCli);
                    else
                    {
                        listCli = (from unC in listCli
                                   where unC.CliNom.ToLower().StartsWith(nombre.Trim().ToLower())
                                   select unC).ToList();

                        return View(listCli);
                    }
                }
                else
                    throw new Exception("No hay clientes con dicho nombre");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Cliente>());
            }
        }
    }
}