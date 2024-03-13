using System;
using System.Threading.Tasks;
using AutoMapper;
using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectBudget;
using Promact.CustomerSuccess.Platform.Services.Emailing; // Import the email service namespace
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.ProjectBudgets
{
    public class ProjectBudgetService : CrudAppService<ProjectBudget,
                                ProjectBudgetDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateProjectBudgetDto,
                                UpdateProjectBudgetDto>,
                                IProjectBudgetService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username; 
        private readonly IRepository<ProjectBudget,Guid> _projectBudgetRepository;


        public ProjectBudgetService(IRepository<ProjectBudget, Guid> projectBudgetRepository, IEmailService emailService)
            : base(projectBudgetRepository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
            _projectBudgetRepository = projectBudgetRepository;

        }

        public override async Task<ProjectBudgetDto> CreateAsync(CreateProjectBudgetDto input)
        {
            var projectBudgetDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Budget Created alert",
                Body = Template.GetEmailTemplate(Username) 
            };
            _emailService.SendEmail(emailDto);

            return projectBudgetDto;
        }

        public override async Task<ProjectBudgetDto> UpdateAsync(Guid id, UpdateProjectBudgetDto input)
        {
            var projectBudgetDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Budget Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return projectBudgetDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Project Budget Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }

        public async Task<List<ProjectBudget>> GetProjectBudgetByProjectId(Guid projectId)
        {
            return await _projectBudgetRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
