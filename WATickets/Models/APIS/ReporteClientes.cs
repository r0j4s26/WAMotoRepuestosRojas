using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WATickets.Models.APIS
{
    public class ReporteClientes
    {
        public decimal Totalizado { get; set; }
        public int idCliente { get; set; }
        public string Nombre { get; set; }
    }
}