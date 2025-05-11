using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MEF;
using System.Data.SqlClient;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    ProyectoFinal2024Entities contexto = null;

    List<Articulo> _TodosArt = null;
    List<Categoria> _TodasCat = null;
    List<Articulo> _FiltroArt = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Application["contexto"] == null)
                {
                    Application["contexto"] = contexto = new ProyectoFinal2024Entities();
                }

                _TodasCat = (from unaC in contexto.Categoria.ToList()
                             where unaC.CatActivo
                             select unaC).ToList();

                Session["Categorias"] = _TodasCat;

                _FiltroArt = (from unA in contexto.Articulo.ToList()
                              where unA.ArtFechVen >= DateTime.Now && unA.ArtActivo
                              orderby unA.ArtNom
                              select unA).ToList();

                Session["Articulos"] = Session["ArtFiltro"] = _TodosArt = _FiltroArt;

                Limpiar();
            }
            else
            {
                contexto = Application["contexto"] as ProyectoFinal2024Entities;
                _TodasCat = Session["Categorias"] as List<Categoria>;
                _TodosArt = Session["Articulos"] as List<Articulo>;
                _FiltroArt = Session["ArtFiltro"] as List<Articulo>;
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    private void Limpiar()
    {
        ddlCategorias.DataSource = _TodasCat;
        ddlCategorias.DataTextField = "CatNom";
        ddlCategorias.DataBind();
        ddlCategorias.Items.Insert(0, "Seleccione una Cat");
        ddlCategorias.SelectedIndex = 0;

        Session["ArtFiltro"] = _FiltroArt = _TodosArt;
        gvArtxVen.DataSource = _FiltroArt;
        gvArtxVen.DataBind();
        gvArtxVen.SelectedIndex = -1;

        txtNombre.Text = "";

        txtCodigo.Text = "";
        txtNombreArt.Text = "";
        txtTipo.Text = "";
        txtTamaño.Text = "";
        txtPrecio.Text = "";
        txtFecha.Text = "";
        txtCategoria.Text = "";

        LimpiarArt();
    }
    protected void LimpiarArt()
    {
        txtNombreArt.Text = "";
        txtCodigo.Text = "";
        txtTipo.Text = "";
        txtTamaño.Text = "";
        txtPrecio.Text = "";
        txtFecha.Text = "";
        txtCantidad.Text = "";
        txtCategoria.Text = "";
    }
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        Limpiar();
    }
    protected void btnFiltro_Click(object sender, EventArgs e)
    {
        try
        {
            List<MEF.Articulo> listaArt = _TodosArt;

            if (txtNombre.Text.Trim().Length > 0)
            {
                string nombre = txtNombre.Text;

                listaArt = (from unA in listaArt
                            where unA.ArtNom.Trim().ToLower().StartsWith(nombre.Trim().ToLower())
                            orderby unA.ArtNom
                            select unA).ToList();
            }

            if (ddlCategorias.SelectedIndex > 0)
            {
                Categoria unCat = _TodasCat[ddlCategorias.SelectedIndex - 1];

                listaArt = (from unA in listaArt
                            where unA.Categoria.CatCod == unCat.CatCod
                            orderby unA.ArtNom
                            select unA).ToList();
            }

            Session["ArtFiltro"] = _FiltroArt = listaArt;

            if (_FiltroArt.Count == 0)
            {
                lblError.Text = "No hay Articulos para dichos filtros";

                gvArtxVen.DataSource = null;
                gvArtxVen.DataBind();
            }
            else
            {
                lblError.Text = "";
                gvArtxVen.DataSource = listaArt;
                gvArtxVen.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void gvArtxVen_SelectedIndexChanged(object sender, EventArgs e)
    {
        LimpiarArt();

        try
        {
            int cant = 0;

            Session["ArtFiltro"] = _FiltroArt;

            int eleccion = (gvArtxVen.PageSize * gvArtxVen.PageIndex) + gvArtxVen.SelectedIndex;

            Articulo _art = _FiltroArt[eleccion];

            cant = (from unA in contexto.DetalleVenta.ToList()
                    where unA.Articulo.ArtCod == _art.ArtCod
                    select unA).Count();

            txtCodigo.Text = _art.ArtCod;
            txtNombreArt.Text = _art.ArtNom;
            txtTipo.Text = _art.ArtTipo;
            txtTamaño.Text = Convert.ToString(_art.ArtTam);
            txtPrecio.Text = Convert.ToString(_art.ArtPre);
            txtFecha.Text = Convert.ToString(_art.ArtFechVen);
            txtCantidad.Text = Convert.ToString(cant);
            txtCategoria.Text = _art.Categoria.CatNom;
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
    protected void gvArtxVen_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvArtxVen.PageIndex = e.NewPageIndex;
        gvArtxVen.DataSource = _FiltroArt;
        gvArtxVen.DataBind();
    }
}