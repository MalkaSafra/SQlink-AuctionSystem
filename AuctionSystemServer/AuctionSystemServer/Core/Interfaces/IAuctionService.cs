using AuctionSystemServer.Core.Entities;

namespace AuctionSystemServer.Core.Interfaces
{
    public interface IAuctionService
    {
        Task<IEnumerable<Auction>> GetAllAuctionsAsync();

        Task<Auction?> GetAuctionByIdAsync(int id);

        Task<bool> PlaceBidAsync(int auctionId, string bidderName, decimal bidAmount);

        Task<bool> CloseAuctionAsync(int id);
    }
}
