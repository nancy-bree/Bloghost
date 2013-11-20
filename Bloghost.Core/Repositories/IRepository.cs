using System;
using System.Collections.Generic;
using Bloghost.Core.Entities;

namespace Bloghost.Core.Repositories
{
    public interface IRepository { }

    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        void Create(TEntity entity);

        TEntity Get(Guid id);

        IEnumerable<TEntity> GetAll();

        void Update(TEntity entity);

        void Delete(Guid id);

    }
}
