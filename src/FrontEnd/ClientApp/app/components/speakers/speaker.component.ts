import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location }                 from '@angular/common';

import { SessionsService } from '../sessions/sessions.service';
import { Speaker } from '../sessions/session';

@Component({
  selector: 'speakers',
  templateUrl: 'speaker.component.html'
})
export class SpeakerComponent implements OnInit {
  speaker: Speaker;

  constructor(
    private sessionService: SessionsService,
    private route: ActivatedRoute,
    private location: Location
  ) { }

  ngOnInit() {
    this.route.paramMap
    .switchMap((params: ParamMap) => this.sessionService.getSpeaker(+params.get('id')))
    .subscribe(speaker => this.speaker= speaker);
  }

}