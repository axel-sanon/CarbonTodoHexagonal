﻿using CarbonTodo.Domain.Exceptions;
using CarbonTodo.Domain.Models;
using CarbonTodo.Domain.Repositories;

namespace CarbonTodo.Domain.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository todoRepository)
        {
            _repository = todoRepository;
        }

        public async Task<List<Todo>> FindAll()
        {
            return (await _repository.GetAll()).ToList();
        }

        public async Task<Todo> FindById(int id)
        {
            var todo = await _repository.GetById(id);

            if (todo is null)
            {
                throw new NotFoundEntityAppException(nameof(todo), id);
            }

            return todo;
        }

        public Task<Todo> Create(string title)
        {
            throw new NotImplementedException();
        }
    }
}