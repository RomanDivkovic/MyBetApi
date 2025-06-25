namespace ApiBet.Models
{
    public class Fight
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public required Event Event { get; set; }
        public required string FighterA { get; set; }
        public required string FighterB { get; set; }
        public required string FighterARecord { get; set; }
        public required string FighterBRecord { get; set; }
        public required string FighterAStats { get; set; }
        public required string FighterBStats { get; set; }
    }
}
