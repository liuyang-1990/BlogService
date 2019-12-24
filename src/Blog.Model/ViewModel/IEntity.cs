namespace Blog.Model.ViewModel
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
