using Promact.CustomerSuccess.Platform.Services.Dtos.ClientFeedback;
using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Promact.CustomerSuccess.Platform.Services.Dtos.PhaseMilestone;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectBudget;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource;
using Promact.CustomerSuccess.Platform.Services.Dtos.RiskProfile;
using Volo.Abp.Application.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Project
{
    public class ProjectDto : IEntityDto<Guid>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IEnumerable<DocumentDto>? Documents { get; set; }
        public IEnumerable<ProjectBudgetDto>? Budgets { get; set; }
        public IEnumerable<EscalationMatrixDto>? EscalationMatrices { get; set; }
        public IEnumerable<RiskProfileDto>? RiskProfiles { get; set; }
        public IEnumerable<PhaseMilestoneDto>? PhaseMilestones { get; set; }
        public IEnumerable<ProjectResourcesDto>? Resources { get; set; }
        public IEnumerable<ClientFeedbackDto>? ClientFeedbacks { get; set; }
        public IEnumerable<MeetingMinuteDto>? MeetingMinutes { get; set; }

    }
}
