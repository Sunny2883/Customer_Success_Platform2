import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MinuteMeetingComponent } from './minute-meeting.component';

describe('MinuteMeetingComponent', () => {
  let component: MinuteMeetingComponent;
  let fixture: ComponentFixture<MinuteMeetingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MinuteMeetingComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MinuteMeetingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
