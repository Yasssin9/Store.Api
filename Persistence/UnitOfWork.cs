﻿using Domain.Contracts;
using Domain.Entities;
using Persistence.Data;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _storeDbContext;
        private ConcurrentDictionary<string, Object> _repositories;
        public UnitOfWork(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
            _repositories = new();
        }
        public IGenrericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
            => (IGenrericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, x => new GenericRepository<TEntity, TKey>(_storeDbContext));

        public async Task<int> SaveChangesAsync()
        => await _storeDbContext.SaveChangesAsync();
    }
}
