using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using YouTubeApp.Core.Models;

namespace YouTubeApp.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IMongoCollection<T> Collection { get; }

        IMongoDatabase Database { get; }

        /// <summary>
        /// Obtem por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Entity</returns>
        T GetById(string id);

        /// <summary>
        /// Obtem por Identificador Assincrono
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Entity</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Insere Entidade
        /// </summary>
        /// <param name="entity">Entity</param>
        T Insert(T entity);

        /// <summary>
        /// Insere entidade assincrono
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// Insere várias entidades
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// Insere várias entidades Assincrono
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Altera entidade
        /// </summary>
        /// <param name="entity">Entity</param>
        T Update(T entity);

        /// <summary>
        /// Altera entidade assíncrono
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Altera várias entidades
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<T> entities);

        /// <summary>
        /// Altera várias entidades assincrono
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Deleta entidade 
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(T entity);

        /// Deleta entidade Assincrono
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> DeleteAsync(T entity);

        /// <summary>
        /// Deleta entidades
        /// </summary>
        /// <param name="entities">Entities</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Deleta entidades assincrono
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Determina se uma lista contém algum elemento
        /// </summary>
        /// <returns></returns>
        bool Any();

        /// <summary>
        /// Determina se uma lista contém algum elemento que satisfaça a condição.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Any(Expression<Func<T, bool>> where);

        /// <summary>
        /// Determina se uma lista contém algum elemento Assincrono
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();

        /// <summary>
        /// Determina se uma lista contém algum elemento que satisfaça a condição. Assincrono
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Retorna o numero de elementos da coleção.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Retorna o número de elementos da coleção que satisfaça a condição.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        long Count(Expression<Func<T, bool>> where);

        /// <summary>
        /// Retorna o numero de elementos da coleção Assincrono
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// Retorna o número de elementos da coleção que satisfaça a condição Assincrono.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Obtem uma tablela
        /// </summary>
        IMongoQueryable<T> Table { get; }

        /// <summary>
        /// Obter coleção por definições de filtro
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<T> FindByFilterDefinition(FilterDefinition<T> query);

        Task<T> GetSingleAsync();
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate);

    }
}
