using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Stakeholders
{
    public class StakeholderService : CrudAppService<Stakeholder,
                                StakeholderDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateStakeholderDto,
                                UpdateStakeholderDto>,
                                IStakeholderService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username ;
        private readonly IRepository<Stakeholder, Guid> _stakeholderRepository;
        public StakeholderService(IRepository<Stakeholder, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
            _stakeholderRepository = repository;
        }

        public override async Task<StakeholderDto> CreateAsync(CreateStakeholderDto input)
        {
            var stakeholderDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Stakeholder Created alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return stakeholderDto;
        }

        public override async Task<StakeholderDto> UpdateAsync(Guid id, UpdateStakeholderDto input)
        {
            var stakeholderDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Stakeholder Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return stakeholderDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Stakeholder Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }

        public async Task<List<StakeholderDto>> GetStakeholdersByProjectIdAsync(Guid projectId)
        {
            var stakeholders = await _stakeholderRepository.GetListAsync(s => s.ProjectId == projectId);
            return ObjectMapper.Map<List<Stakeholder>, List<StakeholderDto>>(stakeholders);
        }
    }
}
