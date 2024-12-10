using System.Net.Sockets;

namespace EventPlannerApp.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public int MaxParticipants { get; set; }
        public required string Description { get; set; }
        public string? ImageUrl { get; set; }

        public int OrganisorId { get; set; }
        public Organisor? Organisor { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }

}
