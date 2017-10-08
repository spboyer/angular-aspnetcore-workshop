# Building the Front End

In this session, we'll add the code for the client application. Create a view for the conference sessions, details and speaker information.

## Create and wire-up the API using an Angular Service

> We'll create a class to talk to the ASP.NET Core Web API service

### Create the service

1. Create folder called **shared** in the FrontEnd/ClientApp/components/ folder
1. In this folder, add a new file called `data.service.ts` with the following code:

    ``` typescript
      import { Injectable } from '@angular/core';
      import { Headers, Http } from '@angular/http';

      import 'rxjs/add/operator/toPromise';

      import { Session, Speaker } from './model';

      @Injectable()
      export class DataService {

        private headers = new Headers({ 'Content-Type': 'application/json' });
        private sessionUrl = '/api/sessions';
        private speakerUrl = '/api/speakers';
        /**
         * init with Http
         */
        constructor(private http: Http) { }

        getSessions(): Promise<Session[]> {
          return this.http.get(this.sessionUrl)
              .toPromise()
            .then(response => <Session[]>response.json() )
            .catch(this.handleError);
        }

        getSession(id: number) : Promise<Session> {
          const url = `${this.sessionUrl}/${id}`
          return this.http.get(url)
            .toPromise()
            .then(response => <Session>response.json())
            .catch(this.handleError);
        }

        getSpeaker(id: number) : Promise<Speaker> {
          const url = `${this.speakerUrl}/${id}`
          return this.http.get(url)
            .toPromise()
            .then(response => <Speaker>response.json())
            .catch(this.handleError);
        }

        getSpeakers(): Promise<Speaker[]> {
          return this.http.get(this.speakerUrl)
            .toPromise()
            .then(response => <Speaker[]>response.json())
            .catch(this.handleError);
        }

        private getData(response: Response) { }
        private handleError(error: any): Promise<any> {
          console.error('An error occurred', error); // for demo purposes only
          return Promise.reject(error.message || error);
        }
      }
    ```

1. Staying in this folder, add a new file called `model.ts` with the following classes. These are used to map the json return from the Web API.

    ```typescript
      export class Track {
        trackID: number;
        conferenceID: number;
        name: string;
      }

      export class Speaker {
        id: number;
        name: string;
        bio?: any;
        webSite?: any;
        sessions?: Session[]
      }

      export class Session {
        track: Track;
        speakers: Speaker[];
        tags: any[];
        id: number;
        conferenceID: number;
        title: string;
        abstract: string;
        startTime: Date;
        endTime: Date;
        duration: string;
        trackId: number;
      }
    ```

## List the sessions

> Now that we have a service to talk to the API, we'll add the views to display a basic list of all sessions for the conference and validate the FrontEnd / API communication.

### Create and add the Sessions component

1. Add a new folder called **sessions**
1. In the same folder, create a new file called `sessions.component.ts`

    ```typescript
    import { Component, OnInit } from '@angular/core';
    import { Router } from '@angular/router';

    import { DataService } from '../shared/data.service';
    import { Session } from '../shared/model';

    @Component({
        selector: 'conf-sessions',
        templateUrl: './sessions.component.html'
    })
    export class SessionsComponent implements OnInit {
        sessions: Session[];

        constructor(private dataService: DataService) { }

        getSessions(): void {
            this.dataService
                .getSessions()
                .then(sessions => this.sessions = sessions);
        }

        ngOnInit() {
            this.getSessions();
        }
    }
    ```
1. Next add the template file `sessions.component.html`

    ```html
    <div class="agenda">
      <h1>My Conference 2017</h1>

      <p *ngIf="!sessions">
        <em>Loading...</em>
      </p>

      <div class="row">
        <div *ngFor="let session of sessions" class="col-md-3">
          <div class="panel panel-default session">
            <div class="panel-body">
              <p>{{session.track.name}}</p>
              <h3 class="panel-title">
                <a [routerLink]="['/sessiondetail', session.id]">{{session.title}}</a>
              </h3>
              <p *ngFor="let speaker of session.speakers">
                <em>
                  <a [routerLink]="['/speaker', speaker.id]">{{speaker.name}}</a>
                </em>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
    ```
1. Now that the components and data service is created, open `app.module.shared.ts` to import the newly created components. Add the following to the top to import the modules.

    ```typescript
    import { DataService } from './components/shared/data.service';
    import { SessionsComponent } from './components/sessions/sessions.component';

    ```
1. In the same file add the **SessionsComponent** in the `declarations` part of the **@NgModule**

1. Add the route for the sessions page to the **RouterModule**

    ```typescript
    { path: 'sessions', component: SessionsComponent },
    ```
1. Finally add the **DataService** as a provider after the imports.

    ```typescript
    providers: [DataService]
    ```

### Creating the Session Detail Page

> Now that we have a home page showing all the sessions, we'll create a page to show all the details of a specific session

1. Add a new folder called **sessiondetail**
1. In the same folder, create a new file called `sessiondetail.component.ts`

    ```typescript
    import 'rxjs/add/operator/switchMap';
    import { Component, OnInit } from '@angular/core';
    import { ActivatedRoute, ParamMap } from '@angular/router';
    import { Location }                 from '@angular/common';

    import { Session } from '../shared/model';
    import { DataService } from '../shared/data.service';

    @Component({
      selector: 'session-detail',
      templateUrl: './sessiondetail.component.html'
    })
    export class SessionDetailComponent implements OnInit {
      session: Session;

      constructor(
        private sessionService: DataService,
        private route: ActivatedRoute,
        private location: Location
      ) { }

      ngOnInit(): void {

        this.route.paramMap
          .switchMap((params: ParamMap) => this.sessionService.getSession(+params.get('id')))
          .subscribe(session => this.session = session);
      }

      goBack() {
        this.location.back();
      }

    }
    ```
1. Add a new file in the same folder called `sessiondetail.component.html` for the template

    ```html
    <ol class="breadcrumb">
        <li><a (click)="goBack()" style="cursor: pointer">Back</a></li>
        <li><a [routerLink]="['/sessions']">Agenda</a></li>
        <li class="active">{{session.title}}</li>
    </ol>

    <h1>{{session.title}}</h1>
    <span class="label label-default">{{session.track.name}}</span>

    <p *ngFor="let speaker of session.speakers">
        <em>
          <a [routerLink]="['/speaker', speaker.id]">{{speaker.name}}</a>
        </em>
      </p>

    <p>{{session.abstract}}</p>
    ```

1. As we did with the session page, add the component to the imports in `app.module.shared.ts`

    ```typescript
    import { SessionDetailComponent } from './components/sessiondetail/sessiondetail.component';
    ```
1. Add the **SessionDetailComponent** in the `declarations` part of the **@NgModule**
1. Add the route for the session detail page to the **RouterModule** also adding the `/:id` parameter to be passed along for the specific session.

    ```typescript
    { path: 'sessiondetail/:id', component: SessionDetailComponent }
    ```
### Create the Speakers Detail Page

> We'll add a page to show details for a given speaker

1. Add a new folder called **speakerdetail**
1. In the same folder, create a new file called `speakerdetail.component.ts`

    ```typescript
    import 'rxjs/add/operator/switchMap';
    import { Component, OnInit } from '@angular/core';
    import { ActivatedRoute, ParamMap } from '@angular/router';
    import { Location }                 from '@angular/common';

    import { DataService } from '../shared/data.service';
    import { Speaker } from '../shared/model';

    @Component({
      selector: 'conf-speakerdetail',
      templateUrl: './speakerdetail.component.html'
    })
    export class SpeakerDetailComponent implements OnInit {
      speaker: Speaker;

      constructor(
        private dataService: DataService,
        private route: ActivatedRoute,
        private location: Location
      ) { }

      ngOnInit() {
        this.route.paramMap
        .switchMap((params: ParamMap) => this.dataService.getSpeaker(+params.get('id')))
        .subscribe(speaker => this.speaker= speaker);
      }

      goBack() {
        this.location.back();
      }

    }
    ```
1. Add a new file in the same folder called `speakerdetail.component.html` for the template

    ```html
    <ol class="breadcrumb">
      <li><a (click)="goBack()" style="cursor: pointer">Back</a></li>
      <li><a [routerLink]="['/speakers']">Speakers</a></li>
      <li class="active">{{speaker.name}}</li>
    </ol>

    <h2>{{speaker.name}}</h2>

    <p>{{speaker.bio}}</p>

    <h3>Sessions</h3>
    <div class="row">
      <div class="col-md-5">
          <ul class="list-group">
            <li *ngFor="let session of speaker.sessions" class="list-group-item">
              <a [routerLink]="['/sessiondetail', session.id]">{{session.title}}</a>
            </li>
        </ul>
      </div>
    </div>
    ```

1. As we did with the session page, add the component to the imports in `app.module.shared.ts`

    ```typescript
    ```

1. Add the **SpeakerDetailComponent** in the `declarations` part of the **@NgModule**
1. Add the route for the speaker detail page to the **RouterModule** also adding the `/:id` parameter to be passed along for the specific session.

    ```typescript
    { path: 'speaker/:id', component: SpeakerDetailComponent }
    ```

### Challenge / Bonus

Add a Speakers Listing Page and link from to the `/speakerdetail` route