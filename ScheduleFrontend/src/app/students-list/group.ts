import {Student} from './student';
export class Group {
  name: string;
  students: Student[];

  constructor(name: string) {
    this.name = name;
    this.students = [];
  }
}
