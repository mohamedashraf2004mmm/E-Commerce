using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Specifications
{
    internal class SpecificationEvaluator
    {
        //spec => query
        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> inputQuery , ISpecifications<TEntity,TKey>spec) where TEntity : BaseEntity<TKey>
        {
            //entryPoint

            var query = inputQuery;
            if(spec.Condition != null)
            {
                query = query.Where(spec.Condition);
            }

            if (spec.IncludeExpressions.Any())
            {
                //foreach (var expression in spec.IncludeExpressions)
                //{
                //    query = query.Include(expression);
                //} 
                query = spec.IncludeExpressions.Aggregate(query, (current, nextexp) => current.Include(nextexp));
            }
            return query;
        }
    }
}
