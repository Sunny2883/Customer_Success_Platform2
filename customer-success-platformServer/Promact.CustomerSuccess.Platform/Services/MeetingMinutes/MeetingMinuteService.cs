using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.MeetingMinutes
{
    public class MeetingMinuteService : CrudAppService<MeetingMinute, MeetingMinuteDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateMeetingMinuteDto, CreateUpdateMeetingMinuteDto>, IMeetingMinuteService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;
        private readonly IRepository<MeetingMinute, Guid> _meetingMinuteRepository;

        public MeetingMinuteService(IRepository<MeetingMinute, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _meetingMinuteRepository = repository;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }

        public override async Task<MeetingMinuteDto> CreateAsync(CreateUpdateMeetingMinuteDto input)
        {
            var meetingMinuteDto = await base.CreateAsync(input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Meeting Minute Created Alert",
                Body = $"Meeting minute with ID {meetingMinuteDto.Id} has been created."
            };
            _emailService.SendEmail(emailDto);

            return meetingMinuteDto;
        }

        public override async Task<MeetingMinuteDto> UpdateAsync(Guid id, CreateUpdateMeetingMinuteDto input)
        {
            var meetingMinuteDto = await base.UpdateAsync(id, input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Meeting Minute Updated Alert",
                Body = $"Meeting minute with ID {meetingMinuteDto.Id} has been updated."
            };
            _emailService.SendEmail(emailDto);

            return meetingMinuteDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve meeting minute to get details before deletion
            var meetingMinute = await _meetingMinuteRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Meeting Minute Deleted Alert",
                Body = $"Meeting minute with ID {id} and Title  has been deleted."
            };
            _emailService.SendEmail(emailDto);
        }

        public async Task<List<MeetingMinute>> GetMeetingMinuteByProjectIdAsync(Guid projectId)
        {
            return await _meetingMinuteRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
