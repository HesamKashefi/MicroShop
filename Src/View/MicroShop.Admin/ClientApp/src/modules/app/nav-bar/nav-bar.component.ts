import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/modules/shared/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  user: any;

  constructor(
    private authService: AuthService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(e => this.user = e);
  }

  onLogoutClicked() {
    this.authService.logout();
    this.router.navigateByUrl("/login");
  }
}

