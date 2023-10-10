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
    public class DepenseController : ControllerBase
    {
        private readonly DbKerialContext _context;
        public DepenseController(DbKerialContext context)
        {
            _context = context;
        }

        // Création d'objets Utilisateur simulés pour simuler une connexion
        private Utilisateur utilisateur1 = new Utilisateur
        {
            idUtilisateur = 0,
            nomDeFamille = "Stark",
            prenom = "Anthony",
            devise = "USD"
        };

        private Utilisateur utilisateur2 = new Utilisateur
        {
            idUtilisateur = 1,
            nomDeFamille = "Romanova",
            prenom = "Natasha",
            devise = "RUB"
        };

        // GET: api/Depense
        [HttpGet("lister")]
        public async Task<ActionResult<IEnumerable<object>>> GetDepenseAvecUtilisateur()
        {
            // Simuler l'utilisateur connecté (utilisateur1 ou utilisateur2)
            Utilisateur utilisateurConnecte = utilisateur1; // Vous pouvez choisir l'utilisateur simulé ici

            var depensesAvecUtilisateur = await _context.Depense
                .Where(depense => depense.idUtilisateur == utilisateurConnecte.idUtilisateur)
                .Select(depense => new
                {
                    depense.idDepense,
                    depense.idUtilisateur,
                    depense.date,
                    depense.nature,
                    depense.montant,
                    depense.devise,
                    depense.commentaire,
                    NomUtilisateur = $"{utilisateurConnecte.prenom} {utilisateurConnecte.nomDeFamille}"
                })
                .ToListAsync();

            return depensesAvecUtilisateur;
        }
        // GET: api/Depense/5
        [HttpGet("detail/{id}")]
        public async Task<ActionResult<Depense>> GetDepense(int id)
        {
          if (_context.Depense == null)
          {
              return NotFound();
          }
            var depense = await _context.Depense.FindAsync(id);
            if (depense == null)
            {
                return NotFound();
            }
            return depense;
        }
        // PUT: api/Depense/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("modifier/{id}")]
        public async Task<IActionResult> PutDepense(int id, Depense depense)
        {
            if (id != depense.idDepense)
            {
                return BadRequest();
            }
            _context.Entry(depense).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepenseExists(id))
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
        [HttpPost("ajouter")]
        public async Task<ActionResult<Depense>> PostDepense(Depense depense)
        {
            // Simulation Connexion
            Utilisateur utilisateurConnecte = utilisateur1;
            // Utilisateur utilisateurConnecte = utilisateur2;

            // Vérification si l'idUtilisateur de la dépense est différent de celui de l'utilisateur connecté
            if (depense.idUtilisateur != utilisateurConnecte.idUtilisateur)
            {
                return BadRequest("L'idUtilisateur de la dépense doit être le même que celui de l'utilisateur connecté.");
            }
            // Vérification si la date de la dépense est dans le futur
            if (depense.date > DateTime.Now)
            {
                return BadRequest("La date de la dépense ne peut pas être dans le futur.");
            }
            // Vérification si la date de la dépense est datée de plus de 3 mois
            DateTime troisMoisAvant = DateTime.Now.AddMonths(-3);
            if (depense.date < troisMoisAvant)
            {
                return BadRequest("La dépense est datée de plus de 3 mois en arrière.");
            }
            // Vérification si "nature" est l'une des trois options valides
            if (depense.nature != "Restaurant" && depense.nature != "Hotel" && depense.nature != "Misc")
            {
                return BadRequest("La valeur de 'nature' n'est pas valide. Les options valides sont : Restaurant, Hotel, Misc.");
            }
            // Vérification si le commentaire est obligatoire
            if (string.IsNullOrWhiteSpace(depense.commentaire))
            {
                return BadRequest("Le commentaire est obligatoire.");
            }
            // Vérification si un utilisateur a déjà déclaré la même dépense (même date et même montant)
            if (_context.Depense.Any(d => d.idUtilisateur == utilisateurConnecte.idUtilisateur && d.date == depense.date && d.montant == depense.montant))
            {
                return Conflict("Un utilisateur a déjà déclaré la même dépense (même date et même montant).");
            }
            // Vérification si la devise de la dépense est identique à celle de l'utilisateur connecté
            if (utilisateurConnecte.devise != depense.devise)
            {
                return BadRequest("La devise de la dépense doit être identique à celle de l'utilisateur.");
            }
            _context.Depense.Add(depense);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DepenseExists(depense.idDepense))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetDepense", new { id = depense.idDepense }, depense);
        }
        // SORT: api/Depense/
        [HttpGet("tri-par-montant")]
        public ActionResult<IEnumerable<Depense>> GetDepensesTrieesParMontant()
        {
            var depensesTriees = _context.Depense.OrderBy(d => d.montant).ToList();
            return depensesTriees;
        }
        // SORT: api/Depense/
        [HttpGet("tri-par-date")]
        public ActionResult<IEnumerable<Depense>> GetDepensesTrieesParDate()
        {
            var depensesTriees = _context.Depense.OrderBy(d => d.date).ToList();
            return depensesTriees;
        }
        // DELETE: api/Depense/5
        [HttpDelete("supprimer/{id}")]
        public async Task<IActionResult> DeleteDepense(int id)
        {
            if (_context.Depense == null)
            {
                return NotFound();
            }
            var depense = await _context.Depense.FindAsync(id);
            if (depense == null)
            {
                return NotFound();
            }
            _context.Depense.Remove(depense);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool DepenseExists(int id)
        {
            return (_context.Depense?.Any(e => e.idDepense == id)).GetValueOrDefault();
        }
    }
}