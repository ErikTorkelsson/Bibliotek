using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Bibliotek.Data;
using Bibliotek.Models;

namespace Bibliotek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly Context _context;

        public CardsController(Context context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return await _context.Cards.Include(r => r.Rentals).ToListAsync();
        }

        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        // PUT: api/Cards/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(int id, Card card)
        {
            if (id != card.CardId)
            {
                return BadRequest();
            }

            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // POST: api/Cards
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCard", new { id = card.CardId }, card);
        }

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Card>> DeleteCard(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return card;
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }


        // Fredriks metod för att låna en bok
        [HttpPost("{CardId}/rentBook/{BookId}")]
        public async Task<ActionResult<Card>> RentBook(int cardId, int bookId)
        {
            var Card = await _context.Cards
                .SingleOrDefaultAsync(c => c.CardId == cardId);

            if (Card == null)
            {
                return BadRequest("Card not found.");
            }

            // Hämtar ut all data tillhörande inventories
            var inventory = await _context.Inventories
                .Include(i => i.Book)
                .Include(i => i.Rentals)
                .Where(i => i.BookId == bookId)
                .ToListAsync();

            var availableInv = inventory.FirstOrDefault(i => i.Available);
            
            // Kollar om boken är tillgänglig
            if (availableInv == null)
            {
                return BadRequest("Book is not available");
            }

            var rental = new Rental()
            {
                CardId = cardId,
                InventoryId = availableInv.InventoryId,
                RentalDate = DateTime.Now,
                RentedUntilDate = DateTime.Now.Date.AddMonths(1)
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return Ok($"Customer {Card.FirstName} rented the book {availableInv.Book.Title} at {rental.RentalDate}");
        }

        // Fredriks metod för att lämna tillbaks böcker
        [HttpPost("{CardId}/returnBook/{BookId}")]
        public async Task<ActionResult<Card>> Returnbook(int cardId, int bookId)
        {
            // hämtar all info tillhörande kortet
            var card = await _context.Cards
                .Include(c => c.Rentals)
                .ThenInclude(r => r.Inventory)
                .ThenInclude(i => i.Book)
                .SingleOrDefaultAsync(c => c.CardId == cardId);

            if (card == null)
            {
                return BadRequest("Card does not exist.");
            }

            if (card.Rentals == null || card.Rentals.Count == 0)
            {
                return BadRequest("Card does not have any rentals.");
            }

            // kollar om det finns lånade böcker på kortet och plockar den första
            var rental = card.Rentals.FirstOrDefault(r => r.Inventory.BookId == bookId && !r.Returned);

            if (rental == null)
            {
                return BadRequest("The book is not rented at this card");
            }

            
            // stämmer allt sätts ett returdatum och boken har lämnats tillbaks.           
            _context.Entry(rental).State = EntityState.Modified;

            rental.ReturnDate = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok($"Customer {card.FirstName} return the book {rental.Inventory.Book.Title} at {rental.ReturnDate}");
        }
    }
}
