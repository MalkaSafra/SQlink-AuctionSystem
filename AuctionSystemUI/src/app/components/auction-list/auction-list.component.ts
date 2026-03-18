import { Component, OnInit, OnDestroy, signal, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuctionService } from '../../services/auction.service';
import { Auction } from '../../models/auction.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-auction-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './auction-list.component.html'
})
export class AuctionListComponent implements OnInit, OnDestroy {
  auctions = signal<Auction[]>([]);
  
  private auctionService = inject(AuctionService);
  private bidSubscription?: Subscription;

  ngOnInit(): void {
    this.auctionService.getAuctions().subscribe(data => {
      this.auctions.set(data);
    });

    this.bidSubscription = this.auctionService.bidUpdates$.subscribe(update => {
      if (update) {
        this.auctions.update(current => 
          current.map(a => a.id === update.auctionId 
            ? { ...a, currentHighBid: update.newPrice } 
            : a
          )
        );
      }
    });
  }

  getRemainingTime(endTime: Date): string {
    const distance = new Date(endTime).getTime() - Date.now();
    if (distance < 0) return 'Expired';
    const minutes = Math.floor((distance % 3600000) / 60000);
    const seconds = Math.floor((distance % 60000) / 1000);
    return `${minutes}m ${seconds}s`;
  }

  ngOnDestroy(): void {
    this.bidSubscription?.unsubscribe();
  }
}