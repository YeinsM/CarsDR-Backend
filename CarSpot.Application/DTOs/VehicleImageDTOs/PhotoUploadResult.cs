public record PhotoUploadResult
{
    public string Url { get; init; } = default!;
    public string PublicId { get; init; } = default!;
}
