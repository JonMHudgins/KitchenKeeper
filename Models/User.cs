using System.ComponentModel.DataAnnotations;

namespace KitchenKeeper.Models
{
    public class User
    {
        public int UserID { get; set; }

        // user ID from ASPNetUser table.
        public string? OwnerID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
    }
}
