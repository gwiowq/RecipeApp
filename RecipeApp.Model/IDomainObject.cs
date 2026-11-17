namespace RecipeApp.Model
{
    /// <summary>
    /// Базовый интерфейс для всех сущностей.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>Уникальный идентификатор.</summary>
        int Id { get; set; }
    }
}