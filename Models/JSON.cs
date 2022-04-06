using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KitchenKeeper.Models
{
    public class JSON
    {
        [Key]
        [ForeignKey("Item")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ItemID { get; set; }
        [Required]

        public byte[] Content { get; set; }
    }
}
