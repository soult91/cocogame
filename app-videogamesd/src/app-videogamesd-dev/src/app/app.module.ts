import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { UserService } from './services/user.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChartModule } from 'angular-highcharts';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UsersComponent } from './components/users/users.component';
import { from } from 'rxjs';
import { NewUserComponent } from './components/new-user/new-user.component';
import { UserComponent } from './components/user/user.component';
import { HeaderComponent } from './components/header/header.component';
import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';
import { DataTablesModule } from 'angular-datatables';


const config: SocketIoConfig = {url: 'localhost://4444', options: {}};

@NgModule({
  declarations: [
    AppComponent,
    NewUserComponent,
    UserComponent,
    UsersComponent,
    HeaderComponent
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ChartModule,
    SocketIoModule.forRoot(config),
    DataTablesModule
  ],
  exports: [
    UsersComponent
  ],
  providers: [UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
