using Promact.CustomerSuccess.Platform.Entities;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Services.Dtos.EscalationMatrix;
using Promact.CustomerSuccess.Platform.Services.Emailing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.EscalationMatrices
{
    public class EscalationMatrixService : CrudAppService<EscalationMatrix,
                                EscalationMatrixDto,
                                Guid,
                                PagedAndSortedResultRequestDto,
                                CreateEscalationMatrix,
                                UpdateEscalationMatrix>,
                                IEscalationMatrixService
    {
        private readonly IEmailService _emailService;
        private readonly string Useremail ; 
        private readonly string Username ;
        IRepository<EscalationMatrix, Guid> _escalationMatrixRepository;
        public EscalationMatrixService(IRepository<EscalationMatrix, Guid> escalationMatrixRepository, IEmailService emailService)
            : base(escalationMatrixRepository)
        {
            _emailService = emailService;
            this.Useremail = Template.Useremail;
            this.Username = Template.Username;
            _escalationMatrixRepository = escalationMatrixRepository;


        }

        public override async Task<EscalationMatrixDto> CreateAsync(CreateEscalationMatrix input)
        {
            var escalationMatrixDto = await base.CreateAsync(input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Escalation Matrix Created alert",
                Body = Template.GetEmailTemplate(Username) 
            };
            _emailService.SendEmail(emailDto);

            return escalationMatrixDto;
        }

        public override async Task<EscalationMatrixDto> UpdateAsync(Guid id, UpdateEscalationMatrix input)
        {
            var escalationMatrixDto = await base.UpdateAsync(id, input);

            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Escalation Matrix Updated alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            return escalationMatrixDto;
        }

        public override async Task DeleteAsync(Guid id)
        {
            var emailDto = new EmailDto
            {
                To = Useremail,
                Subject = "Escalation Matrix Deleted alert",
                Body = Template.GetEmailTemplate(Username)
            };
            _emailService.SendEmail(emailDto);

            await base.DeleteAsync(id);
        }

        public async Task<List<EscalationMatrix>> GetEscalationmatricesByProjectIdAsync(Guid projectId)
        {
            return await _escalationMatrixRepository.GetListAsync(ah => ah.ProjectId == projectId);
        }

    }
}
