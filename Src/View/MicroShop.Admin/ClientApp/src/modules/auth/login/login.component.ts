import { Component, OnInit } from '@angular/core';
import { ConfigService } from 'src/modules/shared/services/config.service';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  constructor(private configService: ConfigService, private oauthService: OAuthService) {
  }

  ngOnInit(): void {
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
    this.oauthService.tryLoginCodeFlow();
  }

  onLogin() {
    this.oauthService.initCodeFlow();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }
}
