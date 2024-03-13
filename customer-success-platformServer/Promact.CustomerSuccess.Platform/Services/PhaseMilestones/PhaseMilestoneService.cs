using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.PhaseMilestone;
using Promact.CustomerSuccess.Platform.Services.Emailing; // Import the email service namespace
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.PhaseMilestones
{
    public class PhaseMilestoneService : CrudAppService<PhaseMilestone,
                                PhaseMilestoneDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreatePhaseMilestoneDto,
                                UpdatePhaseMilestoneDto>,
                                IPhaseMilestoneService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail; 
        private readonly string Username;
        IRepository<PhaseMilestone, Guid> _phaseMilestoneRepository;
        public PhaseMilestoneService(IRepository<PhaseMilestone, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
            _phaseMilestoneRepository = repository;
        }

        public override async Task<PhaseMilestoneDto> CreateAsync(CreatePhaseMilestoneDto input)
        {
            var phaseMilestoneDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Phase Milestone Created alert",
                Body = Template.GetEmailTemplate(Username) 
            };
            _emailService.SendEmail(emailDto);

            return phaseMilestoneDto;
        }

        public override async Task<PhaseMilestoneDto> UpdateAsync(Guid id, UpdatePhaseMilestoneDto input)
        {
            var phaseMilestoneDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Phase Milestone Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return phaseMilestoneDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Phase Milestone Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }


        public async Task<List<PhaseMilestone>> GetPhaseMilestoneByProjectIdAsync(Guid projectId)
        {
            return await _phaseMilestoneRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
