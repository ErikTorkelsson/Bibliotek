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

        // GET: Rented
        //public async Task<IActionResult> Index()
        //{
        //    var cards = await _context.Cards.ToListAsync();

        //    foreach (var item in cards)
        //    {
        //        item.OnNotReturnedList = false;
        //    }

        //    var rentals = await _context.Rentals.Where(r => r.RentedUntilDate < DateTime.Now && r.ReturnDate == null).Include(c => c.Card).ToListAsync();

        //    foreach (var item in rentals)
        //    {
        //        item.Card.OnNotReturnedList = true;
        //    }

        //    var lateCards = cards.Where(c => c.OnNotReturnedList == true);

        //    return View(lateCards);

        //}

        public async Task<IActionResult> Index()
        {
            var cards = await _context.Cards.Include(r => r.Rentals).ToListAsync();

            return View(cards);

        }

        public async Task<IActionResult> Books(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentals = await _context.Rentals
                .Where(r => r.RentedUntilDate < DateTime.Now && r.ReturnDate == null && r.CardId == id)
                .Include(i => i.Inventory)
                .ThenInclude(b => b.Book)
                .ThenInclude(ba => ba.BookAuthors)
                .ThenInclude(a => a.Author)
                .ToListAsync();


            return View(rentals); 


        }

        public async Task<IActionResult> Rentals()
        {
            var context = _context.Rentals
                .Where(r => r.RentedUntilDate < DateTime.Now && r.ReturnDate == null)
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

        // GET: Rented/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rented/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,FirstName,LastName,Email,TelephoneNumber")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Rented/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Rented/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardId,FirstName,LastName,Email,TelephoneNumber")] Card card)
        {
            if (id != card.CardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Rented/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Rented/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }
}
