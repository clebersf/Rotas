using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;
using System.Data.Entity;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager
{
    public class VS50ReadOnly<T> : IReadOnly<T> where T : Entity
    {
        internal VS50ReadOnlyContext context = new VS50ReadOnlyContext();

        public IEnumerable<T> All()
        {
            try
            {
                var ret = this.context.Set<T>().ToList();
                return ret;
            }
            catch (Exception ex)
            {
                var test = ex.InnerException;
                return new List<T>();
            }
        }

        public IEnumerable<T> Find(Func<T, bool> where)
        {
            try
            {
                return this.Find(where, null);
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public IEnumerable<T> Find(Func<T, bool> where, string include)
        {
            try
            {
                var set = this.context.Set<T>().AsQueryable();

                if (!string.IsNullOrEmpty(include))
                    set = set.Include(include);

                return set.Where(where);
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public T FindById(object id)
        {
            try
            {
                return this.context.Set<T>().Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }


    }
}