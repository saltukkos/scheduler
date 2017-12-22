import {Component, OnInit, Input} from '@angular/core';
import {Lesson} from "./lesson";

@Component({
  selector: 'app-lesson',
  templateUrl: 'lesson.component.html',
  styleUrls: ['lesson.component.css']
})
export class LessonComponent implements OnInit {
  @Input()
  item: Lesson;
  @Input()
  index: number;
  shownItem;

  ngOnInit(): void {
    this.shownItem = this.item;
  }
}
