import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TodoListComponent } from './todo-list.component';
import { TodoService } from './todo.service';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { TodoItem } from './todo-item.model';

class MockTodoService {
  update(id: number, text: string) {
    const todo = this.todos.find((t) => t.id === id);
    if (todo) {
      todo.text = text;
      return of({ id, text });
    }
    return of(undefined);
  }
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

  it('should show edit input and buttons when Edit is clicked', () => {
    expect(component.editIndex).toBeNull();
    component.todos = [
      { id: 1, text: 'Test Todo 1' },
      { id: 2, text: 'Test Todo 2' },
    ];
    fixture.detectChanges(); // Initial render with todos
    component.startEdit(0); // Enter edit mode
    fixture.detectChanges(); // Update DOM for edit mode
    expect(component.editIndex).toBe(0);
    expect(component.editText).toBe('Test Todo 1');
  });

  it('should call update and exit edit mode on saveEdit', () => {
    component.todos = [...todoService.todos];
    component.startEdit(0);
    component.editText = 'Updated';
    component.saveEdit();
    fixture.detectChanges();
    expect(component.editIndex).toBeNull();
    expect(component.todos[0].text).toBe('Updated');
  });

  it('should exit edit mode on cancelEdit', () => {
    component.startEdit(0);
    component.cancelEdit();
    expect(component.editIndex).toBeNull();
    expect(component.editText).toBe('');
  });

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
