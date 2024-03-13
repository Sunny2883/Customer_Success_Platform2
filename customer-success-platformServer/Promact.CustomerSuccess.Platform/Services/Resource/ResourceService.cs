using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource;

namespace Promact.CustomerSuccess.Platform.Services.Resource
{
    public class ResourceService : CrudAppService<ProjectResources, ProjectResourcesDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProjectResourceDto, CreateUpdateProjectResourceDto>
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;
        private readonly IRepository<ProjectResources, Guid> _resourceRepository;

        public ResourceService(IRepository<ProjectResources, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _resourceRepository = repository;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }

        public override async Task<ProjectResourcesDto> CreateAsync(CreateUpdateProjectResourceDto input)
        {
            var resourceDto = await base.CreateAsync(input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Resource Created Alert",
                Body = $"Resource with ID {resourceDto.Id} has been created."
            };
            _emailService.SendEmail(emailDto);

            return resourceDto;
        }

        public override async Task<ProjectResourcesDto> UpdateAsync(Guid id, CreateUpdateProjectResourceDto input)
        {
            var resourceDto = await base.UpdateAsync(id, input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Resource Updated Alert",
                Body = $"Resource with ID {resourceDto.Id} has been updated."
            };
            _emailService.SendEmail(emailDto);

            return resourceDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve resource to get details before deletion
            var resource = await _resourceRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Resource Deleted Alert",
                Body = $"Resource with ID {id} and Name has been deleted."
            };
            _emailService.SendEmail(emailDto);
        }

        public async Task<List<ProjectResourcesDto>> GetResourcesByProjectIdAsync(Guid projectId)
        {
            var resources = await _resourceRepository.GetListAsync(r => r.ProjectId == projectId);
            return ObjectMapper.Map<List<ProjectResources>, List<ProjectResourcesDto>>(resources);
        }
    }
}
