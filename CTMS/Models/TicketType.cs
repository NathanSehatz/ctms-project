namespace CTMS.Models;

public class TicketType
{
    public int Id { get; set; }
    public string Name { get; set; } // VIP, General
    public decimal Price { get; set; }
    public int QuantityAvailable { get; set; }

    public int ConcertId { get; set; }
    public Concert Concert { get; set; }
}