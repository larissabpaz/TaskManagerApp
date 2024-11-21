public class TodoRepository
    {
        private readonly List<TaskDto> _todos = new();
        private int _nextId = 1;

        public List<TaskDto> GetAll() => _todos;

        public TaskDto? GetById(int id) => _todos.FirstOrDefault(todo => todo.Id == id);

        public void Add(TaskDto todo)
        {
            todo.Id = _nextId++;
            _todos.Add(todo);
        }

        public void Update(TaskDto updatedTodo)
        {
            var index = _todos.FindIndex(todo => todo.Id == updatedTodo.Id);
            if (index != -1)
            {
                _todos[index] = updatedTodo;
            }
        }

        public void Delete(int id) => _todos.RemoveAll(todo => todo.Id == id);

        public void ChangeStatus(int id, string status)
        {
            var todo = GetById(id);
            if (todo != null)
            {
                todo.Status = status;
            }
        }

        public List<TaskDto> GetByStatus(string status)
        {
            return _todos.Where(todo => todo.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
        }
}

