using Phoenix.Application.Common.Interfaces;

namespace Phoenix.Application.Common.Mailing;

public interface IEmailTemplateService : ITransientService
{
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
}
