using System.ComponentModel.DataAnnotations;

namespace AuctionSystemServer.Application.DTOs
{
    public class BidRequest
    {
        [Required]
        public int AuctionId { get; init; }

        [Required]
        [StringLength(50)]
        public string BidderName { get; init; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Bid must be greater than 0")]
        public decimal BidAmount { get; init; }
    }
}
