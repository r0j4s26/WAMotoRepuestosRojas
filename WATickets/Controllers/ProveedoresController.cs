using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WATickets.Models;
using WATickets.Models.Cliente;

namespace WATickets.Controllers
{
    public class ProveedoresController : ApiController
    {

        ModelCliente db = new ModelCliente();
        public HttpResponseMessage GetAll([FromUri] Filtros filtro)
        {
            try
            {
                var Proveedores = db.Proveedores.ToList();

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Proveedores);
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
        [Route("api/Proveedores/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                Proveedores proveedores = db.Proveedores.Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, proveedores);
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
        [Route("api/Proveedores/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Proveedores proveedores)
        {
            try
            {
                Proveedores Proveedor = db.Proveedores.Where(a => a.id == proveedores.id || a.Cedula == proveedores.Cedula).FirstOrDefault();
                if (Proveedor == null)
                {
                    Proveedor = new Proveedores();
                    Proveedor.id = proveedores.id;
                    Proveedor.Nombre = proveedores.Nombre;
                    Proveedor.Cedula = proveedores.Cedula;
                    Proveedor.Telefono = proveedores.Telefono;
                    Proveedor.Provincia = proveedores.Provincia;
                    Proveedor.Canton = proveedores.Canton;
                    Proveedor.Distrito = proveedores.Distrito;
                    Proveedor.Barrio = proveedores.Barrio;
                    Proveedor.Sennas = proveedores.Sennas;
                    Proveedor.TipoCedula = proveedores.TipoCedula;
                    Proveedor.Email = proveedores.Email;
                    Proveedor.idCondicionPago = db.CondicionesPagos.Where(a => a.Dias == 0 && a.Nombre == "Contado").FirstOrDefault().id;
                    Proveedor.Activo = true;
                    db.Proveedores.Add(Proveedor);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Ya existe un proveedor con esta Cédula");
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
        [Route("api/Proveedores/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Proveedores proveedores)
        {
            try
            {
                Proveedores Proveedor = db.Proveedores.Where(a => a.id == proveedores.id).FirstOrDefault();
                if (Proveedor != null)
                {
                    db.Entry(Proveedor).State = System.Data.Entity.EntityState.Modified;
                    Proveedor.Nombre = proveedores.Nombre;
                    Proveedor.Cedula = proveedores.Cedula;
                    Proveedor.Telefono = proveedores.Telefono;
                    Proveedor.Provincia = proveedores.Provincia;
                    Proveedor.Canton = proveedores.Canton;
                    Proveedor.Distrito = proveedores.Distrito;
                    Proveedor.Barrio = proveedores.Barrio;
                    Proveedor.Sennas = proveedores.Sennas;
                    Proveedor.TipoCedula = proveedores.TipoCedula;
                    Proveedor.Email = proveedores.Email;
                    Proveedor.idCondicionPago = proveedores.idCondicionPago;
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe un proveedor" +
                        " con este ID");
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
        [Route("api/Proveedores/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                Proveedores Proveedores = db.Proveedores.Where(a => a.id == id).FirstOrDefault();
                if (Proveedores != null)
                {
                    db.Proveedores.Remove(Proveedores);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe un proveedor con este ID");
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
    }
}