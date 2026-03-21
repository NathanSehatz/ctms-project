namespace CTMS.Models;

public class Reservation
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int UserId { get; set; }
    public int TicketTypeId { get; set; }
}