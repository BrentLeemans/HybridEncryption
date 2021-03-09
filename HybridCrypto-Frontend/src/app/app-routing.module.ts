import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SendComponent } from './send/send.component';
import { InboxComponent } from './inbox/inbox.component';
import { LogoutComponent } from './logout/logout.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthGuardService } from './services/auth-guard.service';

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'send', component: SendComponent, canActivate: [AuthGuardService] },
  { path: 'inbox', component: InboxComponent, canActivate: [AuthGuardService] },
  { path: 'logout', component: LogoutComponent },
  {
    path: '',
    redirectTo: '/inbox',
    pathMatch: 'full',
    canActivate: [AuthGuardService],
  },
  {
    path: '**',
    component: PageNotFoundComponent,
    canActivate: [AuthGuardService],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
