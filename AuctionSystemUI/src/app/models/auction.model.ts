export interface Auction {
  id: number;
  title: string;
  currentHighBid: number;
  endTime: Date;
}

export interface AuctionDetails extends Auction {
  description: string;
  status: string;
}


export interface BidRequest {
  auctionId: number;
  bidderName: string;
  bidAmount: number;
}