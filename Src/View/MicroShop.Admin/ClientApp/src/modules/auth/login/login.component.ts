import { Component, OnInit } from '@angular/core';
import { ConfigService } from 'src/modules/shared/services/config.service';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private configService: ConfigService, private oauthService: OAuthService, private router: Router) {
  }

  ngOnInit(): void {
    this.oauthService.loadDiscoveryDocumentAndTryLogin()
      .then(loggedIn => {
        if (loggedIn) {
          this.router.navigateByUrl("/");
        }
      });
  }

  onLogin() {
    this.oauthService.initCodeFlow();
  }
}
