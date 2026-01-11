import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoService } from './todo.service';
import { TodoItem } from './todo-item.model';

@Component({
  selector: 'app-todo-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './todo-list.component.html',
  styleUrls: ['./todo-list.component.css'],
})
export class TodoListComponent implements OnInit {
  todos: TodoItem[] = [];
  newTodo: string = '';
  errorMessage: string = '';

  constructor(private todoService: TodoService) {}

  ngOnInit() {
    this.loadTodos();
  }

  loadTodos() {
    this.todoService.getTodos().subscribe({
      next: (todos) => (this.todos = todos),
      error: (err) => console.error('Failed to load todos', err),
    });
  }

  addTodo() {
    const text = this.newTodo.trim();
    if (!text) return;
    this.errorMessage = '';
    this.todoService.add(text).subscribe({
      next: (todo) => {
        this.todos.push(todo);
        this.newTodo = '';
      },
      error: (err) => {
        console.error('Add todo error:', err);
        const status = err.status || (err.error && err.error.status);
        // Always try to extract message from err.error.message if present
        if (err.error && typeof err.error === 'object') {
          if ('message' in err.error) {
            this.errorMessage = err.error.message;
          } else if (
            err.error.errors &&
            err.error.errors.Text &&
            err.error.errors.Text.length > 0
          ) {
            this.errorMessage = err.error.errors.Text[0];
          } else {
            this.errorMessage = 'Failed to add todo.';
          }
        } else if (typeof err.error === 'string') {
          this.errorMessage = err.error;
        } else if (status === 409) {
          this.errorMessage = 'Duplicate todo not allowed.';
        } else {
          this.errorMessage = 'Failed to add todo.';
        }
      },
    });
  }

  deleteTodo(index: number) {
    const todo = this.todos[index];
    if (!todo || todo.id == null) return;
    this.todoService.delete(todo.id).subscribe({
      next: () => this.todos.splice(index, 1),
      error: (err) => console.error('Failed to delete todo', err),
    });
  }
}
