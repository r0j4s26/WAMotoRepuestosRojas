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
    public class OrdenesController : ApiController
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
                var Ordenes = db.EncOrdenes.Select(a => new
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
                    a.Status,

                    Detalle = db.DetOrdenes.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => (filtro.FechaInicial != time ? a.Fecha >= filtro.FechaInicial : true) && (filtro.FechaFinal != time ? a.Fecha <= filtro.FechaFinal : true)).ToList(); //Traemos el listado de productos


                if (filtro.Codigo1 > 0) // esto por ser integer
                {
                    Ordenes = Ordenes.Where(a => a.idCliente == filtro.Codigo1).ToList(); // filtramos por lo que traiga el codigo1 
                }

                if (!string.IsNullOrEmpty(filtro.ItemCode))
                {
                    Ordenes = Ordenes.Where(a => a.Status == filtro.ItemCode).ToList();
                }



                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Ordenes);
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
        [Route("api/Ordenes/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                var Orden = db.EncOrdenes.Select(a => new
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
                    a.Status,

                    Detalle = db.DetOrdenes.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Orden);
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

        [Route("api/Ordenes/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Ordenes orden)
        {
            try
            {
                EncOrdenes Orden = db.EncOrdenes.Where(a => a.id == orden.id).FirstOrDefault();
                if (Orden == null)
                {
                    Orden = new EncOrdenes();
                    Orden.idCliente = orden.idCliente;
                    Orden.idCondPago = orden.idCondPago;
                    Orden.Fecha = DateTime.Now;
                    Orden.Comentarios = orden.Comentarios;
                    Orden.Subtotal = orden.Subtotal;
                    Orden.TotalImpuestos = orden.TotalImpuestos;
                    Orden.TotalDescuentos = orden.TotalDescuentos;
                    Orden.TotalCompra = orden.TotalCompra;
                    Orden.PorDescto = orden.PorDescto;
                    Orden.Moneda = orden.Moneda;
                    Orden.Status = "0";
                    db.EncOrdenes.Add(Orden);
                    db.SaveChanges();

                    var i = 0;
                    foreach (var item in orden.Detalle)
                    {
                        DetOrdenes det = new DetOrdenes();
                        det.idEncabezado = Orden.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;

                        db.DetOrdenes.Add(det);
                        db.SaveChanges();
                        i++;
                    }
                }
                else
                {
                    throw new Exception("Ya existe una orden con este ID");
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
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
        [Route("api/Ordenes/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Ordenes orden)
        {
            var t = db.Database.BeginTransaction();
            try
            {
                EncOrdenes Orden = db.EncOrdenes.Where(a => a.id == orden.id).FirstOrDefault();
                if (Orden != null)
                {
                    db.Entry(Orden).State = EntityState.Modified;
                    Orden.idCliente = orden.idCliente;
                    Orden.idCondPago = orden.idCondPago;
                    Orden.Fecha = DateTime.Now;
                    Orden.Comentarios = orden.Comentarios;
                    Orden.Subtotal = orden.Subtotal;
                    Orden.TotalImpuestos = orden.TotalImpuestos;
                    Orden.TotalDescuentos = orden.TotalDescuentos;
                    Orden.TotalCompra = orden.TotalCompra;
                    Orden.PorDescto = orden.PorDescto;
                    Orden.Moneda = orden.Moneda;


                    db.SaveChanges();

                    var Detalles = db.DetOrdenes.Where(a => a.idEncabezado == Orden.id).ToList();

                    foreach (var item in Detalles)
                    {
                        db.DetOrdenes.Remove(item);
                        db.SaveChanges();
                    }


                    var i = 0;
                    foreach (var item in orden.Detalle)
                    {
                        DetOrdenes det = new DetOrdenes();
                        det.idEncabezado = Orden.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;

                        db.DetOrdenes.Add(det);
                        db.SaveChanges();
                        i++;
                    }



                    t.Commit();
                }
                else
                {
                    throw new Exception("NO existe una orden con este ID");
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
        [Route("api/Ordenes/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                var Orden = db.EncOrdenes.Where(a => a.id == id).FirstOrDefault();
                if (Orden != null)
                {
                    db.Entry(Orden).State = EntityState.Modified;


                    if (Orden.Status == "0")
                    {

                        Orden.Status = "1";

                    }
                    else
                    {

                        Orden.Status = "0";

                    }




                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("No existe una orden con este ID");
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK);
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

                return Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError, be);
            }
        }
 
    }
}