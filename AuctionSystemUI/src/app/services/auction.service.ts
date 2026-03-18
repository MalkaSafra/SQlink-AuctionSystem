import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { Auction, AuctionDetails, BidRequest } from '../models/auction.model';

@Injectable({
  providedIn: 'root'
})
export class AuctionService {
  private hubConnection!: signalR.HubConnection;
  
  private bidUpdateSource = new Subject<{ auctionId: number, newPrice: number }>();
  bidUpdates$ = this.bidUpdateSource.asObservable();

  private auctionClosedSource = new Subject<number>();
  auctionClosed$ = this.auctionClosedSource.asObservable();

  constructor(private http: HttpClient) {
    this.startSignalRConnection();
  }


  getAuctions(): Observable<Auction[]> {
    return this.http.get<Auction[]>('auctions');
  }

  getAuctionById(id: number): Observable<AuctionDetails> {
    return this.http.get<AuctionDetails>(`auctions/${id}`);
  }

  placeBid(bid: BidRequest): Observable<any> {
    return this.http.post('auctions/bid', bid);
  }


  private startSignalRConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5001/auctionhub') 
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR: Connected to Hub'))
      .catch(err => console.error('SignalR: Connection Error: ', err));

    this.hubConnection.on('ReceiveNewBid', (auctionId: number, newPrice: number) => {
      this.bidUpdateSource.next({ auctionId, newPrice });
    });

    this.hubConnection.on('AuctionClosed', (auctionId: number) => {
      this.auctionClosedSource.next(auctionId);
    });
  }
}