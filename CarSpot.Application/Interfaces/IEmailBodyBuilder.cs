public interface IEmailBodyBuilder<T>
{
    string Build(T entity);
}