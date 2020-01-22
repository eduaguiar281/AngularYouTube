import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { YoutubeSearchComponent } from './youtube-search/youtube-search.component';
import { CanalComponent } from './canal/canal.component';
import { CanalDetailsComponent } from './canal-details/canal-details.component';
import { VideoComponent } from './video/video.component';
import { VideoDetailsComponent } from './video-details/video-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    YoutubeSearchComponent,
    CanalComponent,
    CanalDetailsComponent,
    VideoComponent,
    VideoDetailsComponent

  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'youtube-search', component: YoutubeSearchComponent },
      { path: 'canal', component: CanalComponent },
      { path: 'canal-details', component: CanalDetailsComponent },
      { path: 'video', component: VideoComponent },
      { path: 'video-details', component: VideoDetailsComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
