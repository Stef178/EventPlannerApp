namespace EventPlannerApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public bool IsPaid { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int ParticipantId { get; set; }
        public Participant? Participant { get; set; }

        public int? CashierId { get; set; }
        public Cashier? Cashier { get; set; }
    }
}
