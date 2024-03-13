import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent{
  constructor(private auth: AuthService) {
  
    
  }
  ngOnInit(){
    let user = localStorage.getItem('user');
    if(user) {
      console.log(user);
    }
  }
  login() {
    this.auth.loginWithRedirect();
  }
}