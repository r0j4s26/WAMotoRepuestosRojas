using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WATickets.Models.Cliente
{
    public class Bodegas
    {
        public int id { get; set; }



        [StringLength(500)]
        public string Nombre { get; set; }
    }
}