using Newtonsoft.Json;
using System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
    [Authorize]
    public class CondicionesPagosController : ApiController
    {
        ModelCliente db = new ModelCliente();



        public HttpResponseMessage GetAll([FromUri] Filtros filtro)
        {
            try
            {
                var CondicionesPagos = db.CondicionesPagos.ToList();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, CondicionesPagos);
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
        [Route("api/CondicionesPagos/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                CondicionesPagos condiciones = db.CondicionesPagos.Where(a => a.id == id).FirstOrDefault();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, condiciones);
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
        [Route("api/CondicionesPagos/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] CondicionesPagos condiciones)
        {
            try
            {
                CondicionesPagos Condicion = db.CondicionesPagos.Where(a => a.id == condiciones.id).FirstOrDefault();
                if (Condicion == null)
                {
                    Condicion = new CondicionesPagos();

                    Condicion.Dias = condiciones.Dias;
                    Condicion.Nombre = condiciones.Nombre;
                    db.CondicionesPagos.Add(Condicion);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Ya existe una bodega con este ID");
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
        [Route("api/CondicionesPagos/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] CondicionesPagos condiciones)
        {
            try
            {
                CondicionesPagos CondicionesPagos = db.CondicionesPagos.Where(a => a.id == condiciones.id).FirstOrDefault();
                if (CondicionesPagos != null)
                {
                    db.Entry(CondicionesPagos).State = System.Data.Entity.EntityState.Modified;
                    CondicionesPagos.Dias = condiciones.Dias;
                    CondicionesPagos.Nombre = condiciones.Nombre;
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe una bodega" +
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
        [Route("api/CondicionesPagos/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                CondicionesPagos CondicionesPagos = db.CondicionesPagos.Where(a => a.id == id).FirstOrDefault();
                if (CondicionesPagos != null)
                {
                    db.CondicionesPagos.Remove(CondicionesPagos);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe una bodega con este ID");
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