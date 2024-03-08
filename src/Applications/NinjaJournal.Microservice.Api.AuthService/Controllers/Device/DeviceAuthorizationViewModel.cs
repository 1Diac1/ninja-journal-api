using NinjaJournal.Microservice.Api.AuthService.Controllers.Consent;

namespace NinjaJournal.Microservice.Api.AuthService.Controllers.Device
{
    public class DeviceAuthorizationViewModel : ConsentViewModel
    {
        public string UserCode { get; set; }
        public bool ConfirmUserCode { get; set; }
    }
}