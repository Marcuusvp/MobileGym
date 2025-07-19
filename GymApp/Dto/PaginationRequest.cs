
namespace GymApp.Dto;

public class PaginationRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginationRequest() { }

    public PaginationRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 50 ? 50 : (pageSize < 1 ? 10 : pageSize); 
    }
}
