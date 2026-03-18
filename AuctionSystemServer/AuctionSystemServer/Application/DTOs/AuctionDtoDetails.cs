namespace AuctionSystemServer.Application.DTOs
{
    public class AuctionDtoDetails
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal CurrentHighBid { get; init; }
        public DateTime EndTime { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
