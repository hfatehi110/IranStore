using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsynce();

        Task<IReadOnlyList<T>> GetAsynce(Expression<Func<T, bool>> predicate);

        Task<IReadOnlyList<T>> GetAsynce(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true);

        Task<IReadOnlyList<T>> GetAsynce(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> include = null,
            string includeString = null,
            bool disableTraching = true);
        Task<T> GetByIdAsynce(int id); 
        Task<T> AddAsynce(T entity);
        Task UpdateAsynce(T entity);
        Task DeleteAsynce(T entity);

    }
}
