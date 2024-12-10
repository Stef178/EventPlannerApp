using EventPlannerApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerApp.Data
{
    // DbContext class
    public class EventPlannerContext : DbContext
    {
        public EventPlannerContext(DbContextOptions<EventPlannerContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Organisor> Organisors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Cashier> Cashiers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organisor)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganisorId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tickets)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Participant)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.ParticipantId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Cashier)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CashierId);
        }
    }
}
