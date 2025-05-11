using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EC;
using Logica;

namespace SitioMVC.Controllers
{
    public class CategoriaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult FormCategoriaListar(Categoria C)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                //obtengo lista de Categorias
                List<Categoria> _lista = FabricaLogica.GetLCategoria().Listar(usuL);
                if (_lista.Count >= 1)
                    return View(_lista);
                else
                    throw new Exception("No hay Categoria Para Mostrar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Categoria>());

            }
        }
        [HttpGet]
        public ActionResult FormCategoriaNuevo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FormCategoriaNuevo(Categoria C)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }

                //valido objeto correctamente
                C.Validar();

                //intento agregar Categoria en la bd
                FabricaLogica.GetLCategoria().AgregarCategoria(C, usuL);

                //no hubo error, alta correcta
                return RedirectToAction("FormCategoriaListar", "Categoria");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult FormCategoriasModificar(string CatCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                //intento buscar Categoria en la bd
                Categoria categoria = FabricaLogica.GetLCategoria().BuscarCategoria(CatCod, usuL);

                //busco si la categoria existe o no
                if (categoria != null)
                {
                    return View(categoria);
                }                   
                else 
                {
                    return View();
                }
                    
               
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }
        [HttpPost]
        public ActionResult FormCategoriasModificar(Categoria categoria)
        {
            if (categoria != null)
            {
                try
                {
                    Empleado usuL = (Empleado)Session["EmpLogueado"];
                    if (usuL == null)
                    {
                        return RedirectToAction("Ingresar", "Empleado");
                    }
                    // Valido categoria con sus restricciones
                    categoria.Validar();

                    //intento modificar Categoria en la bd
                    FabricaLogica.GetLCategoria().ModificarCategoria(categoria, usuL);

                    // Si anda bien se redirige a la lista
                    return RedirectToAction("FormCategoriaListar", "Categoria");
                }
                catch (Exception ex)
                {
                    // Muestra un mensaje de error si es que lo hay
                    ViewBag.Mensaje = ex.Message;
                    return View(categoria);
                }
            }
            else
            {
                ViewBag.Mensaje = "La categoría no es válida.";
                return View();
            }
        }
        [HttpGet]
        public ActionResult FormCategoriaConsultar(string CatCod)
        {
            try
            {

                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                // Obtener la categoría por su código

                Categoria categoria = FabricaLogica.GetLCategoria().BuscarCategoria(CatCod, usuL);

                if (categoria != null)
                {
                    // Si la categoría se encuentra, la pasamos a la vista
                    return View(categoria);
                }
                else
                {
                    // Si la categoría no existe, mostrar mensaje
                    ViewBag.Mensaje = "No existe la categoría con el código proporcionado.";
                    return View(); // Pasar un modelo vacío
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(); // En caso de error, pasar un modelo vacío
            }
        }
        [HttpGet]
        public ActionResult FormCategoriaEliminar(string CatCod)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                Categoria categoria = FabricaLogica.GetLCategoria().BuscarCategoria(CatCod, usuL);

                if (categoria != null)
                {
                   
                    return View(categoria);  // Pasamos el objeto Categoria a la vista
                }
                else
                {
                    ViewBag.Mensaje = "Categoria no encontrada.";
                    return View(new Categoria());  // Si no se encuentra, retornamos una vista vacía o con mensaje
                }
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Categoria());
            }
        }
        [HttpPost]
        public ActionResult FormCategoriaEliminar(Categoria C)
        {
            try
            {
                Empleado usuL = (Empleado)Session["EmpLogueado"];
                if (usuL == null)
                {
                    return RedirectToAction("Ingresar", "Empleado");
                }
                // Intentamos eliminar la categoría en la base de datos
                FabricaLogica.GetLCategoria().EliminarCategoria(C, usuL);

                // Mensaje de éxito
                ViewBag.Mensaje = "Eliminación Exitosa";

                // Redirigir a la lista de categorías después de la eliminación
                return RedirectToAction("FormCategoriaListar", "Categoria");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();  // Vuelve a la vista con el mensaje de error
            }
        }
    }
}
