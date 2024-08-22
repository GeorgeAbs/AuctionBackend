using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Interfaces
{
    public interface IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public Guid Id { get; }

        public DateTime CreationTime { get; }

        public DateTime LastModifiedTime { get; }

        public void ChangeLastModifiedTime(DateTime currentUtcTime);
    }
}
