import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Nav } from './nav/nav';
import {FormsModule} from '@angular/forms';
import { authModel } from './models/authmodel';
import { Home } from './home/home';
import { Footer } from './footer/footer';
import { MemeberProfile } from './memeber-profile/memeber-profile';
import { Messages } from './messages/messages';
import { Feed } from './feed/feed';
import { authGuard } from './guard/auth-guard';
import { SharedModule } from './shared-module';


@NgModule({
  declarations: [
    App,
    Nav,
    Home,
    Footer,
    MemeberProfile,
    Messages,
    Feed
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    SharedModule
    

  ],
  providers: [
    provideBrowserGlobalErrorListeners()
  ],
  bootstrap: [App]
})
export class AppModule { }
