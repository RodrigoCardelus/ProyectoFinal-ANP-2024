using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;
using Logica;

namespace SitioMVC.Controllers
{
    public class ArticuloController : Controller
    {
        public ActionResult FormArticulosListar(string nombre)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Empleado usuDB = null; // se agrego validacion si el usuario DB no existe.
                try
                {
                    usuDB = FabricaLogica.GetLEmpleado().BuscarEmpleado(usuL.EmpUsu, usuL);
                }
                catch (Exception )
                {
                   
                    Session["EmpLogueado"] = null;
                    return RedirectToAction("Ingresar", "Empleado");
                }


                List<Articulo> _lista = FabricaLogica.GetLArticulo().Listar(usuL);

                if (_lista.Count >= 1)
                {

                    if (string.IsNullOrEmpty(nombre))
                        return View(_lista);
                    else
                    {
                        _lista = (from unA in _lista
                                  where unA.ArtNom.Trim().ToLower().StartsWith(nombre.Trim().ToLower())
                                  select unA
                                  ).ToList();
                        return View(_lista);
                    }

                }
                else
                    throw new Exception("No hay Articulos");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());

            }
        }
        [HttpGet]
        public ActionResult FormModificarArticulo(string ArtCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }


                List<Categoria> listCategoria = FabricaLogica.GetLCategoria().Listar(usuL);
                Session["ListCategoria"] = listCategoria;



                Articulo articulo = FabricaLogica.GetLArticulo().BuscarArticulo(ArtCod, usuL);



                List<string> listaTipoPresentacion = new List<string>();
                listaTipoPresentacion.Add("Seleccione Tipo");
                listaTipoPresentacion.Add("Blíster");
                listaTipoPresentacion.Add("Frasco");
                listaTipoPresentacion.Add("Sobre");
                listaTipoPresentacion.Add("Unidad");
                ViewBag.ListaPresentacion = new SelectList(listaTipoPresentacion, articulo.ArtTipo);


               

                if (articulo != null)
                {
                    ViewBag.ListaCategoria = new SelectList(listCategoria, "CatCod", "CatNom", articulo.ArtCatCod.CatCod);

                    return View(articulo);
                }
                else
                    throw new Exception("No existe articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaCategoria = new SelectList((List<Categoria>)Session["ListCategoria"], "CatCod", "CatNom");
                return View(new Articulo());
            }
        }
        [HttpPost]
        public ActionResult FormModificarArticulo(Articulo articulo, string seleccionCategoria, string SeleccionPresentacion)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }


                Categoria categoria = FabricaLogica.GetLCategoria().BuscarCategoria(seleccionCategoria, usuL);


                List<string> listaTipoPresentacion = new List<string>();
                listaTipoPresentacion.Add("Seleccione Tipo");
                listaTipoPresentacion.Add("Blíster");
                listaTipoPresentacion.Add("Frasco");
                listaTipoPresentacion.Add("Sobre");
                listaTipoPresentacion.Add("Unidad");
                ViewBag.ListaPresentacion = new SelectList(listaTipoPresentacion);

                articulo.ArtCatCod = categoria;
                articulo.ArtTipo = SeleccionPresentacion;
                articulo.Validar();

                FabricaLogica.GetLArticulo().ModificarArticulo(articulo, usuL);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaCategoria = new SelectList((List<Categoria>)Session["ListCategoria"], "CatCod", "CatNom", articulo.ArtCatCod.CatCod);
                return View(articulo);
            }
        }
        [HttpGet]
        public ActionResult FormEliminarArticulo(string ArtCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Articulo articulo = FabricaLogica.GetLArticulo().BuscarArticulo(ArtCod, usuL);
                if (articulo != null)
                    return View(articulo);
                else
                    throw new Exception("No existe articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }
        [HttpPost]
        public ActionResult FormEliminarArticulo(Articulo articulo)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }


                FabricaLogica.GetLArticulo().EliminarArticulo(articulo, usuL);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }
        [HttpGet]
        public ActionResult FormAgregarArticulo()
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }



                List<Categoria> listCategoria = FabricaLogica.GetLCategoria().Listar(usuL);

                List<string> listaTipoPresentacion = new List<string>();
                listaTipoPresentacion.Add("Seleccione Tipo");
                listaTipoPresentacion.Add("Blíster");
                listaTipoPresentacion.Add("Frasco");
                listaTipoPresentacion.Add("Sobre");
                listaTipoPresentacion.Add("Unidad");
                ViewBag.ListaPresentacion = new SelectList(listaTipoPresentacion);

                ViewBag.ListaCategoria = new SelectList(listCategoria, "CatCod", "CatNom");

                return View();
            }
            catch (Exception ex)
            {

                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaCategoria = new SelectList((List<Categoria>)Session["ListCategoria"], "CatCod", "CatNom");
                return View();
            }

        }
        [HttpPost]
        public ActionResult FormAgregarArticulo(Articulo articulo, string seleccionCategoria, string SeleccionPresentacion)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Categoria categoria = FabricaLogica.GetLCategoria().BuscarCategoria(seleccionCategoria, usuL);

                articulo.ArtCatCod = categoria;

                List<string> listaTipoPresentacion = new List<string>();
                listaTipoPresentacion.Add("Seleccione Tipo");
                listaTipoPresentacion.Add("Blíster");
                listaTipoPresentacion.Add("Frasco");
                listaTipoPresentacion.Add("Sobre");
                listaTipoPresentacion.Add("Unidad");
                ViewBag.ListaPresentacion = new SelectList(listaTipoPresentacion);
                articulo.ArtTipo = SeleccionPresentacion;

                articulo.Validar();

                FabricaLogica.GetLArticulo().AgregarArticulo(articulo, usuL);
                return RedirectToAction("FormArticulosListar", "Articulo");
            }
            catch (Exception ex)
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                ViewBag.ListaCategoria = new SelectList((List<Categoria>)FabricaLogica.GetLCategoria().Listar(usuL), "CatCod", "CatNom");
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult FormConsultarArticulo(string ArtCod)
        {
            try
            {

                Empleado usuL = (Empleado)Session["EmpLogueado"];
               
                if (usuL == null)
                {
                  return RedirectToAction("Ingresar", "Empleado");                                       
                }
                                  

                Articulo unArticulo = FabricaLogica.GetLArticulo().BuscarArticulo(ArtCod, usuL);
                if (unArticulo != null)
                    return View(unArticulo);
                else
                    throw new Exception("No existe articulo");
            }
            catch (Exception ex)
            {
                
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }
        public ActionResult ListadoInteractivoArticulos(string nombre, string ArticuloCat)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                List<Articulo> listArt = Logica.FabricaLogica.GetLArticulo().Listar(usuL);

                listArt = (from unA in listArt
                           orderby unA.ArtNom
                           select unA).ToList();

                if (listArt.Count == 0)
                    throw new Exception("No hay articulos para mostrar");

                if (listArt.Count >= 1)
                {
                    List<Categoria> listCat = Logica.FabricaLogica.GetLCategoria().Listar(usuL);
                    listCat.Insert(0, new Categoria("0", "Seleccione una Cat"));
                    ViewBag.ListaAC = new SelectList(listCat, "CatCod", "CatNom");

                    if (!String.IsNullOrEmpty(nombre))
                    {
                        listArt = (from unA in listArt
                                   where unA.ArtNom.ToLower().StartsWith(nombre.Trim().ToLower())
                                   select unA).ToList();
                    }
                    if (!String.IsNullOrEmpty(ArticuloCat) && ArticuloCat.Trim() != "0")
                    {
                        listArt = (from unA in listArt
                                   where unA.ArtCatCod.CatCod == ArticuloCat
                                   select unA).ToList();
                    }

                    return View(listArt);
                }
                else
                    throw new Exception("No hay articulos o categorias con dicho nombre");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                ViewBag.ListaAC = new SelectList(new List<Categoria> {new Categoria("0", "Seleccione una Cat")},"CatCod", "CatNom");
                return View(new List<Articulo>());
                
            }
        }
        public ActionResult DatosCompletos(string ArtCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];

                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                Articulo unArticulo = FabricaLogica.GetLArticulo().BuscarArticulo(ArtCod, usuL);

                if (unArticulo != null)
                    return View(unArticulo);
                else
                    throw new Exception("No existe el articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }
        public ActionResult ArticulosCli(string CliCi)
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

                List<Articulo> listArt = (from unV in listaVen
                                          where unV.VenCliCi.CliCi == cli.CliCi
                                          from unA in unV.ListaDetVen
                                          orderby unA.DetVenArtCod.ArtNom
                                          group unA.DetVenArtCod by unA.DetVenArtCod.ArtCod
                                          into grupo
                                          select grupo.First()).ToList();

                if (listArt.Count == 0)
                    throw new Exception("No hay articulos para listar");
                else
                    return View(listArt);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());
            }
        }
    }
}