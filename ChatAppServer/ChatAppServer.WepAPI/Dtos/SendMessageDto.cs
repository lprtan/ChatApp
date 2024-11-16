namespace ChatAppServer.WepAPI.Dtos
{
    public sealed record SendMessageDto(
        Guid UserId,
        Guid ToUserId,
        string Message);

}
