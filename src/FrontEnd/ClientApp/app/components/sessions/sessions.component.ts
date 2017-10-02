import { Component, OnInit } from '@angular/core';
import { SessionsService } from './sessions.service';
import { Session } from './session';

@Component({
    selector: 'sessions',
    templateUrl: 'sessions.component.html'
})
export class SessionsComponent implements OnInit {
    sessions: Session[];

    constructor(private sessionService: SessionsService) { }

    getSessions(): void {
        this.sessionService
            .getSessions()
            .then(sessions => this.sessions = sessions);
      }

    ngOnInit() {
        this.getSessions();
    }

}