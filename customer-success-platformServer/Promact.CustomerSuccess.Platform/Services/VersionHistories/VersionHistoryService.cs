using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.VersionHistory;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.VersionHistories
{
    public class VersionHistoryService : CrudAppService<VersionHistory,
                                VersionHistoryDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateVersionHistoryDto,
                                UpdateVersionHistoryDto>,
                                IVersionHistoryService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;

        public VersionHistoryService(IRepository<VersionHistory, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }

        public override async Task<VersionHistoryDto> CreateAsync(CreateVersionHistoryDto input)
        {
            var versionHistoryDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Version History Created alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return versionHistoryDto;
        }

        public override async Task<VersionHistoryDto> UpdateAsync(Guid id, UpdateVersionHistoryDto input)
        {
            var versionHistoryDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Version History Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return versionHistoryDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Version History Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }
        public async Task<List<VersionHistoryDto>> GetVersionHistoriesByProjectIdAsync(Guid projectId)
        {
            var versionHistories = await Repository.GetListAsync(vh => vh.ProjectId == projectId);
            return ObjectMapper.Map<List<VersionHistory>, List<VersionHistoryDto>>(versionHistories);
        }
    }
}
