using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WATickets.Models.Cliente
{
    public class Parametros
    {
        public int id { get; set; }
        public string SQLReporteVentas { get; set; }
        public string SQLReporteClientes { get; set; }
        public string SQLReporteProductos { get; set; }
        public string SQLReporteStocks { get; set; }
    }
}