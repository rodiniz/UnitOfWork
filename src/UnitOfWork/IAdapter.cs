namespace UnitOfWork
{
    public interface IAdapter<M, T>
    {
        T convertFromModel(M model);
    }
}
