using System.Linq;

namespace CR.ViewModels.Core
{
    public interface IViewModelReader
    {
        TEntity GetByKey<TEntity>(string key) where TEntity : class;
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    }
}