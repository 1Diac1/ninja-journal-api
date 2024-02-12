using NinjaJournal.Microservice.Core.Helpers;

namespace NinjaJournal.Microservice.Core.Extensions;

public static class BaseResponseExtensions
{
    public static BaseResponse ToFailure(this string errorMessage) => new BaseResponse
    {
        Succeeded = false,
        Messages = new[] { errorMessage }
    };

    public static BaseResponse ToFailure(this IEnumerable<string> errorMessages) => new BaseResponse
    {
        Succeeded = false,
        Messages = errorMessages
    };
}