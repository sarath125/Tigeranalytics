import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BulkPriceFeedManagerComponent } from './bulk-price-feed-manager.component';

describe('BulkPriceFeedManagerComponent', () => {
  let component: BulkPriceFeedManagerComponent;
  let fixture: ComponentFixture<BulkPriceFeedManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BulkPriceFeedManagerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BulkPriceFeedManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
