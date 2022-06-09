using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication2.Data
{
    public partial class BookStoreDBContext : IdentityDbContext<APIUser>
    {
        public BookStoreDBContext()
        {
        }

        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=AURBLRLT-353\\MSSQLSERVER2019;Database=BookStoreDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Bio)
                    .HasMaxLength(250)
                    .IsFixedLength();

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__447D36EA4745F97C")
                    .IsUnique();

                entity.Property(e => e.AuthorId).HasColumnName("AuthorID");

                entity.Property(e => e.Image).HasMaxLength(50);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(50)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Summary).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = "0fceab49-2493-4df6-ad3a-82ca11467fce"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Id = "8cceac4d-ec33-4f9d-b6e4-29833083b4e7"
                }
                );

            var hasher = new PasswordHasher<APIUser>();  

            modelBuilder.Entity<APIUser>().HasData(
               new APIUser
               {
                   Id = "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab",
                   Email =  "admin@bookstore.com",
                   NormalizedEmail =    "ADMIN@BOOKSTORE.COM",
                   FirstName =  "System",
                   LastName =   "Admin",
                   PasswordHash = hasher.HashPassword(null,"Password@")

               },
               new APIUser
               {
                   Id = "90ec3121-3328-42ef-8117-1569cf3e70a7",
                   Email = "user@bookstore.com",
                   NormalizedEmail = "USER@BOOKSTORE.COM",
                   FirstName = "System",
                   LastName = "User",
                   PasswordHash = hasher.HashPassword(null, "Password@")
               }
               );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "8cceac4d-ec33-4f9d-b6e4-29833083b4e7",
                    UserId = "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0fceab49-2493-4df6-ad3a-82ca11467fce",
                    UserId = "b8f9b3f5-64d0-48a0-ba57-e04e57c545ab"
                }
                );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
