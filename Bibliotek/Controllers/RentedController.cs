using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bibliotek.Data;
using Bibliotek.Models;

namespace Bibliotek.Controllers
{
    public class RentedController : Controller
    {
        private readonly Context _context;

        public RentedController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Index metoden går till listan för att visa alla låntagare md försenade lån
            // Men själva checken sker i Index vy

            var cards = await _context.Cards.Include(r => r.Rentals).ToListAsync();

            return View(cards);

        }

        public async Task<IActionResult> Books(int? id)
        {
            // Books visar en lista på alla försenade böcker tillhörande ett specifikt id
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Where(r => r.RentedUntilDate.Date < DateTime.Now.Date && r.ReturnDate == null && r.CardId == id)
                .Include(i => i.Inventory)
                .ThenInclude(b => b.Book)
                .ThenInclude(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .OrderBy(o => o.RentedUntilDate)
                .ToListAsync();


            return View(rentals); 

        }

        public async Task<IActionResult> Rentals()
        {
            var context = _context.Rentals
                .Where(r => r.RentedUntilDate.Date < DateTime.Now.Date && r.ReturnDate == null)
                .Include(r => r.Card)
                .Include(r => r.Inventory)
                .ThenInclude(b => b.Book);
            return View(await context.ToListAsync());
        }

        // GET: Rented/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }
        
    }
}
