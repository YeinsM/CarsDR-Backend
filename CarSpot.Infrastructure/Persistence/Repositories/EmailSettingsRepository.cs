using CarSpot.Domain.Entities;
using CarSpot.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;


public class EmailSettingsRepository : IEmailSettingsRepository
{
    private readonly ApplicationDbContext _context;

    public EmailSettingsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmailSettings?> GetSettingsAsync()
    {
        return await _context.EmailSettings.FirstOrDefaultAsync();
    }

    public async Task AddAsync(EmailSettings settings)
    {
        _context.EmailSettings.Add(settings);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(EmailSettings updatedSettings)
    {
        var existing = await _context.EmailSettings.FirstOrDefaultAsync();
        if (existing == null)
            return false;

        existing.SmtpServer = updatedSettings.SmtpServer;
        existing.SmtpPort = updatedSettings.SmtpPort;
        existing.FromEmail = updatedSettings.FromEmail;
        existing.FromPassword = updatedSettings.FromPassword;
        existing.NickName = updatedSettings.NickName;

        _context.EmailSettings.Update(existing);
        await _context.SaveChangesAsync();

        return true;
    }


    public async Task<EmailSettings?> GetSettingsByNickNameAsync(string? nickName)
    {
        return await _context.EmailSettings.FirstOrDefaultAsync(e => e.NickName == nickName);
    }

}
