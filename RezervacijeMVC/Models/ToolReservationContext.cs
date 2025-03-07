using Microsoft.EntityFrameworkCore;

namespace RezervacijeMVC.Models
{
    public class ToolReservationContext : DbContext
    {
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolReservation> ToolReservations { get; set; }

        public ToolReservationContext(DbContextOptions<ToolReservationContext> options)
            : base(options)
        { }

    }
}
