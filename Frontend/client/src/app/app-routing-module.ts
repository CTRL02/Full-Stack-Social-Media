import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { MemeberProfile } from './memeber-profile/memeber-profile';
import { Messages } from './messages/messages';
import { Feed } from './feed/feed';
import { authGuard } from './guard/auth-guard';
import { Notfound } from './notfound/notfound';
import { ServerError } from './server-error/server-error';

const routes: Routes = [
  { path: '', component: Home },
  { path: 'members/:id', component: MemeberProfile },
  { path: 'messages', component: Messages, canActivate: [authGuard] },
  { path: 'feed', component: Feed, canActivate: [authGuard] },
  { path: 'not-found', component: Notfound },
  { path: 'server-error', component: ServerError},
  { path: '**', component: Notfound, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
