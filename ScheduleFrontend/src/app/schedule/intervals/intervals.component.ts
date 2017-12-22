import {trigger, transition, style, animate} from "@angular/animations";
import {Component} from "@angular/core";

@Component({
  selector: 'app-intervals',
  templateUrl: 'intervals.component.html',
  styleUrls: ['intervals.component.css'],
  animations: [
    trigger(
      'enterAnimation', [
        transition(':enter', [
          style({ opacity: 0, 'height': 0}),
          animate('500ms', style({opacity: 1, 'height': '192px'}))
        ]),
        transition(':leave', [
          style({ opacity: 1, 'height': '192px'}),
          animate('500ms', style({opacity: 0, 'height': 0}))
        ])
      ]
    )
  ],
})
export class IntervalsComponent {
  showIntervals = false;

  getStateMessage(): string {
    return this.showIntervals ? 'Скрыть' : 'Показать';
  }
}
