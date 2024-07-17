using System;
using System.Collections.Generic;
using Caps_Project.DTOs.LoginDTOs;
using Caps_Project.DTOs.OrdenDTOs;
using Caps_Project.Models.View.DTOs.LoginDTOs;
using Microsoft.EntityFrameworkCore;
using Caps_Project.DTOs.InventarioDTOs;

namespace Caps_Project.Models;

public partial class DbCapsContext : DbContext
{
    public DbCapsContext()
    {
    }

    public DbCapsContext(DbContextOptions<DbCapsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EmpleadoActivo> EmpleadoActivos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<OrderReceipt> OrderReceipts { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<ProductPrice> ProductPrices { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<TempCarrito> TempCarritos { get; set; }

    public virtual DbSet<TipoEmpleado> TipoEmpleados { get; set; }

    public virtual DbSet<VwDatosUsuario> VwDatosUsuarios { get; set; }

    public virtual DbSet<VwListaEmpleado> VwListaEmpleados { get; set; }

   //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   //    => optionsBuilder.UseSqlServer("name=DevDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E466B3691");

            entity.ToTable("Empleado");

            entity.Property(e => e.IdEmpleado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ApMaterno)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ApPaterno)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.EmployeeTypeNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.EmployeeType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empleado__Employ__398D8EEE");
        });

        modelBuilder.Entity<EmpleadoActivo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Empleado__3214EC07EE576B0E");

            entity.ToTable("Empleado_Activo");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IdEmpleado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Turno)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.EmpleadoActivos)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Empleado___IdEmp__3C69FB99");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventar__3214EC070F46CF1A");

            entity.ToTable("Inventario", tb => tb.HasTrigger("TR_INVENTARY_QUANTITY"));

            entity.Property(e => e.EntryDate).HasColumnType("datetime");
            entity.Property(e => e.IdAdmin)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdAdminNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdAdmin)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventari__IdAdm__49C3F6B7");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventari__IdPro__4AB81AF0");
        });

        modelBuilder.Entity<OrderReceipt>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__OrderRec__C3905BCFBB940E96");

            entity.ToTable("OrderReceipt");

            entity.Property(e => e.IdEmpleado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.TotalPaid).HasColumnType("money");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.OrderReceipts)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderRece__IdEmp__46E78A0C");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__ProductC__CBD74706DDCCC2E6");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__Product___51E84262490C87B2");

            entity.ToTable("Product_Items", tb => tb.HasTrigger("TR_REDUCESTOCK"));

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductPriceId).HasColumnName("ProductPriceID");
            entity.Property(e => e.TicketOrderId).HasColumnName("TicketOrderID");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_I__Produ__4F7CD00D");

            entity.HasOne(d => d.ProductPrice).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.ProductPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_I__Produ__4D94879B");

            entity.HasOne(d => d.TicketOrder).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.TicketOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product_I__Ticke__4E88ABD4");
        });

        modelBuilder.Entity<ProductPrice>(entity =>
        {
            entity.HasKey(e => e.IdPrice).HasName("PK__ProductP__CDFAF3C278ED7BF6");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UnitPrice).HasColumnType("money");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductPr__Produ__440B1D61");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__098892100E4A102A");

            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.ProdName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdProdCategoryNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdProdCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__IdPro__412EB0B6");
        });

        modelBuilder.Entity<TempCarrito>(entity =>
        {
            entity.HasKey(e => e.IdItem).HasName("PK__TempCarr__51E8426261C66D48");

            entity.ToTable("TempCarrito");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductPriceId).HasColumnName("ProductPriceID");
        });

        modelBuilder.Entity<TipoEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdEmployeeType).HasName("PK__TipoEmpl__365B2211BE2C5F22");

            entity.ToTable("TipoEmpleado");

            entity.Property(e => e.EmpTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwDatosUsuario>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Datos_Usuario");

            entity.Property(e => e.EmpTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IdEmpleado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(302)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwListaEmpleado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ListaEmpleados");

            entity.Property(e => e.EmpTypeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.IdEmpleado)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(302)
                .IsUnicode(false);
            entity.Property(e => e.Turno)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }
	//public DbSet<vwDatosUsuarioDTO> VwDatosUsuarios { get; set; }
	public DbSet<ProductItemDTO> ProductItemsDTOs { get; set; }
	public DbSet<ProductCarritoDTO> ProductCarritoDTOs { get; set; }
	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

	public DbSet<Caps_Project.DTOs.InventarioDTOs.NuevoProductoDTO> NuevoProductoDTO { get; set; } = default!;
}
