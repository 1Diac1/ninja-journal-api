namespace NinjaJournal.Microservice.Core.Helpers;

public class DataResponse<T> : BaseResponse
{
    public T Data { get; set; }

    public static DataResponse<T> Success(T data) =>
        new DataResponse<T> { Succeeded = true, Data = data };
}