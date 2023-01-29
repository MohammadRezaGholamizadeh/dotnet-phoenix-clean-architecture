using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Mailing;

public interface IEmailTemplateService : TransientService
{
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
}
