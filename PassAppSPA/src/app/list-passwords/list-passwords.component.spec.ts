import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListPasswordsComponent } from './list-passwords.component';

describe('ListPasswordsComponent', () => {
  let component: ListPasswordsComponent;
  let fixture: ComponentFixture<ListPasswordsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListPasswordsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListPasswordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
