using CarSpot.Domain.Entities;

public class UserUpdateBodyBuilder : IEmailBodyBuilder<WelcomeEmailDto>
{
    public string Build(WelcomeEmailDto user)
    {
        return $"<p>Hello! {user.FullName},</p><p>Your Data has been updated.</p>";
    }
   
}