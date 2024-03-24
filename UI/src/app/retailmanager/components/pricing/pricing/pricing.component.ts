import { ChangeDetectorRef, Component } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { AgGridModule } from '@ag-grid-community/angular';
import { environment } from '../../../../../environments/environment';


import "@ag-grid-enterprise/all-modules";
import { MessageService } from 'primeng/api';
import {
  ColDef,
  GridApi,
  GridOptions,
  GridReadyEvent,
  ModuleRegistry,
  IServerSideDatasource,
  ColTypeDef
} from "@ag-grid-community/core";
import { AdvancedFilterModule } from "@ag-grid-enterprise/advanced-filter";
import { MenuModule } from "@ag-grid-enterprise/menu";
import { ServerSideRowModelModule } from "@ag-grid-enterprise/server-side-row-model";
import { Observable } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';

ModuleRegistry.registerModules([
  AdvancedFilterModule,
  MenuModule,
  ServerSideRowModelModule
]);

@Component({
  selector: 'app-pricing',
  standalone: true,
  imports: [AgGridModule, DialogModule, ButtonModule, CommonModule, ToastModule],
  providers: [MessageService],
  templateUrl: './pricing.component.html',
  styleUrls: ['./pricing.component.scss']
})
export class PricingComponent {
  public gridOptions: GridOptions;
  public gridApi: GridApi;
  public deletePricing: boolean = false;
  public deletePricingID: any;
  public payload: any = {
    StartRow: 1,
    EndRow: 10,
    PivotMode: true,
    CurrentPageNumber: 1,
    PageSize: 2
  };
  public columnTypes: { [key: string]: ColTypeDef } = {
    number: { filter: "agNumberColumnFilter" }
  };
  public columnDefs: ColDef[] = [
    {
      headerName: 'Store Name',
      field: "StoreName",
      cellDataType: "text",
      minWidth: 220,
    },
    {
      headerName: 'SKU',
      field: "Sku",
      cellDataType: "number",
    },
    {
      field: "Price",
      cellDataType: "number",
      editable: true
    },
    {
      field: "Date",
      cellDataType: "date",
      editable: true,
      valueFormatter: (params) => {
        const date = new Date(params.value);
        return date.toLocaleDateString('zh-CN');
      },
      valueParser: params => new Date(params.newValue)
    },
    {
      field: "Country",
      cellDataType: "text",
    },
    {
      headerName: 'Delete',
      width: 5,
      cellRenderer: (params) => {
        // Create delete icon element
        var eDiv = document.createElement('div');
        eDiv.innerHTML = '<i class="pi pi-trash" style="cursor: pointer;"></i>';
        // Get the delete icon element
        var eDelete = eDiv.querySelector('.pi.pi-trash');
        // Add click event listener for delete operation
        eDelete.addEventListener('click', () => {
          this.deletePricingID = params.data.PricingID;
          this.deletePricing = true;
          this.cdr.detectChanges();
        });
        return eDiv;
      }
    }
  ];
  private apibaseUrl =environment.apiUrl+'Pricing';
  public defaultColDef: ColDef = {
    flex: 1,
    minWidth: 100,
    filter: true,
    menuTabs: ["filterMenuTab"]
  };
  public rowModelType: string = "serverSide";
  public rowData!: any[];
  public themeClass: string = "ag-theme-quartz";

  constructor(
    private http: HttpClient,
    private messageService: MessageService,
    private cdr: ChangeDetectorRef
  ) {

  }

  onGridReady(params: GridReadyEvent<any>) {
    var datasource = this.getServerSideDatasource();
    params.api!.setGridOption("serverSideDatasource", datasource);
    this.gridApi = params.api;
  }

  deleteItem(itemId: any): Observable<any> {
    
    return this.http.delete(`${this.apibaseUrl}/${itemId}`);
  }

  onCellValueChanged(event: any) {
    const itemId = event.data.PricingID;
    const updatedValue = event.value;  

    var pricing={
      "PricingId": event.data.PricingID,
     "StoreId": event.data.StoreId,
     "Sku": event.data.Sku,
    "Price": event.data.Price,
     "Date": event.data.Date
    }

    this.http.put(`${this.apibaseUrl}/${itemId}`, pricing)
      .subscribe(
        res =>{this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Updated Sucessfully', life: 3000 })},
        err => console.error('Update failed', err)
      );
  }

  getServerSideDatasource(): IServerSideDatasource {
    return {
      getRows: (params: any) => {
        console.log("[Datasource] - rows requested by grid:", params.request);
        let filterModel = "";
        if (params.request?.filterModel && params.request.filterModel?.hasOwnProperty("conditions")) {
          filterModel = JSON.stringify(params.request.filterModel.conditions);
        } else if (params.request?.filterModel) {
          filterModel = JSON.stringify([params.request.filterModel])
        }
        const sortModel = JSON.stringify(params.request.sortModel);
        let queryParams = new HttpParams()
          .set('StartRow', params.request.startRow.toString())
          .set('EndRow', params.request.endRow.toString())
          .set('FilterModelJson', (filterModel != "" ? JSON.stringify(filterModel) : JSON.stringify([])))
          .set('Sort', JSON.stringify([]))       
        this.http.get<any>(this.apibaseUrl, { params: queryParams }).subscribe({
          next: (data: any) => {
            params.success({ rowData: data.Items, rowCount: data.TotalCount }); // Provide both data and total count
          },
          error: (err) => {
            console.error('Error fetching data:', err);
            params.fail();
          }
        });
      }
    };
  }




  confirmDeleteSelected() {
    this.deleteItem(this.deletePricingID).subscribe({
      next: (response) => {
        this.gridApi.onFilterChanged();
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Price feed for SKU has been successfully deleted.', life: 3000 });
        this.deletePricing = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error deleting item:', error);
      }
    });
  }

  cancelDelete() {
    this.deletePricing = false;
    this.cdr.detectChanges();
  }
  

}
