using Microsoft.EntityFrameworkCore;
using EvenementenAPI.Models;

namespace EvenementenAPI.Data
{
    public class EventPlannerContext : DbContext
    {
        public EventPlannerContext(DbContextOptions<EventPlannerContext> options) : base(options) { }

        public DbSet<Event> Events { get; set; }
    }
}
