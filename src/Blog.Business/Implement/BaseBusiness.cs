using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        private readonly IBaseRepository<T> _baseRepository;

        public BaseBusiness(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }


        public bool Insert(T entity)
        {
            return _baseRepository.Insert(entity);
        }

      
    }
}