namespace GestaoFrota.Services;
public interface IEmailService
{
    // Contrato para enviar email com PDF em anexo
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName);
}
