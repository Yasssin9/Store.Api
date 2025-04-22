using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public abstract class Specification<T>where T : class
    {
        protected Specification(Expression<Func<T,bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Include { get; } = new();//Empty List to get ready when i call it

        protected void AddInclude(Expression<Func<T, object>> expression)
        {
            Include.Add(expression);
        }
    }
}
