using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class EntityBase : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

        public DateTime CreationTime { get; private set; } = DateTime.UtcNow;

        public DateTime LastModifiedTime { get; private set; }

        public void ChangeLastModifiedTime(DateTime currentUtcTime)
        {
            LastModifiedTime = currentUtcTime;
        }

        public EntityBase() { }
    }
}
