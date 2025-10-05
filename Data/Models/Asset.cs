using System.ComponentModel.DataAnnotations;

namespace AssetManagementApp.Data.Models
{
    public class Asset
    {
        [Key]
        public int AssetId { get; set; }

        [Required(ErrorMessage = "Asset name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Asset name must be between 3-100 characters")]
        public string AssetName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Asset type is required")]
        [StringLength(50)]
        public string AssetType { get; set; } = string.Empty;

        [StringLength(100)]
        public string? MakeModel { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Serial number must be between 3-100 characters")]
        [RegularExpression(@"^[A-Za-z0-9\-]+$", ErrorMessage = "Serial number can only contain letters, numbers, and hyphens")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purchase date is required")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        public DateTime? WarrantyExpiryDate { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        [StringLength(50)]
        public string Condition { get; set; } = "Good";

        [Required(ErrorMessage = "Status is required")]
        [StringLength(50)]
        public string Status { get; set; } = "Available";

        public bool IsSpare { get; set; } = false;

        [StringLength(500, ErrorMessage = "Specifications cannot exceed 500 characters")]
        public string? Specifications { get; set; }

        public virtual ICollection<AssetAssignment> AssetAssignments { get; set; } = new List<AssetAssignment>();
    }
}