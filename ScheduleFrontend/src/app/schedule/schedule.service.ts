import {Injectable} from '@angular/core';
import {environment} from '../../environments/environment';
import {Lesson} from './lesson';
import {HttpClient} from '@angular/common/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ScheduleService {
  private lessonsResource = environment.apiRoot + 'schedule/';

  constructor(private http: HttpClient) {
  }

  getLessons(studentName: string): Promise<Lesson[]> {
    return this.http.get(this.lessonsResource + '?student=' + studentName).toPromise();
  }

  groupByWeekDay(lessons: Lesson[]): Lesson[][] {
    const groupedLessons = new Array<Lesson[]>(6);
    let maxLessons = 0;
    for (let i = 0; i < 6; ++i) {
      groupedLessons[i] = [];
    }
    for (const lesson of lessons) {
      groupedLessons[lesson.WeekDay - 1].push(lesson);
      maxLessons = Math.max(maxLessons, lesson.Number - 1);
    }
    groupedLessons.forEach(lessons1 => {
      const missed = new Set<number>();
      for (let i = 0; i <= maxLessons; ++i) {
        missed.add(i);
      }
      for (const lesson of lessons1) {
        missed.delete(lesson.Number - 1);
      }
      missed.forEach(index => lessons1.splice(index, 0, null));
    });
    return groupedLessons;
  }

  filterByWeek(lessons: Lesson[], odd: boolean): Lesson[] {
    return lessons.filter(lesson => lesson.IsOddWeek === true && odd ||
                                    lesson.IsOddWeek === false && !odd ||
                                    lesson.IsOddWeek === null);
  }
}
