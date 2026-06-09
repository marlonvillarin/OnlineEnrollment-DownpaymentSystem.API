using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface ITrackerRepository
    {
        Task<ServiceResponse<TrackResponse>> TrackApplication(string searchValue);
    }
}
