using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;
using Logica;

namespace SitioMVC.Controllers
{
    public class VentaController : Controller
    {
        [HttpGet]
        public ActionResult FormAgregarVenta()
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                List<Cliente> listCliente  =  FabricaLogica.GetLCliente().Listar(usuL);

                Session["ListCliente"] = listCliente;

                ViewBag.ListaCliente = new SelectList(listCliente, "CliCi", "CliNom");

                return View();
            }
            catch (Exception ex)
            {

                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaCliente = new SelectList((List<Cliente>)Session["ListCliente"], "CliCi", "CliNom");
                return View();
            }
        }
        [HttpPost]
        public ActionResult FormAgregarVenta(Venta venta, string SeleccionCliente)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                venta.VenEmpUsu = usuL;

                venta.VenCliCi = FabricaLogica.GetLCliente().BuscarCliente(SeleccionCliente, usuL);

                Estado estado = FabricaLogica.GetLEstado().BuscarEstado(1, usuL);

                venta.ListaHisEst = new List<HistoricoEstado> { new HistoricoEstado(DateTime.Now, estado) };

                venta.ListaDetVen = new List<DetalleVenta>();


                Session["Venta"] = venta;

                return RedirectToAction("FormAgregarDetalleVenta", "Venta");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }
        [HttpGet]
        public ActionResult FormAgregarDetalleVenta()
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                List<Articulo> listArticulos = FabricaLogica.GetLArticulo().Listar(usuL);
                ViewBag.ListaArticulos = new SelectList(listArticulos, "ArtCod", "ArtNom");
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.ListaArticulos = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }
        [HttpPost]
        public ActionResult FormAgregarDetalleVenta(DetalleVenta detVenta, string CodArt)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                if (CodArt.Trim().Length > 0)
                {
                    
                    detVenta.DetVenArtCod = FabricaLogica.GetLArticulo().BuscarArticulo(CodArt, usuL);

                }

                detVenta.Validar();

                bool conArt = (from unA in ((Venta)Session["Venta"]).ListaDetVen.ToList()
                               where unA.DetVenArtCod.ArtCod == CodArt
                               select unA).Any();
                if (conArt) 
                {
                    ((Venta)Session["Venta"]).ListaDetVen.Where(a => a.DetVenArtCod.ArtCod == CodArt).FirstOrDefault().DetVenCant += detVenta.DetVenCant;
                }
                else 
                { 
                
                    ((Venta)Session["Venta"]).ListaDetVen.Add(detVenta);
                
                }


                return RedirectToAction("FormAgregarDetalleVenta", "Venta");

            }
            catch (Exception ex)
            {

                Empleado usuL = (Empleado)Session["EmpLogueado"];
                List<Articulo> _ListaA = FabricaLogica.GetLArticulo().Listar(usuL);
                ViewBag.ListaArticulos = new SelectList(_ListaA, "ArtCod", "ArtNom");
                ViewBag.Mensaje = ex.Message;
                return View();

            }
        }
        [HttpGet]
        public ActionResult AgregarVenta()
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                double total = 0;

                Venta venta = ((Venta)Session["Venta"]);

                foreach (DetalleVenta dventa in venta.ListaDetVen)
                {
                    
                    
                        total += dventa.DetVenCant * dventa.DetVenArtCod.ArtPre;
                    
                }

                venta.VenMon = total;

                venta.Validar();

                FabricaLogica.GetLVenta().AgregarVenta(venta, usuL);

                return RedirectToAction("FormAltaExito", "Venta");
            }
            catch (Exception ex)
            {
                Session["ErrorFactura"] = ex.Message;
                return RedirectToAction("FormAltaError", "Venta");

            }
        }
        public ActionResult FormAltaExito()
        {
            return View();
        }
        public ActionResult FormAltaError()
        {
            ViewBag.Mensaje = Session["ErrorFactura"].ToString();
            return View();
        }
        public ActionResult FormVentasListar(string FechVen, string estado)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                ViewBag.Estado = new SelectList(FabricaLogica.GetLEstado().Listar(usuL), "EstCod", "EstNom");

                List<Venta> _lista = FabricaLogica.GetLVenta().Listar(usuL);

                Session["Venta"] = _lista;

                if (_lista.Count >= 1)
                {
                           
                                       
                    if (!string.IsNullOrEmpty(FechVen))
                    {
                        _lista = (from unV in _lista
                                  where unV.VenFec.Date == Convert.ToDateTime(FechVen)
                                  select unV
                                  ).ToList();
                       
                        return View(_lista);
                    }
                    if (!string.IsNullOrEmpty(estado))
                    {
                        _lista = (from unV in _lista
                                  where unV.ListaHisEst.LastOrDefault().VenEst.EstCod == Convert.ToInt32(estado)
                                  select unV
                                  ).ToList();
                        
                        return View(_lista);
                    }

                    return View(_lista);
                }
                else
                    throw new Exception("No hay Ventas");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());

            }
        }
        public ActionResult CambiarEstado(int VenNum)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                List<Venta> listVenta = (List<Venta>)Session["Venta"];

                Venta unaVent = (from unaV in listVenta
                                 where unaV.VenNum == VenNum
                                 select unaV).FirstOrDefault();


                int ultimoEstado = unaVent.ListaHisEst.LastOrDefault().VenEst.EstCod ;
                int sigEstado = 0;
                if (ultimoEstado == 4)
                {
                    throw new Exception("Fue ultilizado el ultimo estado");
                }
                else 
                {
                     sigEstado =  ultimoEstado += 1;
                }

                Estado estado = FabricaLogica.GetLEstado().BuscarEstado(sigEstado, usuL);

                
                HistoricoEstado agregaEstado = new HistoricoEstado(DateTime.Now, estado);
                unaVent.ListaHisEst.Add(agregaEstado);

           
                FabricaLogica.GetLVenta().CambiarEstadoVen(unaVent, usuL);


                return RedirectToAction("FormVentasListar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return RedirectToAction("FormVentasListar");
               
            }
        }
        public ActionResult VentasCli(string CliCi)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Cliente cli = Logica.FabricaLogica.GetLCliente().BuscarCliente(CliCi, usuL);

                List<Venta> listaVen = Logica.FabricaLogica.GetLVenta().Listar(usuL);

                listaVen = (from unaV in listaVen
                            where unaV.VenCliCi.CliCi == cli.CliCi
                            orderby unaV.VenFec
                            select unaV).ToList();

                if (listaVen.Count == 0)
                    throw new Exception("No hay Ventas para mostrar");
                else
                    return View(listaVen);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());
            }
        }
        public ActionResult VentasArt(string ArtCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Articulo art = Logica.FabricaLogica.GetLArticulo().BuscarArticulo(ArtCod, usuL);

                List<Venta> listaVen = Logica.FabricaLogica.GetLVenta().Listar(usuL);

                listaVen = (from unaV in listaVen
                            from unA in unaV.ListaDetVen
                            where unA.DetVenArtCod.ArtCod == ArtCod
                            orderby unaV.VenFec
                            select unaV).ToList();

                if (listaVen.Count == 0)
                    throw new Exception("No hay Ventas para mostrar");
                else
                    return View(listaVen);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Venta>());
            }
        }

    }


}
