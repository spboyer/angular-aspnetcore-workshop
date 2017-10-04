import 'rxjs/add/operator/switchMap';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location }                 from '@angular/common';

import { SessionsService } from '../shared/data.service';
import { Speaker } from '../shared/model';

@Component({
  selector: 'speakerdetail',
  templateUrl: 'speakerdetail.component.html'
})
export class SpeakerDetailComponent implements OnInit {
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