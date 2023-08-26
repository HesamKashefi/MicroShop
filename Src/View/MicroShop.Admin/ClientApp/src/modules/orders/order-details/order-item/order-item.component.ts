import { Component, Input } from '@angular/core';
import { OrderItem } from '../../models/order-details-dto';

@Component({
  selector: 'app-order-item',
  templateUrl: './order-item.component.html',
  styleUrls: ['./order-item.component.scss']
})
export class OrderItemComponent {
  @Input()
  orderItem!: OrderItem;


}
