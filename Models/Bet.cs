using ApiBet.Models;

public class Bet
{
    public int Id { get; set; }
    public int UserId { get; set; } // Koppling till användaren
    public required User User { get; set; }  // Navigationsproperty

    public int GroupId { get; set; } // Koppling till gruppen
    public required Group Group { get; set; }  // Navigationsproperty

    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? Result { get; set; }
    public string? Status { get; set; }
}