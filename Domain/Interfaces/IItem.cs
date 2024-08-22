namespace Domain.Interfaces
{
    public interface IItem
    {
        public Guid Id { get; }

        public Guid UserId { get; }
    }
}
