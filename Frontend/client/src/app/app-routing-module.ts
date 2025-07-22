import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Home } from './home/home';
import { MemeberProfile } from './memeber-profile/memeber-profile';
import { Messages } from './messages/messages';
import { Feed } from './feed/feed';

const routes: Routes = [
  { path: '', component: Home },
  { path: 'members/:id', component: MemeberProfile },
  { path: 'messages', component: Messages },
  { path: 'feed', component: Feed },
  {path: '**', component: Home, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
