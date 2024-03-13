import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AngularMaterialModule } from './material.module';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { StakeholderComponent } from './pages/stakeholder/stakeholder.component';
import { AuditHistoryComponent } from './pages/audit-history/audit-history.component';
import { ProjectBudgetComponent } from './pages/project-budget/project-budget.component';
import { VersionHistoryComponent } from './pages/version-history/version-history.component';
import { SprintComponent } from './pages/sprint/sprint.component';
import { EscalationMatrixComponent } from './pages/escalation-matrix/escalation-matrix.component';
import { RiskProfileComponent } from './pages/risk-profiling/risk-profiling.component';
import { PhaseMilestoneComponent } from './pages/phase-milestone/phase-milestone.component';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';

// Auth 0
import { AuthModule } from '@auth0/auth0-angular';

// angualr material
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatSidenavModule} from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table'


import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ApprovedTeamComponent } from './pages/approved-team/approved-team.component';
import { ResourcesComponent } from './pages/resources/resources.component';
import { MinuteMeetingComponent } from './pages/minute-meeting/minute-meeting.component';
import { ClientFeedbackComponent } from './pages/client-feedback/client-feedback.component';
import { ProjectUpdateComponent } from './pages/project-update/project-update.component';
import { ProjectComponent } from './pages/project/project.component';
import { TabComponent } from './components/tab/tab.component';


@NgModule({
  declarations: [
    AppComponent,
    StakeholderComponent,
    AuditHistoryComponent,
    ProjectBudgetComponent,
    VersionHistoryComponent,
    SprintComponent,
    EscalationMatrixComponent,
    RiskProfileComponent,
    PhaseMilestoneComponent,
    LoginComponent,
    DashboardComponent,
    ApprovedTeamComponent,
    ResourcesComponent,
    MinuteMeetingComponent,
    ClientFeedbackComponent,
    ProjectUpdateComponent,
    ProjectComponent,
    TabComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    MatFormFieldModule,
    MatInputModule,
    MatSidenavModule,
    MatTableModule,
    AngularMaterialModule,



   
    // Import the module into the application, with configuration
    AuthModule.forRoot({
      domain: 'dev-w2cpuspyl8y04bzh.us.auth0.com',
      clientId: 'l6AoZ9MlugUZmmFY83P58BA2YQargRJ3',
      
      authorizationParams: {
        redirect_uri: "http://localhost:4200/dashboard"
      }
    }),
    
  ],
  providers: [
    provideClientHydration(),
    provideAnimationsAsync(),
    provideHttpClient(),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
