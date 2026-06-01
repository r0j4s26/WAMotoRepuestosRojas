using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WATickets.Models;
using WATickets.Models.Cliente;

namespace WATickets.Controllers
{
    public class DocumentosController : ApiController
    {
        ModelCliente db = new ModelCliente();

        public HttpResponseMessage GetAll([FromUri] Filtros filtro)
        {
            try
            {

                var time = new DateTime(); // 01-01-0001
                if (filtro.FechaFinal != time)
                {
                    filtro.FechaInicial = filtro.FechaInicial.Date;
                    filtro.FechaFinal = filtro.FechaFinal.AddDays(1);
                }
                var Documentos = db.EncDocumento.Select(a => new
                {
                    a.id,
                    a.idCliente,
                    a.Fecha,
                    a.Comentarios,
                    a.Subtotal,
                    a.TotalImpuestos,
                    a.TotalDescuentos,
                    a.TotalCompra,
                    a.PorDescto,
                    a.Moneda,
                    a.idCondPago,
                    a.idRuta,

                    Detalle = db.DetDocumento.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => (filtro.FechaInicial != time ? a.Fecha >= filtro.FechaInicial : true) && (filtro.FechaFinal != time ? a.Fecha <= filtro.FechaFinal : true)).ToList(); //Traemos el listado de productos


                if (filtro.Codigo1 > 0) // esto por ser integer
                {
                    Documentos = Documentos.Where(a => a.idCliente == filtro.Codigo1).ToList(); // filtramos por lo que traiga el codigo1 
                }





                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Documentos);
            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StrackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                be.JSON = JsonConvert.SerializeObject(ex);
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex);

            }


        }
        [Route("api/Documentos/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                var Documento = db.EncDocumento.Select(a => new
                {
                    a.id,
                    a.idCliente,
                    a.Fecha,
                    a.Comentarios,
                    a.Subtotal,
                    a.TotalImpuestos,
                    a.TotalDescuentos,
                    a.TotalCompra,
                    a.PorDescto,
                    a.Moneda,
                    a.idCondPago,
                    a.idRuta,

                    Detalle = db.DetDocumento.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Documento);
            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StrackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                be.JSON = JsonConvert.SerializeObject(ex);
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex);

            }

        }

        [Route("api/Documentos/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Documentos documento)
        {
            try
            {
                EncDocumento Documento = db.EncDocumento.Where(a => a.id == documento.id).FirstOrDefault();
                if (Documento == null)
                {
                    Documento = new EncDocumento();
                    Documento.idCliente = documento.idCliente;
                    Documento.idCondPago = documento.idCondPago;
                    Documento.idRuta = documento.idRuta;
                    Documento.Fecha = DateTime.Now;
                    Documento.Comentarios = documento.Comentarios;
                    Documento.Subtotal = documento.Subtotal;
                    Documento.TotalImpuestos = documento.TotalImpuestos;
                    Documento.TotalDescuentos = documento.TotalDescuentos;
                    Documento.TotalCompra = documento.TotalCompra;
                    Documento.PorDescto = documento.PorDescto;
                    Documento.Moneda = documento.Moneda;
                    Documento.BaseEntry = documento.BaseEntry;
                    db.EncDocumento.Add(Documento);
                    db.SaveChanges();

                    var i = 0;
                    foreach (var item in documento.Detalle)
                    {
                        DetDocumento det = new DetDocumento();
                        det.idEncabezado = Documento.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;

                        db.DetDocumento.Add(det);
                        db.SaveChanges();
                        i++;

                        var prod = db.Productos.Where(a => a.id == item.idProducto).FirstOrDefault();
                        if (prod != null)
                        {

                            db.Entry(prod).State = EntityState.Modified;
                            prod.Stock -= item.Cantidad;
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    throw new Exception("Ya existe una documento con este ID");
                }
                documento.id = Documento.id;
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, documento);
            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StrackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                be.JSON = JsonConvert.SerializeObject(ex);
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, ex);
            }
        }
        [Route("api/Documentos/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Documentos documento)
        {
            var t = db.Database.BeginTransaction();
            try
            {
                EncDocumento Documento = db.EncDocumento.Where(a => a.id == documento.id).FirstOrDefault();
                if (Documento != null)
                {
                    db.Entry(Documento).State = EntityState.Modified;
                    Documento.idCliente = documento.idCliente;
                    Documento.idCondPago = documento.idCondPago;
                    Documento.idRuta = documento.idRuta;
                    Documento.Fecha = DateTime.Now;
                    Documento.Comentarios = documento.Comentarios;
                    Documento.Subtotal = documento.Subtotal;
                    Documento.TotalImpuestos = documento.TotalImpuestos;
                    Documento.TotalDescuentos = documento.TotalDescuentos;
                    Documento.TotalCompra = documento.TotalCompra;
                    Documento.PorDescto = documento.PorDescto;
                    Documento.Moneda = documento.Moneda;


                    db.SaveChanges();

                    var Detalles = db.DetDocumento.Where(a => a.idEncabezado == Documento.id).ToList();

                    foreach (var item in Detalles)
                    {
                        db.DetDocumento.Remove(item);
                        db.SaveChanges();
                    }


                    var i = 0;
                    foreach (var item in documento.Detalle)
                    {
                        DetDocumento det = new DetDocumento();
                        det.idEncabezado = Documento.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;

                        db.DetDocumento.Add(det);
                        db.SaveChanges();
                        i++;
                    }



                    t.Commit();
                }
                else
                {
                    throw new Exception("NO existe una documento con este ID");
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                t.Rollback();
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StrackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                be.JSON = JsonConvert.SerializeObject(ex);
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, be);
            }
        }
        //Reenviar correo
        [HttpGet]
        [Route("api/Documentos/Reenvio")]
        public HttpResponseMessage GeCorreo([FromUri]int code, string correos)
        {
            try
            {



                var Orden = db.EncDocumento.Where(a => a.id == code).FirstOrDefault();


                if (Orden == null)
                {
                    throw new Exception("Esta Orden no se encuentra registrado");
                }
                ////Enviar Correo
                ///
                try
                {



                    Parametros parametros = db.Parametros.FirstOrDefault();
                    var CorreoEnvio = db.CorreoEnvio.FirstOrDefault();



                    var Ruta = db.Rutas.Where(a => a.id == Orden.idRuta).FirstOrDefault() == null ? "0" : db.Rutas.Where(a => a.id == Orden.idRuta).FirstOrDefault().Nombre;

                    var Cliente = db.Clientes.Where(a => a.id == Orden.idCliente).FirstOrDefault();
                    var Condicion = db.CondicionesPagos.Where(a => a.id == Orden.idCondPago).FirstOrDefault();
                    var Detalle = db.DetDocumento.Where(a => a.idEncabezado == code).ToList();
                    var resp = false;





                    try
                    {
                        var html = @"<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css' />
</head>
<body>
    <div class='row'>
        <div class='col-sm-3'></div>
        <div class='col-sm-6' style='text-justify: center;'>
            <p>Estimado usuario por este medio se le comunica que existe una actualización en una de las ordenes de venta, abajo encontrará información más detallada:</p>
        </div>
        <div class='col-sm-4'> </div>

        <div class='col-sm-3'><p><b># Orden</b>: @ID</p></div>
        <div class='col-sm-3'><p><b>Cliente:</b> @Cliente</p></div>
        <div class='col-sm-4'> </div>
        <div class='col-sm-3'><p><b>Moneda:</b> <b style='text-decoration: underline'> @Moneda </b></p></div>
        <div class='col-sm-4'> </div>
        <div class='col-sm-3'><p><b>Condición de pago:</b> @CondicionPago</p></div>
        <div class='col-sm-3'><p><b>Ruta:</b> @Ruta</p></div>

        <center>
            <div class='col-sm-6'>
                <table style='font-family: Arial, sans-serif; border-collapse: collapse; width: 80%; margin-bottom: 20px;'>
                    <thead>
                        <tr>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>#</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Producto</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Cantidad</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Precio Unitario</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>% Descuento</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Descuento</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Impuesto</th>
                            <th style='background-color: #f2f2f2; color: #444; padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>Total</th>
                        </tr>
                    </thead>
                    <tbody>
                        @INYECTADO
                    </tbody>
                </table>
            </div>
        </center>
    </div>
</body>
</html>";


                        html = html.Replace("@ID", Orden.id.ToString());
                        html = html.Replace("@Cliente", Cliente.id.ToString() + "-" + Cliente.Nombre.ToString());

                        html = html.Replace("@Fecha", Orden.Fecha.ToString("dd/MM/yyyy"));
                        html = html.Replace("@Moneda", Orden.Moneda.ToString());
                        html = html.Replace("@CondicionPago", Condicion.Nombre.ToString());
                        html = html.Replace("@Ruta", Ruta.ToString());




                        var htmlDetalle = "";
                        foreach (var itemDetalle in Detalle)
                        {
                            var Producto = db.Productos.Where(a => a.id == itemDetalle.idProducto).FirstOrDefault();
                            htmlDetalle += "<tr> <td style='padding: 8px; text-align: left; font-weight: bold; border-bottom: 1px solid #ddd;'>" + itemDetalle.NumLinea + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + Producto.Codigo + "-" + Producto.Nombre + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.Cantidad + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.PrecioUnitario + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.PorDescto + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.Descuento + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.TotalImpuesto + "</td>" +
                   "<td style='padding: 8px; text-align: left; border-bottom: 1px solid #ddd;'>" + itemDetalle.TotalLinea + "</td></tr>";
                        }
                        html = html.Replace("@INYECTADO", htmlDetalle);



                        resp = G.SendV2(correos, "", "", CorreoEnvio.RecepcionEmail, "Mercalab", "Orden de Venta #:" + " " + Orden.id, html, CorreoEnvio.RecepcionHostName, CorreoEnvio.EnvioPort, CorreoEnvio.RecepcionUseSSL, CorreoEnvio.RecepcionEmail, CorreoEnvio.RecepcionPassword);



                        if (!resp)
                        {
                            throw new Exception("No se ha podido enviar el correo a " + correos);
                        }
                    }
                    catch (Exception ex)
                    {

                        BitacoraErrores be = new BitacoraErrores();

                        be.Descripcion = ex.Message;
                        be.StrackTrace = ex.StackTrace;
                        be.Fecha = DateTime.Now;

                        db.BitacoraErrores.Add(be);
                        db.SaveChanges();
                    }





                }
                catch (Exception ex)
                {
                    BitacoraErrores be = new BitacoraErrores();

                    be.Descripcion = ex.Message;
                    be.StrackTrace = ex.StackTrace;
                    be.Fecha = DateTime.Now;

                    db.BitacoraErrores.Add(be);
                    db.SaveChanges();
                }


                return Request.CreateResponse(HttpStatusCode.OK, Orden);
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}