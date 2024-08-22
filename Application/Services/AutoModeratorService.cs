using Domain.Attribytes;
using Domain.Interfaces.DbContexts;
using Domain.Interfaces.Services;
using static Domain.CoreEnums.Enums;

namespace Application.Services
{
    public class AutoModeratorService : IAutoModerator
    {
        private readonly ICatalogDbContext _catalogContext;
        public AutoModeratorService(ICatalogDbContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        async public Task<KeyValuePair<ModerationResults, List<string>>> AutoModerateAsync<T>(T entity)
        {
            if (entity == null) return new KeyValuePair<ModerationResults, List<string>>(ModerationResults.ItemIsNull, new List<string>());
            var result = await CheckPropertiesAsync(entity);
            if (result.Key == true)
            {
                return new KeyValuePair<ModerationResults, List<string>>(ModerationResults.ForManualModeration, new List<string> { "Отправлено на модерацию" });
            }
            else
            {
                return new KeyValuePair<ModerationResults, List<string>>(ModerationResults.Denied, result.Value);
            }
        }

        async private Task<KeyValuePair<bool, List<string>>> CheckPropertiesAsync<T>(T entity)
        {
            var forReturn = new KeyValuePair<bool, List<string>>(true, new List<string>());

            var properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Public);
            foreach (var property in properties)
            {
                var validationAttribyte = property.CustomAttributes.Where(x => x.AttributeType == typeof(ItemPropertyValidationTypeAttribyte)).FirstOrDefault();
                if (validationAttribyte == null) continue;
                var validationAttribyteNamedArg = validationAttribyte.NamedArguments.Where(x => x.TypedValue.ArgumentType == typeof(ItemPropertyValidationType));
                if (!validationAttribyteNamedArg.Any()) continue;
                switch (validationAttribyteNamedArg.First().TypedValue.Value)
                {
                    case ItemPropertyValidationType.ByRulesForUserTypedText:
                        break;

                    case ItemPropertyValidationType.ByStaticList:
                        break;
                }
            }

            return forReturn;
        }

    }
}
