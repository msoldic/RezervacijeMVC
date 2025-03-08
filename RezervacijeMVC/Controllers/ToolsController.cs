using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezervacijeMVC.Models;
using System.Linq;
using System.Threading.Tasks;


namespace RezervacijeMVC.Controllers
{
    public class ToolsController : Controller
    {
        private readonly ToolReservationContext _context;
        /*public IActionResult Index()
        {
            return View();
        }*/
        public ToolsController(ToolReservationContext context)
        {
            _context = context;
        }

        // GET: Tools (with pagination for tools)
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var tools = _context.Tools.AsQueryable();

            var totalTools = await tools.CountAsync();
            var totalPages = (int)Math.Ceiling(totalTools / (double)pageSize);

            var paginatedTools = await tools
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedViewModel<Tool>
            {
                Items = paginatedTools,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View(model);
        }
        // GET: Tools/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            return View(tool);
        }

        // GET: Tools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ToolType,PriceRentPerDay")] Tool tool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tool);
        }
        // GET: Tools/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            return View(tool);
        }

        // POST: Tools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ToolType,PriceRentPerDay")] Tool tool)
        {
            if (id != tool.ID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolExists(tool.ID))
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
            return View(tool);
        }

        // GET: Tools/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
            {
                return NotFound();
            }
            return View(tool);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tool = await _context.Tools.FindAsync(id);
            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.ID == id);
        }
    }
}

