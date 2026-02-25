using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vale.Tops.Domain;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source
{
    public interface IWriteRead<T> where T : Entity
    {
        T FindById(object id);
        IEnumerable<T> All();
        IEnumerable<T> Find(Func<T, bool> where, string include);
        IEnumerable<T> Find(Func<T, bool> where);
        void Save(T entity);
        void Edit(T entity);

        void EditBulk(List<T> entitys);
        void Delete(object id);
        void DeleteAll();
        void DeleteQuery(Func<T, bool> where);
        void DeleteList(IEnumerable<T> list);
    }
}
