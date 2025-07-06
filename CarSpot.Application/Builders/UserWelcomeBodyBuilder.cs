using CarSpot.Domain.Entities;

public class UserWelcomeBodyBuilder : IEmailBodyBuilder<WelcomeEmailDto>
{
    public string Build(WelcomeEmailDto user)
    {
        return $"<p>Hello! {user.FullName},</p><p>Thank you for join to CarSpot.</p>";
    }
   
}