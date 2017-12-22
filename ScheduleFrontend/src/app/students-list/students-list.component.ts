import {Component, OnInit, Input, OnChanges, SimpleChanges} from '@angular/core';
import {StudentService} from "./student.service";
import {Student} from "./student";
import {Group} from "./group";
import {Router} from "@angular/router";

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  styleUrls: ['./students-list.component.css']
})
export class StudentsListComponent implements OnInit, OnChanges {
  groups: Group[];
  selectedStudent: Student;
  @Input()
  selectedStudentName: string;

  constructor(private studentService: StudentService,
              private router: Router) {
  }

  private static groupByGroup(students: Student[]): Group[] {
    let groups = new Map<string, Group>();
    for (let student of students) {
      if (!groups.has(student.Group)) {
        groups.set(student.Group, new Group(student.Group));
      }
      groups.get(student.Group).students.push(student);
    }
    return Array.from(groups.values()).sort((x, y) => x.name > y.name ? 1 : -1);
  }

  ngOnInit() {
    this.studentService.getStudents()
      .then(students => {
        this.groups = StudentsListComponent.groupByGroup(
          this.studentService.sortByLastname(students)
        );
        if (this.selectedStudent == null && this.selectedStudentName) {
          this.selectStudentByName(this.selectedStudentName);
        }
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.selectedStudentName === undefined || this.groups === undefined) return;
    if ('selectedStudentName' in changes) {
      this.selectStudentByName(this.selectedStudentName);
    }
  }

  private selectStudentByName(name: string) {
    for (let group of this.groups) {
      for (let student of group.students) {
        if (student.Name == name) {
          this.selectedStudent = student;
          return;
        }
      }
    }
  }

  selectStudent(student: Student) {
    this.router.navigate(['/schedule/', student.Name]);
    this.selectedStudent = student;
  }

  getSelectedColor(student: Student) {
    if (this.selectedStudent == student) {
      return 'accent';
    }
    return '';
  }
}
