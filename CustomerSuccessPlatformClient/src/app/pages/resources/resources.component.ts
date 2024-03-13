import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizationService, Role } from '../../services/authorization.service';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
export interface Resources {
  name: string;
  role: string;
  startDate: Date;
  endDate: Date;
  comment: string;
}
@Component({
  selector: 'app-resources',
  templateUrl: './resources.component.html',
  styleUrl: './resources.component.css'
})
export class ResourcesComponent {
  dataSource: Resources[] = [
    { name: 'John Doe', role: 'Developer', startDate: new Date('2023-01-15'), endDate: new Date('2023-02-20'), comment: 'Some comment' },
    { name: 'Jane Smith', role: 'Tester', startDate: new Date('2023-03-10'), endDate: new Date('2023-04-15'), comment: 'Another comment' },
    // Add more data objects as needed
  ];

  displayedColumns: string[] = ['name', 'role', 'startDate', 'endDate', 'comment', 'action'];
  form!: FormGroup;
  editDataId: any;

  constructor(private apiService: ApiService, private route: ActivatedRoute, private fb: FormBuilder, private authorizationService: AuthorizationService) {
    let id = localStorage.getItem('projectId');
    if (id) {
      this.projectId = id;
    }
    this.getResources(this.projectId);
    this.form = this.fb.group({
      name: ['', Validators.required],
      role: ['', Validators.required],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      comment: ['', Validators.required],
      projectId: [id || '', Validators.required],
    });
  }

  projectId!: string;
  ngOnInit() {

  }
  getResources(projectId: string) {
    this.apiService.getResources(projectId).subscribe((res) => {
      this.dataSource = res;
    });
  }

  submitForm() {
    if (this.form.valid) {
      // Submit the form data
      this.apiService.postResources(this.form.value).subscribe(res => {
        console.log(res);
        this.apiService.showSuccessToast("Resource Added Successfully")
      });
      console.log(this.form.value);
    } else {

    }
  }

  editItem(data: any) {

    this.editDataId = data.id;

    this.form.patchValue(data);
  }
  deleteItem(id: any) {
    this.apiService.deleteEscalationMatrix(id).subscribe(
      (res) => {
        this.getResources(this.projectId)
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
