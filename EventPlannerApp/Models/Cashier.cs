namespace EventPlannerApp.Models
{
    public class Cashier
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }

}
