namespace ChatAppServer.WepAPI.Dtos
{
    public sealed record RegisterDto(
        string Name,
        IFormFile File);
}
