using System.ComponentModel.DataAnnotations;

namespace RezervacijeMVC.Models
{
    public class Tool
    {
        [Key]
        public int ID { get; set; }
        public string ToolType { get; set; }
        public int PriceRentPerDay { get; set; }
    }
}
