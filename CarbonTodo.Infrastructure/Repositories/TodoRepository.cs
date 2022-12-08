﻿using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;
using CarbonTodo.Infrastructure.Context;
using CarbonTodo.Infrastructure.Entities;

namespace CarbonTodo.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Todo>> GetAll()
        {
            var todos = _dbContext.Todos.OrderBy(t => t.Order);
            return ConvertToTodo(todos);
        }

        public async Task<Todo?> GetById(int id)
        {
            var todo = await _dbContext.Todos.FindAsync(id);

            return todo is null ? null : ConvertToTodo(todo);
        }

        public async Task<Todo> Add(string title)
        {
            var order = GenerateOrder();
            var todoData = new TodoData(0, title, false, order);

            var todo = await _dbContext.Todos.AddAsync(todoData);
            await _dbContext.SaveChangesAsync();

            return ConvertToTodo(todo.Entity);
        }
        
        private int GenerateOrder()
        {
            return GetMaxOrder() + 1;
        }

        private int GetMaxOrder()
        {
            return _dbContext.Todos.Max(t => (int?)t.Order) ?? 0;
        }


        public Task<Todo> Update(int id, string title, bool completed, int order)
        {
            throw new NotImplementedException();
        }

        public Task<Todo?> GetByOrder(int order)
        {
            throw new NotImplementedException();
        }

        private static Todo ConvertToTodo(TodoData todoData)
        {
            return new Todo(todoData.Id, todoData.Title, todoData.Completed, todoData.Order);
        }

        private IEnumerable<Todo> ConvertToTodo(IEnumerable<TodoData> todoData)
        {
            return todoData.Select(t => new Todo(t.Id, t.Title, t.Completed, t.Order));
        }
    }
}