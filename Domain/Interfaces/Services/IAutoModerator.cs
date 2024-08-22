using static Domain.CoreEnums.Enums;

namespace Domain.Interfaces.Services
{
    public interface IAutoModerator
    {
        public Task<KeyValuePair<ModerationResults, List<string>>> AutoModerateAsync<T>(T Item);
    }
}
