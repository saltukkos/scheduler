import { ScheduleService } from './schedule.service';

describe('Service: ScheduleService', () => {
  let service: ScheduleService;

  beforeEach(() => {
    service = new ScheduleService(null);
  });

  it('#filterByWeek should return an empty array when passed an empty array', () => {
    expect(service.filterByWeek([], true)).toEqual([]);
    expect(service.filterByWeek([], false)).toEqual([]);
  });

  it('#groupByWeekDay should return an array of size 6', () => {
    expect(service.groupByWeekDay([]).length).toEqual(6);
  });
});
