import { Component, OnInit } from '@angular/core';
import { OrdersService } from './services/orders.service';
import { DataResult } from '../shared/models/paged-result';
import { OrderDto } from './models/order-dto';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  page = 1;
  orders?: DataResult<OrderDto[]>;

  constructor(private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.getPage(1);
  }

  getPage(page: number) {
    this.ordersService.getOrders(this.page)
      .subscribe(orders => {
        this.orders = orders;
        this.page = page;
      });
  }
}
