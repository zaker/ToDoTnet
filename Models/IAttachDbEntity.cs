
namespace ToDoTnet.Models
{
    public interface IAttachDbEntity<T>
    {
        void AttachDbEntity(T ent);
    }
}
