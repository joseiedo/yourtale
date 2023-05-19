namespace YourTale.Application.Contracts.Documents.Responses.Core;

public class Pageable<T>
{
    public List<T> Content { get; set; } = new();
    public int Page { get; set; }
    public bool IsLastPage { get; set; }
}