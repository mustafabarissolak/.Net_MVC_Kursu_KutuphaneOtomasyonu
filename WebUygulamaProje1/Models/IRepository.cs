using System.Linq.Expressions;

namespace WebUygulamaProje1.Models
{
    public interface IRepository<T> where T : class
    {
        // T -> kitap türü
        IEnumerable<T> GetAll(string? includeProps = null);
        T Get(Expression<Func<T, bool>> filtre, string? includeProps = null);
        void Ekle(T entitiy);
        void Sil(T entitiy);
        void SilAralik(IEnumerable<T> entities);
    }
}
