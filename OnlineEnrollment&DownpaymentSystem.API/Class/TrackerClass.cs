using Dapper;
using Newtonsoft.Json;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class TrackerClass : ITrackerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _conn;

        public TrackerClass(IConfiguration configuration)
        {
            _configuration = configuration;
            _conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<TrackResponse>> TrackApplication(string searchValue)
        {
            var response = new ServiceResponse<TrackResponse>();

            try
            {
              
                var jsonResult = await _conn.QueryFirstOrDefaultAsync<string>(
                    "SP_TrackApplicationStatus",
                    new { SearchValue = searchValue },
                    commandType: CommandType.StoredProcedure);

                if (string.IsNullOrEmpty(jsonResult))
                {
                    response.Status = 404;
                    response.Message = "No data found";
                    return response;
                }

              
                var result = JsonConvert.DeserializeObject<TrackResponse>(jsonResult);

                if (result == null)
                {
                    response.Status = 404;
                    response.Message = "Failed to parse data";
                    return response;
                }

                response.Status = 200;
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.Status = 500;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
