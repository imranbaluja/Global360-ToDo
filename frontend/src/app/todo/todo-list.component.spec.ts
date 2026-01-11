import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TodoListComponent } from './todo-list.component';
import { TodoService } from './todo.service';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TodoItem } from './todo-item.model';

class MockTodoService {
  todos: TodoItem[] = [
    { id: 1, text: 'Test Todo 1' },
    { id: 2, text: 'Test Todo 2' },
  ];
  getTodos() {
    return of(this.todos);
  }
  add(text: string) {
    return of({ id: 3, text });
  }
  delete(id: number) {
    return of(undefined);
  }
}

describe('TodoListComponent', () => {
  it('should disable Add button when input is empty or whitespace', () => {
    component.newTodo = '';
    fixture.detectChanges();
    let button = fixture.nativeElement.querySelector('button');
    expect(button.disabled).toBeTrue();

    component.newTodo = '   ';
    fixture.detectChanges();
    button = fixture.nativeElement.querySelector('button');
    expect(button.disabled).toBeTrue();

    component.newTodo = 'valid todo';
    fixture.detectChanges();
    button = fixture.nativeElement.querySelector('button');
    expect(button.disabled).toBeFalse();
  });
  let component: TodoListComponent;
  let fixture: ComponentFixture<TodoListComponent>;
  let todoService: MockTodoService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TodoListComponent, FormsModule, CommonModule],
      providers: [{ provide: TodoService, useClass: MockTodoService }],
    }).compileComponents();
    fixture = TestBed.createComponent(TodoListComponent);
    component = fixture.componentInstance;
    todoService = TestBed.inject(TodoService) as any;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load todos on init', () => {
    expect(component.todos.length).toBe(2);
  });

  it('should add a todo', () => {
    component.newTodo = 'New Todo';
    component.addTodo();
    expect(component.todos.length).toBe(3);
    expect(component.todos[2].text).toBe('New Todo');
  });

  it('should not add empty todo', () => {
    component.newTodo = '   ';
    component.addTodo();
    expect(component.todos.length).toBe(2);
  });

  it('should delete a todo', () => {
    component.deleteTodo(0);
    expect(component.todos.length).toBe(1);
    expect(component.todos[0].id).toBe(2);
  });

  it('should handle add error', () => {
    spyOn(todoService, 'add').and.returnValue(
      throwError(() => ({ error: { message: 'Failed to add' } }))
    );
    component.newTodo = 'Error Todo';
    component.addTodo();
    expect(component.errorMessage).toBe('Failed to add');
  });
});
