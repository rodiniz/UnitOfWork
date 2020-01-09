namespace UnitOfWork
{
    public interface IAdapter<T, M>
    {
        T convertFromModel(M model);

        M convertToModel(T entity);
    }
}