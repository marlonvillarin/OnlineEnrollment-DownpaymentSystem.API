using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class DocumentClass :IStudentDocumentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection conn;

        public DocumentClass(IConfiguration config) { 
            _configuration = config;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<object>> InsertStudentDocument(DocumentModel studentDocument)
        {
            var service = new ServiceResponse<object>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentDocument.StudentID);
                param.Add("@DocumentType", studentDocument.DocumentType);
                param.Add("@FilePath", studentDocument.FilePath);

                conn.Open();

                var result = conn.Query<int>(
                    "SP_INSERTSTUDENT_DOCUMENT",
                    param,
                    commandType: CommandType.StoredProcedure
                ).FirstOrDefault();

                conn.Close();

                if (result > 0)
                {
                    service.Status = 200;
                    service.Message = "Student document inserted successfully.";
                    service.Data = new
                    {
                        DocumentID = result,
                        studentDocument.StudentID,
                        studentDocument.DocumentType,
                        studentDocument.FilePath
                    };
                }
                else
                {
                    service.Status = 400;
                    service.Message = "Failed to insert student document.";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

    }
}
