import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthorizationService, Role } from '../../services/authorization.service';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../../services/api.service';
export interface ClientFeedback {
  feedbackDate: Date;
  feedbackType: FeedbackType;
  details: string;
}
enum FeedbackType {
  Suggestion = 'Suggestion',
  Complaint = 'Complaint',
}
@Component({
  selector: 'app-client-feedback',
  templateUrl: './client-feedback.component.html',
  styleUrl: './client-feedback.component.css'
})
export class ClientFeedbackComponent {
  dataSource: ClientFeedback[] = [
    { feedbackDate: new Date('2023-01-15'), feedbackType: FeedbackType.Suggestion, details: 'Some feedback details' },
    { feedbackDate: new Date('2023-02-20'), feedbackType: FeedbackType.Complaint, details: 'More feedback details' },
  ];

  displayedColumns: string[] = ['feedbackDate', 'feedbackType', 'details', 'action'];


  form!: FormGroup;
  constructor(private apiService: ApiService, private route: ActivatedRoute, private fb: FormBuilder, private authorizationService: AuthorizationService) { }

  token!: string;
  ngOnInit(): void {
    let id = localStorage.getItem('projectId');
    if (id) {
      this.token = id;
    }
    this.getClientData(this.token);
    this.form = this.fb.group({
      feedbackDate: ['', Validators.required],
      feedbackType: ['', Validators.required],
      details: ['', Validators.required],
      projectId: [id || '', Validators.required]
    });
  }
  getClientData(token: string) {
    this.apiService.getClientFeedback(token).subscribe(data => {
      this.dataSource = data;
      console.log(data);

    });
  }

  submitForm() {
    if (this.form.valid) {
      // Submit the form data
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
  isClient(): boolean {
    const userRole = this.authorizationService.getCurrentUser()?.role;
    return userRole === Role.Client;
  }
}
