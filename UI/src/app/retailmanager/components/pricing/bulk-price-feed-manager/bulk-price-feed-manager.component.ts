import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { environment } from '../../../../../environments/environment';


@Component({
  selector: 'app-bulk-price-feed-manager',
  standalone: true,
  imports: [FileUploadModule,CommonModule,ToastModule],
  templateUrl: './bulk-price-feed-manager.component.html',
  styleUrl: './bulk-price-feed-manager.component.scss',
  providers: [MessageService]

})
export class BulkPriceFeedManagerComponent {
  uploadedFiles: any[] = [];

  public BulkUploadAPI:string
  constructor(private messageService: MessageService) {
this.BulkUploadAPI =environment.apiUrl+'Pricing/UploadSKUPriceFile'

  }

  onUpload(event: any) {
   
   
    // // for (const file of event.files) {
    // //     this.uploadedFiles.push(file);
    // // }

    this.messageService.add({ severity: 'info', summary: 'Success', detail: 'File is uploaded and will be processed shortly' });
  }
}
