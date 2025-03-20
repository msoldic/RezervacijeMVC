using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RezervacijeMVC.Models;
using System.Linq;
using System.Net;
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
        // GET: ToolReservations
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var toolReservations = _context.ToolReservations.Include(tr => tr.Tool).AsQueryable();
         
            var totalReservations = await toolReservations.CountAsync();
            var totalPages = (int)Math.Ceiling(totalReservations / (double)pageSize);
   
            var paginatedReservations = await toolReservations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
  
            var model = new PaginatedViewModel<ToolReservation>
            {
                Items = paginatedReservations,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }
        // drop down
        public async Task<IEnumerable<Tool>> GetToolsForDropdown(int pageNumber, int pageSize)
        {
            var tools = await _context.Tools
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return tools;
        }
        // GET: ToolReservations/Create
        public async Task<IActionResult> Create()
        {            
            var tools = await GetToolsForDropdown(1, 10);
            ViewData["Tools"] = new SelectList(tools, "ID", "ToolType");
            return PartialView("_ToolReservationModal");
        }
        // GET: ToolReservations/Details/1
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

        /*public IActionResult Create()
        {
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType");
            return View();
        }*/

        // POST: ToolReservations/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
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
                return Ok(HttpStatusCode.OK); //RedirectToAction(nameof(Index));
            }
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            return View(reservation);
        }

        // GET: ToolReservations/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await _context.ToolReservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            //ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            //return View(reservation);
            return PartialView("_ToolReservationModal", reservation);
        }

        // POST: ToolReservations/Edit/1
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ClientFirstName,ClientSecondName,DateReservationFrom,DateReservationTo,ToolID,TotalRentPrice")] ToolReservation reservation)
        {
            if (id != reservation.ID)
            {
                return BadRequest(id.ToString() + " " + reservation.ID.ToString());
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
                _context.Update(reservation);
                await _context.SaveChangesAsync();
            }
            ViewData["ToolID"] = new SelectList(_context.Tools, "ID", "ToolType", reservation.ToolID);
            return Ok(HttpStatusCode.OK);//View(reservation);
        }

        // POST: ToolReservations/Delete
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
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

        // GET: ToolReservations/Delete/1
        [HttpPost, ActionName("DeleteF")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.ToolReservations.Include(tr => tr.Tool)
                                                              .FirstOrDefaultAsync(tr => tr.ID == id);
            if (reservation == null)
            {
                return NotFound(id.ToString());
            }
   
            return Ok(HttpStatusCode.OK);//View(reservation);
        }
    }
}

