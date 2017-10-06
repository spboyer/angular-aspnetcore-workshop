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