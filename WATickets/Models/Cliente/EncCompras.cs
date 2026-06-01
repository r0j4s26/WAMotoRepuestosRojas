using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WATickets.Models.Cliente
{
    [Table("EncCompras")]
    public class EncCompras
    {
        public int id { get; set; }
        public int idProveedor { get; set; }
        public int idCondPago { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentarios { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal TotalDescuentos { get; set; }
        public decimal TotalCompra { get; set; }
        public string Moneda { get; set; }
        public decimal PorDescto { get; set; }
        public string Status { get; set; }
        public string Estado { get; set; }
        public bool Recurrente { get; set; }

    }
}