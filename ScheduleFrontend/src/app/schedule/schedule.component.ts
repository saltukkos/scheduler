import {Component, OnInit, Inject} from '@angular/core';
import {Lesson} from './lesson';
import {ScheduleService} from './schedule.service';
import {ActivatedRoute, Params, Router} from '@angular/router';

import 'rxjs/add/operator/switchMap';
import {trigger, transition, style, animate} from '@angular/animations';
import {PageScrollInstance, PageScrollService} from 'ng2-page-scroll';
import {DOCUMENT} from '@angular/common';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css'],
  animations: [
    trigger(
      'enterAnimation', [
        transition(':enter', [
          style({opacity: 0}),
          animate('500ms', style({opacity: 1}))
        ]),
        transition(':leave', [
          style({opacity: 1}),
          animate('500ms', style({opacity: 0}))
        ])
      ]
    )
  ],
})
export class ScheduleComponent implements OnInit {
  private lessons: Lesson[];
  currentLessons: Lesson[][];
  isOddWeek: boolean;
  private idToWeekday: Map<number, string> = new Map([
    [0, 'Понедельник'],
    [1, 'Вторник'],
    [2, 'Среда'],
    [3, 'Четверг'],
    [4, 'Пятница'],
    [5, 'Суббота']]);
  loading: boolean;
  selectedStudentName: string;

  constructor(private scheduleService: ScheduleService,
              private route: ActivatedRoute,
              private router: Router,
              private pageScrollService: PageScrollService,
              @Inject(DOCUMENT) private document: any) {
  }

  ngOnInit() {
    this.route.params
      .switchMap((params: Params) => {
        if ('id' in params) {
          if ('oddWeek' in params) {
            this.isOddWeek = params['oddWeek'] === 'true';
          } else {
            // detect current week number
            const date = new Date();
            date.setHours(0, 0, 0, 0);
            date.setDate(date.getDate() + 3 - (date.getDay() + 6) % 7);
            const week1 = new Date(date.getFullYear(), 0, 4);
            this.isOddWeek = 1 + Math.round(((date.getTime() - week1.getTime()) / 86400000
                - 3 + (week1.getDay() + 6) % 7) / 7) % 2 === 1;
          }
          this.loading = true;
          if (this.selectedStudentName === params['id']) {
            return Promise.resolve(this.lessons);
          }
          this.selectedStudentName = params['id'];
          return this.scheduleService.getLessons(this.selectedStudentName)
            .catch(e => {
              if (e.status === 404) {
                this.router.navigate(['/404']);
              }
              console.error(e);
            });
        }
        return Promise.resolve(null);
      })
      .subscribe(lessons => {
        this.lessons = lessons;
        setTimeout(() => this.loading = false, 400);
        this.showCurrentLessons();
      });
  }

  private showCurrentLessons() {
    if (this.lessons == null) {
      return;
    }
    this.currentLessons = this.scheduleService.groupByWeekDay(
      this.scheduleService.filterByWeek(this.lessons, this.isOddWeek)
    );
    const pageScrollInstance: PageScrollInstance = PageScrollInstance.simpleInstance(this.document, '#logo');
    this.pageScrollService.start(pageScrollInstance);
  }

  convertWeekdayToWord(id: number): string {
    return this.idToWeekday.get(id);
  }

  selectWeekParity(isOddWeek: boolean) {
    this.isOddWeek = isOddWeek;
    this.showCurrentLessons();
    this.router.navigate(['/schedule', this.selectedStudentName, {'oddWeek': this.isOddWeek}]);
  }
}
