import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KitchendashboardComponent } from './kitchendashboard.component';

describe('KitchendashboardComponent', () => {
  let component: KitchendashboardComponent;
  let fixture: ComponentFixture<KitchendashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [KitchendashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KitchendashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
