import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';

import { LoggedUserService } from './services/logged-user.service';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { MessageService } from './services/message.service';
import { AppMessagesComponent } from './components/app-messages/app-messages.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AppMessagesComponent
  ],
  imports: [
    RouterModule.forRoot(routes),
    BrowserModule,
    HttpClientModule
  ],
  providers: [
    LoggedUserService,
    MessageService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
