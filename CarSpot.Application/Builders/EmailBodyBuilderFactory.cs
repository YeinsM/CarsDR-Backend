public class EmailBodyBuilderFactory
{
    public static IEmailBodyBuilder<T> GetBuilder<T>(EmailTemplateType templateType)
    {
        return templateType switch
        {
            EmailTemplateType.Welcome => (IEmailBodyBuilder<T>)new UserWelcomeBodyBuilder(),
            EmailTemplateType.UpdateUser => (IEmailBodyBuilder<T>)new UserUpdateBodyBuilder(),
            _ => throw new ArgumentException("Tipo de plantilla no soportada")
        };
    }
}