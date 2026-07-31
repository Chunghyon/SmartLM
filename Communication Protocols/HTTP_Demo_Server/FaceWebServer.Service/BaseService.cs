using FaceWebServer.Interface;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FaceWebServer.Utility;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using FaceWebServer.DB.Table;
using FaceWebServer.Utility.Extend;

namespace FaceWebServer.Service
{
    public class BaseService : IBaseService
    {
        public IMemoryCache Cache { get; set; }
        public UserDetail CurrentUser { get; set; }

        #region Identity
        protected DbContext Context { get; private set; }
        //public DbContext GetDbContext()
        //{
        //    return Context;
        //}

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="context"></param>
        public BaseService(DbContext context)
        {
            this.Context = context;
        }
        #endregion Identity

        #region Query
        public T Find<T>(int id) where T : class
        {
            return this.Context.Set<T>().Find(id);
        }

        /// <summary>
        /// 不应该暴露给上端使用者，尽量少用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        //[Obsolete("尽量避免使用，using 带表达式目录树的代替")]
        protected IQueryable<T> Set<T>() where T : class
        {
            return this.Context.Set<T>();
        }

        /// <summary>
        /// 这才是合理的做法，上端给条件，这里查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            if (funcWhere == null)
                return this.Context.Set<T>();
            else
                return this.Context.Set<T>().Where<T>(funcWhere);
        }

        public PageResult<T> QueryPage<T, S>(List<Expression<Func<T, bool>>> funcWheres, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true, bool whereIsAnd = true) where T : class
        {
            Expression<Func<T, bool>> oWhere = null;

            if (funcWheres != null)
            {
                if (funcWheres.Count > 0)
                {
                    oWhere = funcWheres[0];
                    for (int i = 1; i < funcWheres.Count; i++)
                    {
                        if (whereIsAnd)
                            oWhere = oWhere.And(funcWheres[i]);
                        else
                            oWhere = oWhere.Or(funcWheres[i]);
                    }

                }
            }

            return QueryPage(oWhere, pageSize, pageIndex, funcOrderby, isAsc);
        }


        public PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class
        {
            var list = this.Set<T>().AsNoTracking();
            if (funcWhere != null)
            {
                list = list.Where<T>(funcWhere);
            }
            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }
            PageResult<T> result = new PageResult<T>()
            {
                DataList = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }

        public IQueryable<T> MergeExpression<T>(IQueryable<T> list, List<Expression<Func<T, bool>>> funcWheres, bool whereIsAnd = true)
        {

            Expression<Func<T, bool>> oWhere = null;

            if (funcWheres != null)
            {
                if (funcWheres.Count > 0)
                {
                    oWhere = funcWheres[0];
                    for (int i = 1; i < funcWheres.Count; i++)
                    {
                        if (whereIsAnd)
                            oWhere = oWhere.And(funcWheres[i]);
                        else
                            oWhere = oWhere.Or(funcWheres[i]);
                    }

                }
            }
            if (oWhere != null)
            {
                list = list.Where(oWhere);
            }
            return list;
        }

        public PageResult<TResult> QueryPage<T, S, TResult>(
            Expression<Func<T, TResult>> funcSelect,
            List<Expression<Func<T, bool>>> funcWheres, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, 
            bool isAsc = true, bool whereIsAnd = true)
            where T : class
            where TResult : class
        {
            var list = this.Set<T>().AsNoTracking();




            list = MergeExpression(list, funcWheres, whereIsAnd);



            if (isAsc)
            {
                list = list.OrderBy(funcOrderby);
            }
            else
            {
                list = list.OrderByDescending(funcOrderby);
            }

            var retList = list.Select(funcSelect);

            PageResult<TResult> result = new PageResult<TResult>()
            {
                DataList = retList.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = list.Count()
            };
            return result;
        }

        #endregion

        #region Insert
        /// <summary>
        /// 即使保存  不需要再Commit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public T Insert<T>(T t) where T : class
        {
            this.Context.Set<T>().Add(t);
            this.Commit();//写在这里  就不需要单独commit  不写就需要
            return t;
        }


        public async Task<T> InsertAsync<T>(T t) where T : class
        {
            this.Context.Set<T>().Add(t);
            await this.CommitAsync();//写在这里  就不需要单独commit  不写就需要
            return t;
        }

        public IEnumerable<T> AddRange<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().AddRange(tList);
            this.Commit();//一个链接  多个sql
            return tList;
        }

        public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> tList) where T : class
        {
            this.Context.Set<T>().AddRange(tList);
            await this.CommitAsync();//一个链接  多个sql
            return tList;
        }

        
        #endregion

        #region Update
        /// <summary>
        /// 是没有实现查询，直接更新的,需要Attach和State
        /// 
        /// 如果是已经在context，只能再封装一个(在具体的service)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Update<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");

            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
        }

        public async Task UpdateAsync<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");

            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            this.Context.Entry<T>(t).State = EntityState.Modified;
            await this.CommitAsync();//保存 然后重置为UnChanged
        }

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        public void Update<T>(T t, Action<EntityEntry<T>> updatedef) where T : class
        {
            if (t == null) throw new Exception("t is null");
            if (updatedef == null) throw new Exception("updatedef is null");

            this.Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            var ent = this.Context.Entry<T>(t);
            updatedef(ent);
            //this.Context.Entry<T>(t).State = EntityState.Modified;
            this.Commit();//保存 然后重置为UnChanged
        }

        public void Update<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
                this.Context.Entry<T>(t).State = EntityState.Modified;
            }
            this.Commit();
        }

        public async Task UpdateListAsync<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                this.Context.Set<T>().Attach(t);
                this.Context.Entry<T>(t).State = EntityState.Modified;
            }
            await this.CommitAsync();
        }


        #endregion

        #region Delete
        /// <summary>
        /// 先附加 再删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public void Delete<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Attach(t);
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        /// <summary>
        /// 还可以增加非即时commit版本的，
        /// 做成protected
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        public void Delete<T>(int Id) where T : class
        {
            T t = this.Find<T>(Id);//也可以附加
            if (t == null) throw new Exception("t is null");
            this.Context.Set<T>().Remove(t);
            this.Commit();
        }

        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            //foreach (var t in tList)
            //{
            //    this.Context.Set<T>().Attach(t);
            //}
            this.Context.Set<T>().RemoveRange(tList);
            this.Commit();
        }
        #endregion

        #region Other
        public void Commit()
        {
            this.Context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await this.Context.SaveChangesAsync();
        }


        public IQueryable<T> ExcuteQuery<T>(string sql, params object[] parameters) where T : class
        {
            //return this.Context.Database.SqlQuery<T>(sql, parameters).AsQueryable();
            return this.Context.Set<T>().FromSqlRaw<T>(sql, parameters);
        }
        public async Task ExcuteAsync(string sql, params object[] parameters)
        {
            IDbContextTransaction trans = null;
            try
            {
                trans = this.Context.Database.BeginTransaction();
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                //this.Context.Database.ExecuteSqlCommand(sql, parameters);
                await trans.CommitAsync();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
        }

        public void Excute(string sql, params object[] parameters)
        {
            IDbContextTransaction trans = null;
            try
            {
                trans = this.Context.Database.BeginTransaction();
                this.Context.Database.ExecuteSqlRaw(sql, parameters);
                //this.Context.Database.ExecuteSqlCommand(sql, parameters);
                trans.Commit();
            }
            catch (Exception ex)
            {
                if (trans != null)
                    trans.Rollback();
                throw ex;
            }
        }

        public void ExcuteNoTransaction(string sql, params object[] parameters)
        {



                this.Context.Database.ExecuteSqlRaw(sql, parameters);


        }

        public virtual void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }


        /// <summary>
        /// 取消跟踪DbContext中所有被跟踪的实体
        /// </summary>
        public void DetachAll()
        {
            //循环遍历DbContext中所有被跟踪的实体
            while (true)
            {
                //每次循环获取DbContext中一个被跟踪的实体
                var currentEntry = Context.ChangeTracker.Entries().FirstOrDefault();

                //currentEntry不为null，就将其State设置为EntityState.Detached，即取消跟踪该实体
                if (currentEntry != null)
                {
                    //设置实体State为EntityState.Detached，取消跟踪该实体，之后dbContext.ChangeTracker.Entries().Count()的值会减1
                    currentEntry.State = EntityState.Detached;
                }
                //currentEntry为null，表示DbContext中已经没有被跟踪的实体了，则跳出循环
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 获取键值对操作对象
        /// </summary>
        /// <returns></returns>
        public DbSet<SystemKV> GetSystemKVDBSet()
        {
            return this.Context.Set<SystemKV>();
        }

        #endregion


        #region 日志
        public DbSet<UserLogModel> GetUserLogDBSet()
        {
            return this.Context.Set<UserLogModel>();
        }

        public void AddUserLog(string sType, string sDetail)
        {
            AddUserLog(sType, sDetail, string.Empty, string.Empty);
        }

        public void AddUserLog(string sType, string sDetail, string drive, string people)
        {
            var userdb = GetUserLogDBSet();
            if (CurrentUser == null || CurrentUser.UserID == 0)
            {
                CurrentUser = new UserDetail()
                {
                    UserID = 1,
                    UserName = "Auto"
                };
            }
            UserLogModel log = new UserLogModel(CurrentUser, sType, sDetail);
            log.LogDrive = drive;
            log.LogPeople = people;
            userdb.Add(log);
        }

        


        #endregion
    }
}
