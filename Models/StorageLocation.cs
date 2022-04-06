using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KitchenKeeper.Models
{
    public class StorageLocation
    {
        [Key]
        [Required]
        public int LocationID { get; set; }
        [DisplayName("Location")]
        public string LocationName { get; set; }
    }
}
