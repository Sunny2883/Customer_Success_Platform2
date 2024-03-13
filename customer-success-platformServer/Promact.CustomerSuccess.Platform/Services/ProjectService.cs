using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.Project;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services
{
    public class ProjectService : CrudAppService<
                                Project,
                                ProjectDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateProjectDto,
                                UpdateProjectDto>,
                                IProjectService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail = "pankajkumarnikk@gmail.com"; 
        private readonly string Username = "Pankaj Kumar";

        public ProjectService(IRepository<Project, Guid> projectRepository, IEmailService emailService) : base(projectRepository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }
        public override async Task<ProjectDto> CreateAsync(CreateProjectDto input)
        {
            var projectDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Created alert",
                Body = Template.GetEmailTemplate(Username) 
            };
            _emailService.SendEmail(emailDto);

            return projectDto;
        }

        public override async Task<ProjectDto> UpdateAsync(Guid id, UpdateProjectDto input)
        {
            var projectDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return projectDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }
    }
}
