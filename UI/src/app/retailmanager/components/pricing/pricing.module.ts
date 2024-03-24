import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PricingRoutingModule } from './pricing-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { DialogModule } from 'primeng/dialog';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PricingRoutingModule,
    HttpClientModule,
    DialogModule
  ]
})
export class PricingModule { }
