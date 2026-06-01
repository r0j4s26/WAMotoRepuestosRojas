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
    public class ClientesController : ApiController
    {

        ModelCliente db = new ModelCliente();
        public HttpResponseMessage GetAll([FromUri] Filtros filtro)
        {
            try
            {
                var Clientes = db.Clientes.ToList();

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Clientes);
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
        [Route("api/Clientes/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                Clientes clientes = db.Clientes.Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, clientes);
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
        [Route("api/Clientes/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Clientes clientes)
        {
            try
            {
                Clientes Cliente = db.Clientes.Where(a => a.id == clientes.id || a.Cedula == clientes.Cedula).FirstOrDefault();
                if (Cliente == null)
                {
                    Cliente = new Clientes();
                    Cliente.id = clientes.id;
                    Cliente.idCondicionPago = db.CondicionesPagos.Where(a => a.Dias == 0 && a.Nombre == "Contado").FirstOrDefault().id;
                    Cliente.Nombre = clientes.Nombre;
                    Cliente.Cedula = clientes.Cedula;
                    Cliente.Telefono = clientes.Telefono;
                    Cliente.Provincia = clientes.Provincia;
                    Cliente.Canton = clientes.Canton;
                    Cliente.Distrito = clientes.Distrito;
                    Cliente.Barrio = clientes.Barrio;
                    Cliente.Sennas = clientes.Sennas;
                    Cliente.TipoCedula = clientes.TipoCedula;
                    Cliente.Email = clientes.Email;
                    Cliente.Activo = true;
                    db.Clientes.Add(Cliente);
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
        [Route("api/Clientes/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Clientes clientes)
        {
            try
            {
                Clientes Cliente = db.Clientes.Where(a => a.id == clientes.id).FirstOrDefault();
                if (Cliente != null)
                {
                    db.Entry(Cliente).State = System.Data.Entity.EntityState.Modified;
                    Cliente.Nombre = clientes.Nombre;
                    Cliente.idCondicionPago = clientes.idCondicionPago;
                    Cliente.Cedula = clientes.Cedula;
                    Cliente.Telefono = clientes.Telefono;
                    Cliente.Provincia = clientes.Provincia;
                    Cliente.Canton = clientes.Canton;
                    Cliente.Distrito = clientes.Distrito;
                    Cliente.Barrio = clientes.Barrio;
                    Cliente.Sennas = clientes.Sennas;
                    Cliente.TipoCedula = clientes.TipoCedula;
                    Cliente.Email = clientes.Email;
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
        [Route("api/Clientes/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                Clientes Clientes = db.Clientes.Where(a => a.id == id).FirstOrDefault();
                if (Clientes != null)
                {
                    db.Clientes.Remove(Clientes);
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