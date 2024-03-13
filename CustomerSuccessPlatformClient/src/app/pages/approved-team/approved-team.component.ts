import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizationService, Role } from '../../services/authorization.service';
import { ApiService } from '../../services/api.service';
export interface Resource {
  noOfResources: number;
  role: string;
  phaseNo: number;
  duration: string;
  availability: string;
  projectId: string;
}
@Component({
  selector: 'app-approved-team',
  templateUrl: './approved-team.component.html',
  styleUrl: './approved-team.component.css'
})
export class ApprovedTeamComponent {
  form!: FormGroup;
  dataSource: Resource[] = [
    { noOfResources: 5, role: 'Developer', phaseNo: 1, duration: '3 months', availability: 'Full-time', projectId: '12345' },
    { noOfResources: 3, role: 'Tester', phaseNo: 2, duration: '2 months', availability: 'Part-time', projectId: '54321' },
  ];

  displayedColumns: string[] = ['noOfResources', 'role', 'phaseNo', 'duration', 'availability', 'action'];
  projects!: any[];


  projectId!: string
  constructor(private apiService: ApiService, private fb: FormBuilder, private authorizationService: AuthorizationService) {
    let id = localStorage.getItem('projectId');
    if (id) {
      this.projectId = id;
    }
    this.form = this.fb.group({
      noOfResources: ['', [Validators.required, Validators.min(1)]],
      role: ['', Validators.required],
      phaseNo: ['', [Validators.required, Validators.min(1)]],
      duration: ['', [Validators.required, Validators.min(1)]],
      availability: ['', Validators.required],
      projectId: [id || '', Validators.required]
    });
  }

  ngOnInit(): void {
    this.getApprovedTeam()

  }

  getApprovedTeam() {
    this.apiService.getApprovedTeam(this.projectId).subscribe(team => {
      this.dataSource = team;
    });
  }
  submitForm() {
    if (this.form.valid) {

      this.apiService.postApprovedTeam(this.form.value).subscribe(data => {
        console.log(data);
        this.apiService.showSuccessToast("Approved team successfully");
      })

    } else {


    }
  }


  editItem(data: any) {

    // this.editDataId = data.id;

    this.form.patchValue(data);
  }
  deleteItem(id: any) {
    this.apiService.deleteApprovedTeam(id).subscribe(
      (res) => {
        this.getApprovedTeam()
        this.apiService.showSuccessToast('Deleted Successfully');
      },
      (error) => {
        this.apiService.showSuccessToast('Error deleting ' + id + ': ' + error);
      }
    );
  }

  isManager(): boolean {
    const userRole = this.authorizationService.getCurrentUser()?.role;
    return userRole === Role.Manager || userRole === Role.Admin;
  }

}
