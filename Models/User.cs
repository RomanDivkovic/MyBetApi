using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiBet.Models
{
  [Index(nameof(Email), IsUnique = true)]
  public class User
  {
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? PhoneNumber { get; set; }

    // Relationer
    public ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    public ICollection<GroupInvitation> Invitations { get; set; } = new List<GroupInvitation>();
    public List<Bet> Bets { get; set; } = new List<Bet>();
  }
}
