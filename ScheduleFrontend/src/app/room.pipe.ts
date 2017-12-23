import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'room'
})
export class RoomPipe implements PipeTransform {

  transform(room: string): any {
    return room ? room.replace('Ауд. ', '').replace('Институт автоматики и электрометрии', 'ИАиЭ') : room;
  }

}
