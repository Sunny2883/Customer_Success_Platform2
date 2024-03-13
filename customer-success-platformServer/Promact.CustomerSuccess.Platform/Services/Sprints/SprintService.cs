using Castle.Core.Smtp;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.sprint;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Sprints
{
    public class SprintService : CrudAppService<Sprint, SprintDto, Guid,
        PagedAndSortedResultRequestDto, CreateSprintDto, UpdateSprintDto>
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username ;
        private readonly IRepository<Sprint, Guid> _sprintRepository;

        public SprintService(IRepository<Sprint, Guid> repository,IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }


        public override async Task<SprintDto> CreateAsync(CreateSprintDto input)
        {
          
            var sprintDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Sprint Created alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return sprintDto;
        }

        public override async Task<SprintDto> UpdateAsync(Guid id, UpdateSprintDto input)
        {
            var sprintDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To =Useremail,
                Subject = "Sprint Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return sprintDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // You can perform any additional operations before deleting the entity if needed
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Sprint deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }
        public async Task<List<SprintDto>> GetSprintsByProjectIdAsync(Guid projectId)
        {
            var sprints = await _sprintRepository.GetListAsync(s => s.ProjectId == projectId);
            return ObjectMapper.Map<List<Sprint>, List<SprintDto>>(sprints);
        }


    }
}
