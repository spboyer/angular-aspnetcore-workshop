import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { SessionsService } from '../shared/data.service';
import { Session } from '../shared/model';

@Component({
    selector: 'sessions',
    templateUrl: 'sessions.component.html'
})
export class SessionsComponent implements OnInit {
    sessions: Session[];
    days: string[];

    constructor(private sessionService: SessionsService) { }

    getSessions(): void {
        this.sessionService
            .getSessions()
            .then(sessions => this.sessions = sessions)
            .then(sessions => this.getDays(this.sessions));
    }

    ngOnInit() {
        this.getSessions();
    }

    getDays(sessions: Session[]) {
        var dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
        this.days = [];

        this.sessions.forEach((s) => {
            var d = new Date(s.startTime);
            var dayName = dayNames[d.getDay()];
            if (this.days.indexOf(dayName) < 0) {
                this.days.push(dayName);
            }
        })
    }

}