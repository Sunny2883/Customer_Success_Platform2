import { Injectable } from '@angular/core';
export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
}

export enum Role {
  Admin = 'admin',
  Manager = 'manager',
  Auditor = 'auditor',
  Client = 'client',
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {


  private users: User[] = [
    { id: 1, name: 'Chintan Shah', email: 'chintan@example.com', role: 'admin' },
    { id: 2, name: 'Dipa Manjukar', email: 'dipa@example.com', role: 'manager' },
    { id: 3, name: 'Divyanshu Singh', email: 'divyanshu@example.com', role: 'auditor' },
    { id: 1, name: 'Rahul yadav', email: 'rahul@example.com', role: 'client' },
  ];

  roles!: Role[];
 

  hasRoles(requiredRoles: Role[]): boolean {
    return this.roles.some(role => requiredRoles.includes(role));
  }

  getCurrentUser(): User {
    return this.users[0];
  }

}
