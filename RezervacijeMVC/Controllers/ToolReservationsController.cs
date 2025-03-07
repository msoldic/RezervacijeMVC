using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezervacijeMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RezervacijeMVC.Controllers
{
    public class ToolReservationsController : Controller
    {
        private readonly ToolReservationContext _context;
        public ToolReservationsController(ToolReservationContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var toolReservations = await _context.ToolReservations.Include(tr => tr.Tool).ToListAsync();
            return View(toolReservations);
        }

        // GET: ToolReservations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.ToolReservations.Include(tr => tr.Tool)
                                                              .FirstOrDefaultAsync(tr => tr.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // GET: ToolReservations/Create
        public IActionResult Create()
        {
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType");
            return View();
        }

        // POST: ToolReservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientFirstName,ClientSecondName,DateReservationFrom,DateReservationTo,ToolID")] ToolReservation reservation)
        {
            if (ModelState.IsValid)
            {
                var tool = await _context.Tools.FindAsync(reservation.ToolID);
                if (tool == null)
                {
                    return NotFound("Tool not found.");
                }

                var days = (reservation.DateReservationTo - reservation.DateReservationFrom).Days;
                reservation.TotalRentPrice = days * tool.PriceRentPerDay;

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            return View(reservation);
        }

        // GET: ToolReservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.ToolReservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            return View(reservation);
        }

        // POST: ToolReservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientFirstName,ClientSecondName,DateReservationFrom,DateReservationTo,ToolID,TotalRentPrice")] ToolReservation reservation)
        {
            if (id != reservation.ID)
            {
                return BadRequest();
            }

            var tool = await _context.Tools.FindAsync(reservation.ToolID);
            if (tool == null)
            {
                return NotFound("Tool not found.");
            }

            var days = (reservation.DateReservationTo - reservation.DateReservationFrom).Days;
            reservation.TotalRentPrice = days * tool.PriceRentPerDay;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolReservationExists(reservation.ID))
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
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            return View(reservation);
        }

        // GET: ToolReservations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.ToolReservations.Include(tr => tr.Tool)
                                                              .FirstOrDefaultAsync(tr => tr.ID == id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: ToolReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.ToolReservations.FindAsync(id);
            _context.ToolReservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolReservationExists(int id)
        {
            return _context.ToolReservations.Any(e => e.ID == id);
        }
    }
}

