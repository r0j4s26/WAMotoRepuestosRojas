using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WATickets.Models.Cliente
{
    public class Ordenes
    {
        public int id { get; set; }
        public int idCliente { get; set; }
        public int idCondPago { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalCompra { get; set; }
        public string Moneda { get; set; }
        public decimal PorDescto { get; set; }
        public List<DetOrdenes> Detalle { get; set; }
        public string Status { get; set; }
    }
}