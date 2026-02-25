using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;
using System.Data.Entity;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source;
using System.Reflection;
using System.Runtime.Remoting.Contexts;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager
{
    public class WriteRead<T> : IWriteRead<T> where T : Entity
    {
        internal WriteReadContext context = new WriteReadContext();

        public IEnumerable<T> All()
        {
            try
            {
                var r = context.Set<T>().ToList();
                return r;
            }
            catch (Exception ex)
            {
                var f = ex.InnerException;
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

        public void Save(T entity)
        {
            try
            {
                this.context.Set<T>().Add(entity);
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                string s = "";
            }
        }


        public void Edit(T entity)
        {
            try
            {
                var o = entity;
                object Id = o.GetType().GetProperty("Id", typeof(Guid))?.GetValue(o, null);
                if (Id == null)
                {
                    Id = o.GetType().GetProperty("Id", typeof(long))?.GetValue(o, null);
                }
                context.Entry(this.context.Set<T>().Find(Id)).CurrentValues.SetValues(entity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                string g = "";
                // sem tratamento
            }
        }
        public void EditBulk(List<T> entitys)
        {
            try
            {
                foreach (var entity in entitys)
                {
                    object Id = entity.GetType().GetProperty("Id", typeof(long))?.GetValue(entity, null);
                    context.Entry(this.context.Set<T>().Find(Id)).CurrentValues.SetValues(entity);
                }
                context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string g = "";
                // sem tratamento
            }
        }

        public void Delete(object id)
        {
            try
            {
                this.context.Set<T>().Remove(this.context.Set<T>().Find(id));
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                //Sem tratamento
            }
        }

        public void DeleteAll()
        {
            try
            {
                this.context.Set<T>().RemoveRange(this.context.Set<T>().ToList());
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                //sem tratamento
            }
        }

        public void DeleteQuery(Func<T, bool> where)
        {
            try
            {
                this.context.Set<T>().RemoveRange(this.Find(where, null));
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                //sem tratamento
            }
}

        public void DeleteList(IEnumerable<T> list)
        {
            try
            {
                this.context.Set<T>().RemoveRange(list);
                this.context.SaveChanges();
            }
            catch (Exception)
            {
                //sem tratamento
            }
        }

    }

}