using NinjaJournal.Microservice.Api.AuthService.Controllers.Consent;

namespace NinjaJournal.Microservice.Api.AuthService.Controllers.Device
{
    public class DeviceAuthorizationInputModel : ConsentInputModel
    {
        public string UserCode { get; set; }
    }
}