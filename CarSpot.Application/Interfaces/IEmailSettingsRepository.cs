public interface IEmailSettingsRepository
{
    Task<EmailSettings?> GetSettingsAsync();
    Task<bool> UpdateAsync(EmailSettings settings);
    Task AddAsync(EmailSettings settings);
    Task<EmailSettings?> GetSettingsByNickNameAsync(string nickName);

}
