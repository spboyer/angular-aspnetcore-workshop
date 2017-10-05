import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { DataService } from '../shared/data.service';
import { Speaker } from '../shared/model';

@Component({
  selector: 'conf-speakers',
  templateUrl: './speakers.component.html'
})
export class SpeakersComponent implements OnInit {
  speakers: Speaker[];

  constructor(private dataService: DataService) { }

  ngOnInit() {
    this.getSpeakers();
  }

  getSpeakers(): void {
    this.dataService
      .getSpeakers()
      .then(speakers => this.speakers = speakers);
  }

}