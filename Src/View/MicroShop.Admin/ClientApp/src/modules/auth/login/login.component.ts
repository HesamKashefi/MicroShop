import { Component } from '@angular/core';
import { ConfigService } from 'src/modules/shared/services/config.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private configService: ConfigService) { }
}
