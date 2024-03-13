import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizationService, Role } from '../../services/authorization.service';
import { ApiService } from '../../services/api.service';
import { ActivatedRoute } from '@angular/router';
export interface ProjectUpdate {
  date: Date;
  generalUpdate: string;
}

@Component({
  selector: 'app-project-update',
  templateUrl: './project-update.component.html',
  styleUrl: './project-update.component.css'
})
export class ProjectUpdateComponent {
  dataSource: ProjectUpdate[] = [
    { date: new Date('2023-01-15'), generalUpdate: 'Some general update details' },
    { date: new Date('2023-02-20'), generalUpdate: 'More general update details' },
    // Add more data objects as needed
  ];

  displayedColumns: string[] = ['date', 'generalUpdate', 'action'];
  form!: FormGroup;

  constructor(private apiService: ApiService, private route: ActivatedRoute, private fb: FormBuilder, private authorizationService: AuthorizationService) {
    let id = localStorage.getItem('projectId');
    if (id) {
      this.projectId = id;
    }
    this.getProjectUdpate(this.projectId)
    this.form = this.fb.group({
      date: ['', Validators.required],
      generalUpdate: ['', Validators.required],
      projectId: [id ? id : '', Validators.required],
    });
  }


  projectId!: string;
  ngOnInit() {

  }
  getProjectUdpate(projectId: string) {
    this.apiService.getProjectUpdate(projectId).subscribe((res) => {
      this.dataSource = res;
    });
  }
  submitForm() {
    if (this.form.valid) {
      this.apiService.postProjectUdpate(this.form.value).subscribe(res => {
        this.apiService.showSuccessToast("Project Upddate Success Added");
      })
      console.log(this.form.value);
    } else {
      // Mark all fields as touched to trigger validation messages

    }
  }
  editItem(data: any) {

    // this.editDataId = data.id;

    this.form.patchValue(data);
  }
  deleteItem(id: any) {
    // this.apiService.deleteEscalationMatrix(id).subscribe(
    //   (res) => {
    //     this.getAllEscalationMatrix()
    //     this.apiService.showSuccessToast('Deleted Successfully');
    //   },
    //   (error) => {
    //     this.apiService.showSuccessToast('Error deleting ' + id + ': ' + error);
    //   }
    // );
  }

  isManager(): boolean {
    const userRole = this.authorizationService.getCurrentUser()?.role;
    return userRole === Role.Manager || userRole === Role.Admin;
  }

}
