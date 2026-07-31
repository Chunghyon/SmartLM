using FaceWebServer.DB.Table;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FaceWebServer.Interface
{
    public interface IBaseService : IDisposable//是为了释放Context
    {
        //DbContext GetDbContext();

        #region Query
        /// <summary>
        /// 根据id查询实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Find<T>(int id) where T : class;

        /// <summary>
        /// 提供对单表的查询
        /// </summary>
        /// <returns>IQueryable类型集合</returns>
        //[Obsolete("尽量避免使用，using 带表达式目录树的 代替")]
        //IQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcWhere"></param>
        /// <returns></returns>
        IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class;

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        /// <param name="funcWhere"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        PageResult<T> QueryPage<T, S>(Expression<Func<T, bool>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true) where T : class;

        PageResult<T> QueryPage<T, S>(List<Expression<Func<T, bool>>> funcWhere, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true, bool whereIsAnd = true) where T : class;

        PageResult<TResult> QueryPage<T, S, TResult>(
    Expression<Func<T, TResult>> funcSelect,
    List<Expression<Func<T, bool>>> funcWheres, int pageSize, int pageIndex, Expression<Func<T, S>> funcOrderby, bool isAsc = true, bool whereIsAnd = true)
    where T : class
    where TResult : class;
        #endregion

        #region Add
        /// <summary>
        /// 新增数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        /// <returns>返回带主键的实体</returns>
        T Insert<T>(T t) where T : class;

        Task<T> InsertAsync<T>(T t) where T : class;

        /// <summary>
        /// 新增数据，即时Commit
        /// 多条sql 一个连接，事务插入
        /// </summary>
        /// <param name="tList"></param>
        IEnumerable<T> AddRange<T>(IEnumerable<T> tList) where T : class;

        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> tList) where T : class;
        #endregion

            #region Update
        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        void Update<T>(T t) where T : class;

        Task UpdateAsync<T>(T t) where T : class;

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        void Update<T>(T t, Action<EntityEntry<T>> updatedef) where T : class;

        /// <summary>
        /// 更新数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        void Update<T>(IEnumerable<T> tList) where T : class;

        Task UpdateListAsync<T>(IEnumerable<T> tList) where T : class;
        #endregion

            #region Delete
        /// <summary>
        /// 根据主键删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        void Delete<T>(int Id) where T : class;

        /// <su+mary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="t"></param>
        void Delete<T>(T t) where T : class;

        /// <summary>
        /// 删除数据，即时Commit
        /// </summary>
        /// <param name="tList"></param>
        void Delete<T>(IEnumerable<T> tList) where T : class;
        #endregion

        #region Other
        /// <summary>
        /// 立即保存全部修改
        /// 把增/删的savechange给放到这里，是为了保证事务的
        /// </summary>
        void Commit();

        /// <summary>
        /// 执行sql 返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<T> ExcuteQuery<T>(string sql, params object[] parameters) where T : class;

        /// <summary>
        /// 执行sql，无返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        void Excute(string sql, params object[] parameters);

        /// <summary>
        /// 获取键值对操作对象
        /// </summary>
        /// <returns></returns>
        DbSet<SystemKV> GetSystemKVDBSet();

        /// <summary>
        /// 添加一个用户操作日志
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sDetail"></param>
        void AddUserLog(string sType, string sDetail);

        /// <summary>
        /// 添加一个用户操作日志
        /// </summary>
        /// <param name="sType"></param>
        /// <param name="sDetail"></param>
        /// <param name="drive"></param>
        /// <param name="people"></param>
        void AddUserLog(string sType, string sDetail, string drive, string people);
        #endregion
    }
}


