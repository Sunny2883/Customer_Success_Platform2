using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Promact.CustomerSuccess.Platform.Services.Dtos.RiskProfile;

namespace Promact.CustomerSuccess.Platform.Services.RiskProfiles
{
    public class RiskProfileService : CrudAppService<RiskProfile,
                                RiskProfileDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateRiskProfileDto,
                                UpdateRiskProfileDto>,
                                IRiskProfileService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail;
        private readonly string Username;
        private readonly IRepository<RiskProfile, Guid> _riskProfileRepository;

        public RiskProfileService(IRepository<RiskProfile, Guid> repository, IEmailService emailService) : base(repository)
        {
            _emailService = emailService;
            _riskProfileRepository = repository;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
        }

        public override async Task<RiskProfileDto> CreateAsync(CreateRiskProfileDto input)
        {
            var riskProfileDto = await base.CreateAsync(input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Risk Profile Created Alert",
                Body = $"Risk profile with ID {riskProfileDto.Id} has been created."
            };
            _emailService.SendEmail(emailDto);

            return riskProfileDto;
        }

        public override async Task<RiskProfileDto> UpdateAsync(Guid id, UpdateRiskProfileDto input)
        {
            var riskProfileDto = await base.UpdateAsync(id, input);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Risk Profile Updated Alert",
                Body = $"Risk profile with ID {riskProfileDto.Id} has been updated."
            };
            _emailService.SendEmail(emailDto);

            return riskProfileDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            // Retrieve risk profile to get details before deletion
            var riskProfile = await _riskProfileRepository.GetAsync(id);

            // Perform deletion
            await base.DeleteAsync(id);

            // Send email notification
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Risk Profile Deleted Alert",
                Body = $"Risk profile with ID {id} has been deleted."
            };
            _emailService.SendEmail(emailDto);
        }

        public async Task<List<RiskProfileDto>> GetRiskProfilesByProjectIdAsync(Guid projectId)
        {
            var riskProfiles = await _riskProfileRepository.GetListAsync(rp => rp.ProjectId == projectId);
            return ObjectMapper.Map<List<RiskProfile>, List<RiskProfileDto>>(riskProfiles);
        }
    }
}
