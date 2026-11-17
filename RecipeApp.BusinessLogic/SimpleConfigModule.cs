using Ninject.Modules;
using DataAccessLayer;
using RecipeApp.Model;

namespace RecipeApp.BusinessLogic
{
    /// <summary>
    /// Модуль конфигурации зависимостей.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {

        /// <summary>
        /// Загружает привязки интерфейсов к их реализациям.
        /// </summary>
        public override void Load()
        {
           
            Bind<IRepository<Recipe>>()
                .To<EntityRepository<Recipe>>()
                .InSingletonScope();

            
            Bind<IReadRepository<Recipe>>()
                .To<EntityRepository<Recipe>>()
                .InSingletonScope();

            Bind<IWriteRepository<Recipe>>()
                .To<EntityRepository<Recipe>>()
                .InSingletonScope();

            
            Bind<RecipeCrudService>().ToSelf().InSingletonScope();
            Bind<Logic>().ToSelf().InSingletonScope();
        }
    }
}