import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { TodoItem } from './todo-item.model';

@Injectable({ providedIn: 'root' })
export class TodoService {
  update(id: number, text: string) {
    return this.http.put<TodoItem>(this.base, { id, text });
  }
  private base = `${environment.apiUrl}/todos`;

  constructor(private http: HttpClient) {}

  getTodos(): Observable<TodoItem[]> {
    return this.http.get<TodoItem[]>(this.base);
  }

  add(text: string) {
    return this.http.post<TodoItem>(this.base, { text });
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
