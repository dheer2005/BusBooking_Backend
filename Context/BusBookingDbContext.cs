using BusBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace BusBooking.Context
{
    public class BusBookingDbContext : DbContext
    {
        public BusBookingDbContext(DbContextOptions<BusBookingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.FromLocation)
                .WithMany()
                .HasForeignKey(s => s.FromLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.ToLocation)
                .WithMany()
                .HasForeignKey(s => s.ToLocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
               .HasOne(b => b.User)
               .WithMany(u => u.Bookings)
               .HasForeignKey(b => b.CustId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bus>()
                .Property(b => b.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<BusRating> BusRatings { get; set; }    
        public DbSet<Feedback> Feedbacks { get; set; }  
        public DbSet<Location> locations { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> payments { get; set; }    
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StateWiseCities> StateWiseCities { get; set; }

    }
}
