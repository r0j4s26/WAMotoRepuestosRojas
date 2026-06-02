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
    public class CategoriasController : ApiController
    {
        ModelCliente db = new ModelCliente();



        public HttpResponseMessage GetAll([FromUri] Filtros filtro)
        {
            try
            {
                var Categorias = db.Categorias.ToList();


                return Request.CreateResponse(System.Net.HttpStatusCode.OK, Categorias);
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
        [Route("api/Categorias/Consultar")]
        public HttpResponseMessage GetOne([FromUri] int id)
        {
            try
            {
                Categorias condiciones = db.Categorias.Where(a => a.id == id).FirstOrDefault();


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
        [Route("api/Categorias/Insertar")]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Categorias condiciones)
        {
            try
            {
                Categorias Categoria = db.Categorias.Where(a => a.id == condiciones.id).FirstOrDefault();
                if (Categoria == null)
                {
                    Categoria = new Categorias();

                    Categoria.Nombre = condiciones.Nombre;
                    db.Categorias.Add(Categoria);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("Ya existe una categoria con este ID");
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
        [Route("api/Categorias/Actualizar")]
        [HttpPut]
        public HttpResponseMessage Put([FromBody] Categorias condiciones)
        {
            try
            {
                Categorias Categorias = db.Categorias.Where(a => a.id == condiciones.id).FirstOrDefault();
                if (Categorias != null)
                {
                    db.Entry(Categorias).State = System.Data.Entity.EntityState.Modified;
                    Categorias.Nombre = condiciones.Nombre;
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
        [Route("api/Categorias/Eliminar")]
        [HttpDelete]
        public HttpResponseMessage Delete([FromUri] int id)
        {
            try
            {
                Categorias Categorias = db.Categorias.Where(a => a.id == id).FirstOrDefault();
                if (Categorias != null)
                {
                    db.Categorias.Remove(Categorias);
                    db.SaveChanges();

                }
                else
                {
                    throw new Exception("No existe una categoria con este ID");
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