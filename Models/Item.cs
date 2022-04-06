using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKeeper.Models
{
    public class Item
    {
        [Key]
        [Required]
        [DisplayName("Item ID")]
        public int ItemID { get; set; }
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [DisplayName("Unit")]
        public string UnitOfMeasure { get; set; }
        [DisplayName("Purchase Date")]
        public DateTime? PurchaseDate { get; set; }
        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDate { get; set; }
        [ForeignKey("StorageLocation")]
        [Required]
        [DisplayName("Location")]
        public int LocationID { get; set; }
    }
}
