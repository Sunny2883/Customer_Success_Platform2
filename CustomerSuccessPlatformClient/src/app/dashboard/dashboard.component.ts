import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { Router } from '@angular/router';
import 'jspdf-autotable';
import { ChangeDetectorRef } from '@angular/core';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

  isLoading: boolean;
  constructor(public router: Router, public apiService: ApiService, private cdr: ChangeDetectorRef, @Inject(DOCUMENT) public document: Document,
    public authService: AuthService) {
    this.authService.user$.subscribe(user => {
      console.log(user);
      localStorage.setItem('user', JSON.stringify(user));
      router.navigate(['/dashboard/project']);
    });

    console.log(this.authService.user$);
    console.log(this.authService.isAuthenticated$)
    this.authService.isAuthenticated$.subscribe(res => {
      console.log("Working");
      console.log(res);
    })
    this.isLoading = false;
  }
  ngOnInit(): void {
    this.apiService.isLoading().subscribe(isLoading => {
      this.isLoading = isLoading;
    });
  }
  Navigations = [
    { path: 'dashboard/project', displayName: 'Project' },
    { path: 'project-manager', displayName: 'Project Manager' },
    { path: 'settings', displayName: 'Settings' },
  ];


  logout() {
    this.authService.logout({
      logoutParams: {
        returnTo: "http://localhost:4200/login"
      }
    });
  }    
  navigateTo(path: string) {

    // throw new Error('Method not implemented.');
    this.router.navigate([path])
  }

}
