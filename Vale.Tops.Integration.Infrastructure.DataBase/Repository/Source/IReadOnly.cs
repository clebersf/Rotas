using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vale.Tops.Domain;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source
{
    public interface IReadOnly<T> where T : Entity
    {
        T FindById(object id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Func<T, bool> where, string include);
        IEnumerable<T> Find(Func<T, bool> where);
    }
}
