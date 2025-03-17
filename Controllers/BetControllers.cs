using Microsoft.AspNetCore.Authorization;  // För att använda [Authorize]
using Microsoft.AspNetCore.Mvc;
using ApiBet.Data;
using ApiBet.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;  // För att hämta användarens ID från token

namespace ApiBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetsController : ControllerBase
    {
        private readonly BettingContext _context;

        public BetsController(BettingContext context)
        {
            _context = context;
        }

        // GET: api/Bets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bet>>> GetBets()
        {
            return await _context.Bets.Include(b => b.User).Include(b => b.Group).ToListAsync();
        }

        // GET: api/Bets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bet>> GetBet(int id)
        {
            var bet = await _context.Bets.Include(b => b.User)
                                         .Include(b => b.Group)
                                         .FirstOrDefaultAsync(b => b.Id == id);

            if (bet == null)
            {
                return NotFound();
            }

            return bet;
        }

        // POST: api/Bets (Endast inloggade användare kan skapa bets)
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Bet>> CreateBet(Bet bet)
        {
            // Hämta användarens ID från JWT-token
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            bet.UserId = userId;  // Koppla bettet till den inloggade användaren

            // Kontrollera att användaren är medlem i gruppen
            var isMember = await _context.UserGroups
                .AnyAsync(ug => ug.UserId == userId && ug.GroupId == bet.GroupId);

            if (!isMember)
            {
                return BadRequest("Du är inte medlem i denna grupp.");
            }

            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBet), new { id = bet.Id }, bet);
        }

        // PUT: api/Bets/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBet(int id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest();
            }

            _context.Entry(bet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bets.Any(b => b.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Bets/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBet(int id)
        {
            var bet = await _context.Bets.FindAsync(id);
            if (bet == null)
            {
                return NotFound();
            }

            // Säkerställ att endast skaparen kan ta bort bettet
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (bet.UserId != userId)
            {
                return Forbid("Du kan bara ta bort bets som du själv skapat.");
            }

            _context.Bets.Remove(bet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}