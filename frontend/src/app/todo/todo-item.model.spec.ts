import { TodoItem } from './todo-item.model';

describe('TodoItem model', () => {
  it('should create a valid TodoItem', () => {
    const item: TodoItem = { id: 1, text: 'Test' };
    expect(item.id).toBe(1);
    expect(item.text).toBe('Test');
  });
});
