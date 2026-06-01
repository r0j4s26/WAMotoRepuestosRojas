namespace WATickets.Models.Cliente
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelCliente : DbContext
    {
        public ModelCliente()
            : base("name=ModelCliente")
        {
        }

        public virtual DbSet<BitacoraErrores> BitacoraErrores { get; set; }
      



        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SeguridadModulos> SeguridadModulos { get; set; }
        public virtual DbSet<SeguridadRolesModulos> SeguridadRolesModulos { get; set; }

        public virtual DbSet<Usuarios> Usuarios { get; set; }


        public virtual DbSet<Parametros> Parametros { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<CondicionesPagos> CondicionesPagos { get; set; }
        public virtual DbSet<Barrios> Barrios { get; set; }
        public virtual DbSet<Cantones> Cantones { get; set; }
        public virtual DbSet<Distritos> Distritos { get; set; }
        public virtual DbSet<EncOrdenes> EncOrdenes { get; set; }
        public virtual DbSet<DetOrdenes> DetOrdenes { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Bodegas> Bodegas { get; set; }
        public virtual DbSet<EncDocumento> EncDocumento { get; set; }
        public virtual DbSet<DetDocumento> DetDocumento { get; set; }
        public virtual DbSet<Rutas> Rutas { get; set; }
        public virtual DbSet<EncCompras> EncCompras { get; set; }
        public virtual DbSet<DetCompras> DetCompras { get; set; }
        public virtual DbSet<CorreoEnvio> CorreoEnvio { get; set; }
        public virtual DbSet<Impuestos> Impuestos { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<EncOferta> EncOferta { get; set; }
        public virtual DbSet<DetOferta> DetOferta { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.StrackTrace)
                .IsUnicode(false);

            modelBuilder.Entity<BitacoraErrores>()
                .Property(e => e.JSON)
                .IsUnicode(false);



            modelBuilder.Entity<Roles>()
                .Property(e => e.NombreRol)
                .IsUnicode(false);

            modelBuilder.Entity<SeguridadModulos>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.NombreUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Clave)
                .IsUnicode(false);



        }
    }
}
