namespace UnitOfWork
{
    using System.Threading.Tasks;
    public interface ICrudService<TEntity,TModel>
    {
        Task<TEntity> FindAsync(params object[] keyValues);
        void ValidateAndThrow(TModel model);
        Task<TEntity> InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(int id);

    }
}
