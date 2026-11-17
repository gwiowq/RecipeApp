using System.Collections.Generic;
using RecipeApp.Model;

namespace DataAccessLayer
{
    /// <summary>
    /// Полный интерфейс репозитория для CRUD-операций.
    /// Наследует методы чтения и записи.
    /// </summary>
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : IDomainObject
    {
        // методы наследуются от IReadRepository и IWriteRepository
    }

    /// <summary>
    /// Интерфейс для чтения данных.
    /// </summary>
    public interface IReadRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Получить все записи из базы данных.
        /// </summary>
        /// <returns>Список объектов.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Получить запись по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи.</param>
        /// <returns>Объект или null, если не найден.</returns>
        T? ReadById(int id);
    }

    /// <summary>
    /// Интерфейс для изменения данных.
    /// </summary>
    public interface IWriteRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавить новую запись в базу данных.
        /// </summary>
        /// <param name="entity">Объект для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Обновить существующую запись.
        /// </summary>
        /// <param name="entity">Объект с обновленными данными.</param>
        void Update(T entity);

        /// <summary>
        /// Удалить запись по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи.</param>
        void Delete(int id);
    }
}