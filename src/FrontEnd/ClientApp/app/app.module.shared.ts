import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { SessionsService } from './components/shared/data.service';
import { SessionsComponent } from './components/sessions/sessions.component';
import { SessionDetailComponent } from './components/sessiondetail/sessiondetail.component'
import { SpeakerComponent } from './components/speakers/speaker.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        SessionsComponent,
        SessionDetailComponent,
        SpeakerComponent
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: 'sessions', component: SessionsComponent },
            { path: 'sessiondetail/:id', component: SessionDetailComponent },
            { path: 'speaker/:id', component: SpeakerComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [SessionsService]
})
export class AppModuleShared {
}
