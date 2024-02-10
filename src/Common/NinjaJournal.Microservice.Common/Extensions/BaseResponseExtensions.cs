using NinjaJournal.Microservice.Common.Helpers;

namespace NinjaJournal.Microservice.Common.Extensions;

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