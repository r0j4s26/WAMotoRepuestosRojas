using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WATickets.Models;
using WATickets.Models.Cliente;

namespace WATickets.Controllers
{
    public class ComprasController : ApiController
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
                var Compras = db.EncCompras.Select(a => new
                {
                    a.id,
                    a.idProveedor,
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
                    a.Estado,
                    a.Recurrente,

                    Detalle = db.DetCompras.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => (filtro.FechaInicial != time ? a.Fecha >= filtro.FechaInicial : true) && (filtro.FechaFinal != time ? a.Fecha <= filtro.FechaFinal : true)).ToList(); //Traemos el listado de productos


                if (filtro.Codigo1 > 0) // esto por ser integer
                {
                    Compras = Compras.Where(a => a.idProveedor == filtro.Codigo1).ToList(); // filtramos por lo que traiga el codigo1 
                }

                if (!string.IsNullOrEmpty(filtro.ItemCode))
                {
                    Compras = Compras.Where(a => a.Status == filtro.ItemCode).ToList();
                }



                Compras = Compras.Where(a => (filtro.pendientes == true ? a.Estado == "P" : false) || (filtro.rechazados == true ? a.Estado == "R" : false)
                || (filtro.aprobados == true ? a.Estado == "A" : false) || (filtro.parciales == true ? a.Estado == "S" : false) || (filtro.completos == true ? a.Estado == "C" : false)
                 || (filtro.cancelados == true ? a.Estado == "E" : false)
                ).ToList();



                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Compras);
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
        [Route("api/Compras/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                var Compra = db.EncCompras.Select(a => new
                {
                    a.id,
                    a.idProveedor,
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
                    a.Estado,
                    a.Recurrente,

                    Detalle = db.DetCompras.Where(b => b.idEncabezado == a.id).ToList()

                }).Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Compra);
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

        [Route("api/Compras/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Compras compra)
        {
            try
            {
                EncCompras Compra = db.EncCompras.Where(a => a.id == compra.id).FirstOrDefault();
                if (Compra == null)
                {
                    Compra = new EncCompras();
                    Compra.idProveedor = compra.idProveedor;
                    Compra.idCondPago = compra.idCondPago;
                    Compra.Fecha = DateTime.Now;
                    Compra.Comentarios = compra.Comentarios;
                    Compra.Subtotal = compra.Subtotal;
                    Compra.TotalImpuestos = compra.TotalImpuestos;
                    Compra.TotalDescuentos = compra.TotalDescuentos;
                    Compra.TotalCompra = compra.TotalCompra;
                    Compra.PorDescto = compra.PorDescto;
                    Compra.Moneda = compra.Moneda;
                    Compra.Status = "0";
                    Compra.Estado = "P";
                    Compra.Recurrente = false;
                    db.EncCompras.Add(Compra);
                    db.SaveChanges();

                    var i = 0;
                    foreach (var item in compra.Detalle)
                    {
                        DetCompras det = new DetCompras();
                        det.idEncabezado = Compra.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;
                        det.CantidadFaltante = item.Cantidad;
                        det.CantidadRecibidoAE = 0;
                        db.DetCompras.Add(det);
                        db.SaveChanges();
                        i++;
                    }
                }
                else
                {
                    throw new Exception("Ya existe una compra con este ID");
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
        [Route("api/Compras/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Compras compra)
        {
            var t = db.Database.BeginTransaction();
            try
            {
                EncCompras Compra = db.EncCompras.Where(a => a.id == compra.id).FirstOrDefault();
                if (Compra != null)
                {
                    db.Entry(Compra).State = EntityState.Modified;
                    Compra.idProveedor = compra.idProveedor;
                    Compra.idCondPago = compra.idCondPago;
                    Compra.Fecha = DateTime.Now;
                    Compra.Comentarios = compra.Comentarios;
                    Compra.Subtotal = compra.Subtotal;
                    Compra.TotalImpuestos = compra.TotalImpuestos;
                    Compra.TotalDescuentos = compra.TotalDescuentos;
                    Compra.TotalCompra = compra.TotalCompra;
                    Compra.PorDescto = compra.PorDescto;
                    Compra.Moneda = compra.Moneda;
                    Compra.Estado = "P";

                    db.SaveChanges();

                    var Detalles = db.DetCompras.Where(a => a.idEncabezado == Compra.id).ToList();

                    foreach (var item in Detalles)
                    {
                        db.DetCompras.Remove(item);
                        db.SaveChanges();
                    }


                    var i = 0;
                    foreach (var item in compra.Detalle)
                    {
                        DetCompras det = new DetCompras();
                        det.idEncabezado = Compra.id;
                        det.idProducto = item.idProducto;
                        det.NumLinea = i;
                        det.PorDescto = item.PorDescto;
                        det.PrecioUnitario = item.PrecioUnitario;
                        det.TotalImpuesto = item.TotalImpuesto;
                        det.Cantidad = item.Cantidad;
                        det.Descuento = item.Descuento;
                        det.TotalLinea = item.TotalLinea;
                        det.CantidadFaltante = item.Cantidad;
                        det.CantidadRecibidoAE = 0;

                        db.DetCompras.Add(det);
                        db.SaveChanges();
                        i++;
                    }



                    t.Commit();
                }
                else
                {
                    throw new Exception("NO existe una compra con este ID");
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
        [Route("api/Compras/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                var Compra = db.EncCompras.Where(a => a.id == id).FirstOrDefault();
                if (Compra != null)
                {
                    db.Entry(Compra).State = EntityState.Modified;


                    if (Compra.Status == "0")
                    {

                        Compra.Status = "1";

                    }
                    else
                    {

                        Compra.Status = "0";

                    }




                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("No existe una compra con este ID");
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

        [Route("api/Compras/Estado")]
        [HttpDelete]
        public HttpResponseMessage Aprobar([FromUri] int id, string status)
        {
            try
            {
                var Compra = db.EncCompras.Where(a => a.id == id).FirstOrDefault();
                if (Compra != null)
                {
                    db.Entry(Compra).State = EntityState.Modified;
                    Compra.Estado = status;
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("No existe una compra con este ID");
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


        [Route("api/Compras/Recibir")]
        [HttpPut]
        public HttpResponseMessage Recibir([FromBody] Compras compra)
        {
            var t = db.Database.BeginTransaction();
            try
            {
                EncCompras Compra = db.EncCompras.Where(a => a.id == compra.id).FirstOrDefault();
                if (Compra != null)
                {
                    var Detalles = db.DetCompras.Where(a => a.idEncabezado == Compra.id).ToList();

                    foreach (var detalle in compra.Detalle)
                    {

                        var Det = Detalles.Where(a => a.id == detalle.id).FirstOrDefault();
                        db.Entry(Det).State = EntityState.Modified;
                        Det.CantidadRecibidoAE += detalle.CantidadRecibidoAE;
                        Det.CantidadFaltante = detalle.Cantidad - Det.CantidadRecibidoAE;
                        db.SaveChanges();
                        var Producto = db.Productos.Where(a => a.id == Det.idProducto).FirstOrDefault();
                        if(Producto != null)
                        {
                            db.Entry(Producto).State = EntityState.Modified;
                            Producto.Stock += detalle.CantidadRecibidoAE;
                            db.SaveChanges();

                        }
                    }

                    db.Entry(Compra).State = EntityState.Modified;
                    Compra.Comentarios = compra.Comentarios;
                    if (db.DetCompras.Where(a => a.idEncabezado == Compra.id && a.CantidadFaltante > 0).ToList().Count() > 0)
                    {
                        Compra.Estado = "S";

                    }
                    else
                    {
                        Compra.Estado = "C";

                    }


                    db.SaveChanges();

                 




                    t.Commit();
                }
                else
                {
                    throw new Exception("NO existe una compra con este ID");
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

        [Route("api/Compras/Recurrente")]
        [HttpDelete]
        public HttpResponseMessage Recurrente([FromUri] int id)
        {
            try
            {
                var Compra = db.EncCompras.Where(a => a.id == id).FirstOrDefault();
                if (Compra != null)
                {
                    db.Entry(Compra).State = EntityState.Modified;
                    if (Compra.Recurrente)
                    {

                        Compra.Recurrente = false;

                    }
                    else
                    {

                        Compra.Recurrente = true;

                    }

                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe una compra con este ID");
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
        [Route("api/Compras/Cancelar")]
        [HttpDelete]
        public HttpResponseMessage Cancelar([FromUri] int id)
        {
            try
            {
                var Orden = db.EncCompras.Where(a => a.id == id).FirstOrDefault();
                if (Orden != null)
                {
                    db.Entry(Orden).State = EntityState.Modified;


                    Orden.Estado = "E";




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