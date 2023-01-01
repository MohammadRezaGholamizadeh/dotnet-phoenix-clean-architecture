using Phoenix.Application.Common.Interfaces;
namespace Phoenix.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}
