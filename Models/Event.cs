using System.Collections.Generic;

namespace ApiBet.Models
{
    public class Event
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int GroupId { get; set; }
        public required Group Group { get; set; }
        public List<Fight> Fights { get; set; } = new List<Fight>();
    }
}
