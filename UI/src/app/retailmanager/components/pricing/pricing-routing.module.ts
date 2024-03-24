import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PricingComponent } from './pricing/pricing.component';
import { BulkPriceFeedManagerComponent } from './bulk-price-feed-manager/bulk-price-feed-manager.component';

const routes: Routes = [
  { path: '', component: PricingComponent} ,
  {path:'bulkprice',component:BulkPriceFeedManagerComponent},
  { path: '**', redirectTo: '/notfound'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PricingRoutingModule { }
