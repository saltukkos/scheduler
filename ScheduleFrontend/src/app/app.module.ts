import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { StudentsListComponent } from './students-list/students-list.component';
import {StudentService} from "./students-list/student.service";
import {HttpClientModule} from "@angular/common/http";
import { ScheduleComponent } from './schedule/schedule.component';
import {ScheduleService} from "./schedule/schedule.service";
import {AppRoutingModule} from "./app-routing.module";
import { LessonComponent } from './schedule/lesson.component';
import { RoomPipe } from './room.pipe';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatTooltipModule, MatButtonModule, MatProgressSpinnerModule} from "@angular/material";
import {FirstWordPipe} from "./first-word.pipe";
import { FooterComponent } from './footer/footer.component';
import {IntervalsComponent} from "./schedule/intervals/intervals.component";
import {Ng2PageScrollModule} from "ng2-page-scroll";
import { NotFoundComponent } from './not-found/not-found.component';

@NgModule({
  declarations: [
    AppComponent,
    StudentsListComponent,
    ScheduleComponent,
    LessonComponent,
    RoomPipe,
    FirstWordPipe,
    FooterComponent,
    IntervalsComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatTooltipModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    Ng2PageScrollModule
  ],
  providers: [StudentService, ScheduleService],
  bootstrap: [AppComponent]
})
export class AppModule { }
