using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shipfinity.Domain.Models;

namespace Shipfinity.DataAccess.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ReviewProduct> ProductReviews { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
        public DbSet<PaymentInfo> PaymentInfos { get; set; }

        public AppDbContext(DbContextOptions dbContextOptions) :
            base(dbContextOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.ProductOrders)
                .WithOne(p => p.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductOrders)
                .WithOne(p => p.Product)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.ProductReviews)
                .WithOne(rp => rp.Product)
                .HasForeignKey(rp => rp.ProductId);
           
            modelBuilder.Entity<ReviewProduct>()
                .HasOne(rp => rp.User)
                .WithMany(c => c.ReviewProducts)
                .HasForeignKey(rp => rp.UserId);

            modelBuilder.Entity<Address>()
                .HasMany(a => a.Users)
                .WithOne(c => c.Address)
                .HasForeignKey(c => c.AddressId);

            modelBuilder.Entity<PaymentInfo>()
                .HasMany(pi => pi.Orders)
                .WithOne(o => o.PaymentInfo)
                .HasForeignKey(o => o.PaymentInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentInfo>()
                .HasOne(pi => pi.User)
                .WithMany(c => c.PaymentInfos)
                .HasForeignKey(pi => pi.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentInfo>().HasIndex(p => p.CardNumber);
            modelBuilder.Entity<PaymentInfo>().HasIndex(p => p.ExpirationDate);
        }
    }
}
