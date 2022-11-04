namespace SampleApp.Client.Application.Managers;
public interface IManagerBase<T>
{
    public List<T> Data { get; }
    public int CurrentPage { get; }
    public int PageSize { get; }
    public int TotalAvailableRecords { get; }
    public bool HasMoreRecords { get; }
}
