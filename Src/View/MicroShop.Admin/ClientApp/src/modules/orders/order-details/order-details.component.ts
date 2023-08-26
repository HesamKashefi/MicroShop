import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../services/orders.service';
import { ActivatedRoute } from '@angular/router';
import { OrderDetailsDto } from '../models/order-details-dto';

@Component({
  selector: 'app-order-details',
  templateUrl: './order-details.component.html',
  styleUrls: ['./order-details.component.scss']
})
export class OrderDetailsComponent implements OnInit {
  order?: OrderDetailsDto;

  constructor(private ordersService: OrdersService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(p => {
      const id = +p["id"];
      if (id > 0) {
        this.getOrder(id);
      }
    });
  }


  private getOrder(id: number) {
    this.ordersService.getOrderById(id)
      .subscribe(orderResult => {
        if (orderResult.status === 0) {
          this.order = orderResult.data;
        }
      });
  }
}
