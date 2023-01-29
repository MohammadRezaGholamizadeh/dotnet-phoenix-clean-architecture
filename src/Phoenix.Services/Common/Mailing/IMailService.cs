using Phoenix.SharedConfiguration.Common.Contracts.Services;

namespace Phoenix.Application.Common.Mailing;

public interface IMailService : TransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}
