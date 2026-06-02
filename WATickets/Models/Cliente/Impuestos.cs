using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WATickets.Models.Cliente
{
    public class Impuestos
    {
        public int id { get; set; }
        public string Codigo { get; set; }
        public decimal Tarifa { get; set; }
        public bool Activo { get; set; }
    }
}