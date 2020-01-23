namespace UnitOfWork
{
    public interface IAdapter<T, M>
    {
        T Adapt(M model);

        M Adapt(T entity);
    }
}