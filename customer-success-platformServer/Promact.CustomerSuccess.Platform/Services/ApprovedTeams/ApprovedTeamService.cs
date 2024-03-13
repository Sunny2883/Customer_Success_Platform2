using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Microsoft.AspNetCore.Authorization;
using static Volo.Abp.Identity.IdentityPermissions;

namespace Promact.CustomerSuccess.Platform.Services.ApprovedTeams
{
    public class ApprovedTeamService : CrudAppService<ApprovedTeam, ApprovedTeamDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateApprovedTeamDto, CreateUpdateApprovedTeamDto>
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;
        private readonly IRepository<ApprovedTeam, Guid> _approvedTeamRepository;

        public ApprovedTeamService(IRepository<ApprovedTeam, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _approvedTeamRepository = repository;
            Useremail = Template.Useremail;
            Username  = Template .Username;
        }

        public override async Task<ApprovedTeamDto> CreateAsync(CreateUpdateApprovedTeamDto input)
        {
            var approvedTeamDto = await base.CreateAsync(input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Approved Team Created Alert",
                Body = $"Approved Team with ID {approvedTeamDto.Id} has been created."
            };
            _emailService.SendEmail(emailDto);

            return approvedTeamDto;
        }
        public override async Task<ApprovedTeamDto> UpdateAsync(Guid id, CreateUpdateApprovedTeamDto input)
        {
            var approvedTeamDto = await base.UpdateAsync(id, input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Approved Team Updated Alert",
                Body = $"Approved Team with ID {approvedTeamDto.Id} has been updated."
            };
            _emailService.SendEmail(emailDto);

            return approvedTeamDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve approved team to get details before deletion
            var approvedTeam = await _approvedTeamRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Approved Team Deleted Alert",
                Body = $"Approved Team with ID {id} has been deleted."
            };
            _emailService.SendEmail(emailDto);
        }

        public async Task<List<ApprovedTeamDto>> GetApprovedTeamsByProjectIdAsync(Guid projectId)
        {
            var approvedTeams = await _approvedTeamRepository.GetListAsync(t => t.ProjectId == projectId);
            return ObjectMapper.Map<List<ApprovedTeam>, List<ApprovedTeamDto>>(approvedTeams);
        }
    }
}
