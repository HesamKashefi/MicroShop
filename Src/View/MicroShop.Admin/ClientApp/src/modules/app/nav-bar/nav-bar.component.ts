import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/modules/shared/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  user: any;

  constructor(private authService: AuthService) {
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(e => this.user = e);
  }

  onLogoutClicked() {
    this.authService.logout();
  }
}

