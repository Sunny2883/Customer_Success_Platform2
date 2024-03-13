using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectUpdate;

namespace Promact.CustomerSuccess.Platform.Services.ProjectUpdates
{
    public class ProjectUpdateService : CrudAppService<ProjectUpdate, ProjectUpdateDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProjectUpdateDto, CreateUpdateProjectUpdateDto>, IProjectUpdateService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;
        private readonly IRepository<ProjectUpdate, Guid> _projectUpdateRepository;

        public ProjectUpdateService(IRepository<ProjectUpdate, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _projectUpdateRepository = repository;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }

        public override async Task<ProjectUpdateDto> CreateAsync(CreateUpdateProjectUpdateDto input)
        {
            var projectUpdateDto = await base.CreateAsync(input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Update Created Alert",
                Body = $"Project update with ID {projectUpdateDto.Id} has been created."
            };
            _emailService.SendEmail(emailDto);

            return projectUpdateDto;
        }

        public override async Task<ProjectUpdateDto> UpdateAsync(Guid id, CreateUpdateProjectUpdateDto input)
        {
            var projectUpdateDto = await base.UpdateAsync(id, input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Update Updated Alert",
                Body = $"Project update with ID {projectUpdateDto.Id} has been updated."
            };
            _emailService.SendEmail(emailDto);

            return projectUpdateDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve project update to get details before deletion
            var projectUpdate = await _projectUpdateRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Update Deleted Alert",
                Body = $"Project update with ID {id}   has been deleted."
            };
            _emailService.SendEmail(emailDto);
        }

        public async Task<List<ProjectUpdateDto>> GetProjectUpdatesByProjectIdAsync(Guid projectId)
        {
            var projectUpdates = await _projectUpdateRepository.GetListAsync(pu => pu.ProjectId == projectId);
            return ObjectMapper.Map<List<ProjectUpdate>, List<ProjectUpdateDto>>(projectUpdates);
        }
    }
}
