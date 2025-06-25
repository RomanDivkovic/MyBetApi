using Microsoft.AspNetCore.Mvc;
using ApiBet.Models;
using ApiBet.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiBet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly BettingContext _context;
        public FightController(BettingContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Fight>> CreateFight(Fight fight)
        {
            _context.Fights.Add(fight);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFight), new { id = fight.Id }, fight);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Fight>> GetFight(int id)
        {
            var fight = await _context.Fights.FirstOrDefaultAsync(f => f.Id == id);
            if (fight == null) return NotFound();
            return fight;
        }
    }
}
