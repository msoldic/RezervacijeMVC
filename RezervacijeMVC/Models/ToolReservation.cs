using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RezervacijeMVC.Models
{
    public class ToolReservation
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string ClientFirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string ClientSecondName { get; set; } = string.Empty;
        public DateTime DateReservationFrom { get; set; }
        public DateTime DateReservationTo { get; set; }

        [ForeignKey("Tool")]
        public int ToolID { get; set; }
        public Tool? Tool { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int TotalRentPrice { get; set; }
    }
}
