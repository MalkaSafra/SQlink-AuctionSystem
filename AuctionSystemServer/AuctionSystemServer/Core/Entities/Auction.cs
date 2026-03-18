using System.ComponentModel.DataAnnotations;

namespace AuctionSystemServer.Core.Entities
{
    public class Auction
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal CurrentHighBid { get; set; }

        public DateTime EndTime { get; set; }

        public string Status { get; set; } = "Active"; // Active, Closed

        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
