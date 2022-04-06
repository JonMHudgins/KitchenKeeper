using System.ComponentModel.DataAnnotations;

namespace KitchenKeeper.ViewModels
{
    public class SingleFileModel
    {
        [Required(ErrorMessage = "Please enter file name")]
        public int ItemID { get; set; }
        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
}
