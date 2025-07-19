namespace GymApp.Dto;

public class GetTreinosDoUsuarioRequest : PaginationRequest
{
    public GetTreinosDoUsuarioRequest(int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
    }
    public GetTreinosDoUsuarioRequest() : base()
    {
    }

    public string Usuario { get; set; } = string.Empty;
}