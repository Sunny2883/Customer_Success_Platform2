using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Promact.CustomerSuccess.Platform.Entities
{
    public class UserRole: AuditedEntity<Guid>
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Role")]
        public string RoleId { get; set; }

        public virtual Role? Role { get; set; }

        public virtual User? User { get; set; }

    }
}
