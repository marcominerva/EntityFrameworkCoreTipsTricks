using System.Linq;

namespace Ef5TipsTricks.DataAccess
{
    public interface IDataContext
    {
        IQueryable<T> Get<T>(bool trackingChanges = false) where T : class;
    }
}
