using System.Collections.Concurrent;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class InMemoryTodoService
    {
        private readonly ConcurrentDictionary<int, TodoItem> _store = new();
        private int _nextId = 0;

        public IEnumerable<TodoItem> GetAll() => _store.Values.OrderBy(t => t.Id);

        public TodoItem? Add(string text)
        {
            // Prevent duplicates (case-insensitive)
            if (_store.Values.Any(t => t.Text.Equals(text, StringComparison.OrdinalIgnoreCase)))
                return null; // Return null if a duplicate is found

            var id = Interlocked.Increment(ref _nextId);
            var item = new TodoItem(id, text);
            _store[id] = item;
            return item;
        }

        public bool Delete(int id) => _store.TryRemove(id, out _);
    }
}
