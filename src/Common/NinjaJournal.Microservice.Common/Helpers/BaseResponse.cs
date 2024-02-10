namespace NinjaJournal.Microservice.Common.Helpers;

public class BaseResponse
{
    public bool Succeeded { get; set; }
    public IEnumerable<string> Messages { get; set; } = new List<string>();

    public static BaseResponse Failure(string errorMessage) =>
        new BaseResponse { Succeeded = false, Messages = new[] { errorMessage } };

    public static BaseResponse Failure(IEnumerable<string> errorMessages) =>
        new BaseResponse { Succeeded = false, Messages = errorMessages };

    public static BaseResponse Success() =>
        new BaseResponse { Succeeded = true };
}