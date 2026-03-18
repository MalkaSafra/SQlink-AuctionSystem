using AuctionSystemServer.Core.Entities;
using AuctionSystemServer.Core.Interfaces;
using AuctionSystemServer.Infrastructure.Hubs;
using AuctionSystemServer.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystemServer.Application.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<AuctionHub> _hubContext;

        public AuctionService(AppDbContext context, IHubContext<AuctionHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // 1. שליפת כל המכירות (עבור ה-List באנגולר)
        public async Task<IEnumerable<Auction>> GetAllAuctionsAsync()
        {
            return await _context.Auctions
                .Where(a => a.Status == "Active")
                .ToListAsync();
        }

        // 2. שליפת מכירה ספציפית (עבור ה-Details באנגולר)
        public async Task<Auction?> GetAuctionByIdAsync(int id)
        {
            return await _context.Auctions.FindAsync(id);
        }

        // 3. הגשת הצעה - הלב של המערכת
        public async Task<bool> PlaceBidAsync(int auctionId, string bidderName, decimal bidAmount)
        {
            var auction = await _context.Auctions.FindAsync(auctionId);

            // בדיקות תקינות בסיסיות
            if (auction == null || auction.Status != "Active" || bidAmount <= auction.CurrentHighBid)
            {
                return false;
            }

            // עדכון המחיר בזיכרון
            auction.CurrentHighBid = bidAmount;

            try
            {
                await _context.SaveChangesAsync();

                // שליחת עדכון בזמן אמת לכל הלקוחות המחוברים (SignalR)
                // אנגולר יקשיב לאירוע בשם "ReceiveNewBid"
                await _hubContext.Clients.All.SendAsync("ReceiveNewBid", auctionId, bidAmount);

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // קורה אם שני אנשים הגישו הצעה בדיוק באותה מילי-שנייה
                return false;
            }
        }

        // 4. סגירת מכירה (מופעל ע"י ה-Worker כשהזמן נגמר)
        public async Task<bool> CloseAuctionAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null || auction.Status == "Closed") return false;

            auction.Status = "Closed";
            await _context.SaveChangesAsync();

            // עדכון הלקוחות שהמכירה נסגרה
            await _hubContext.Clients.All.SendAsync("AuctionClosed", id);

            return true;
        }
    }
}
