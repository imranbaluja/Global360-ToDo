import { TestBed } from '@angular/core/testing';
import { TodoService } from './todo.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';

const mockTodos = [
  { id: 1, text: 'Test 1' },
  { id: 2, text: 'Test 2' },
];

describe('TodoService', () => {
  let service: TodoService;
  let httpMock: HttpTestingController;
  const base = `${environment.apiUrl}/todos`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TodoService],
    });
    service = TestBed.inject(TodoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should get todos', () => {
    service.getTodos().subscribe((todos) => {
      expect(todos.length).toBe(2);
      expect(todos).toEqual(mockTodos);
    });
    const req = httpMock.expectOne(base);
    expect(req.request.method).toBe('GET');
    req.flush(mockTodos);
  });

  it('should add a todo', () => {
    const text = 'New Todo';
    service.add(text).subscribe((todo) => {
      expect(todo.text).toBe(text);
    });
    const req = httpMock.expectOne(base);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ text });
    req.flush({ id: 3, text });
  });

  it('should delete a todo', () => {
    service.delete(1).subscribe((res) => {
      expect(res).toBeNull();
    });
    const req = httpMock.expectOne(`${base}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
