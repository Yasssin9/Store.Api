using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> BaseQuery, Specification<T> specification) where T : class 
        {
            var query = BaseQuery;
            if (specification.Criteria is not null)             
                query = query.Where(specification.Criteria);
            //Example on Criteria means ==> (x=>x.BrandId==3 && x=>x.TypeId==1)
            query = specification.Include.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);

            else if (specification.OrderByDescending is not null)
                query = query.OrderByDescending(specification.OrderByDescending);

            if(specification.IsPaginated)
                query=query.Skip(specification.Skip).Take(specification.Take);
           
                return query;
        }    
        

    }
}
