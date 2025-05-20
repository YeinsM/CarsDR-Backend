using CarSpot.Domain.Entities;

public class UserWelcomeBodyBuilder : IEmailBodyBuilder<User>
{
    public string Build(User user)
    {
        return $"<p>Hello! {user.FirstName} {user.LastName},</p><p>Thank you for join to CarSpot.</p>";
    }
}