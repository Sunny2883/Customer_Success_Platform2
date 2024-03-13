using System.ComponentModel.DataAnnotations;

namespace Promact.CustomerSuccess.Platform.Services.Dtos
{
    public class CreateUpdateRoleDto
    {
        [Required]
        public string RoleName { get; set; }
        [Required]
        public string Description {  get; set; }
    }
}
