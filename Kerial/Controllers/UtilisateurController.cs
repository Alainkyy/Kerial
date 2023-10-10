using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kerial.Models;

namespace Kerial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly DbKerialContext _context;

        public UtilisateurController(DbKerialContext context)
        {
            _context = context;
        }

        // GET: api/Utilisateur
        [HttpGet("lister")]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateur()
        {
          if (_context.Utilisateur == null)
          {
              return NotFound();
          }
            return await _context.Utilisateur.ToListAsync();
        }

        // GET: api/Utilisateur/5
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Utilisateur>> GetUtilisateur(int id)
        {
          if (_context.Utilisateur == null)
          {
              return NotFound();
          }
            var utilisateur = await _context.Utilisateur.FindAsync(id);

            if (utilisateur == null)
            {
                return NotFound();
            }

            return utilisateur;
        }

        // PUT: api/Utilisateur/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("modifier/{id}")]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.idUtilisateur)
            {
                return BadRequest();
            }

            _context.Entry(utilisateur).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilisateurExists(id))
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

        // POST: api/Utilisateur
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("ajouter")]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
          if (_context.Utilisateur == null)
          {
              return Problem("Entity set 'DbKerialContext.Utilisateur'  is null.");
          }
            _context.Utilisateur.Add(utilisateur);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UtilisateurExists(utilisateur.idUtilisateur))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUtilisateur", new { id = utilisateur.idUtilisateur }, utilisateur);
        }

        // DELETE: api/Utilisateur/5
        [HttpDelete("supprimer/{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            if (_context.Utilisateur == null)
            {
                return NotFound();
            }
            var utilisateur = await _context.Utilisateur.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            _context.Utilisateur.Remove(utilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UtilisateurExists(int id)
        {
            return (_context.Utilisateur?.Any(e => e.idUtilisateur == id)).GetValueOrDefault();
        }
    }
}
