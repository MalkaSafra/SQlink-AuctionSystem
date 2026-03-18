import { Routes } from '@angular/router';
import { AuctionListComponent } from './components/auction-list/auction-list.component';
import { AuctionDetailsComponent } from './components/auction-details/auction-details.component';

export const routes: Routes = [
  { path: '', component: AuctionListComponent },
  
  { path: 'auction/:id', component: AuctionDetailsComponent },
  
  { path: '**', redirectTo: '' }
];