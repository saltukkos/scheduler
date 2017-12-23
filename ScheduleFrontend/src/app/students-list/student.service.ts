import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';

import 'rxjs/add/operator/toPromise';
import {environment} from '../../environments/environment';
import {Student} from './student';

@Injectable()
export class StudentService {
  private studentsResource = environment.apiRoot + 'students/';
  private students: Student[];

  constructor(private http: HttpClient) { }

  getStudents(): Promise<Student[]> {
    if (this.students != null) {
      return Promise.resolve(this.students);
    }
    return this.http.get(this.studentsResource)
      .toPromise()
      .then(students => this.students = students as Student[]);
  }

  sortByLastname(students: Student[]): Student[] {
    return students.sort((x, y) => x.Name > y.Name ? 1 : -1);
  }
}
