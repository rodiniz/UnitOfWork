using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;

namespace UnitOfWork
{
    public class CrudService<TEntity, TModel> : ICrudService<TEntity, TModel> where TEntity:class
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        public CrudService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<TEntity>();
        }
        public async Task DeleteAsync(int id)
        {
            _repository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TEntity> FindAsync(params object[] keyValues)
        {
           return  await _repository.FindAsync(keyValues);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _repository.InsertAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public void ValidateAndThrow(TModel model)
        {
            throw new NotImplementedException();
        }
    }
}
