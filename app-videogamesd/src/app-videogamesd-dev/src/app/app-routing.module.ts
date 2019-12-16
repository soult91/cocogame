import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersComponent } from './components/users/users.component';
import { NewUserComponent } from './components/new-user/new-user.component';
import { UserComponent } from './components/user/user.component';
import { LoginComponent } from './auth/login/login.component';

const routes: Routes = [
  {path: 'users', component: UsersComponent},
  {path: 'new-user', component: NewUserComponent},
  {path: 'users/:id', component: UserComponent},
  {path: '', redirectTo: '/auth', pathMatch:'full'},
  {path: 'auth', loadChildren:'./auth/auth.module#AuthModule'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})


export class AppRoutingModule {
 }
