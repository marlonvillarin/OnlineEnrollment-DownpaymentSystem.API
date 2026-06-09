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
        private readonly EmailService _emailService;  

    
        public DocumentClass(IConfiguration config, EmailService emailService)
        {
            _configuration = config;
            conn = new SqlConnection(_configuration["ConnectionString:Enrollmentdb"]);
            _emailService = emailService;  
        }
        // Get all documents with student info (with search)
        public async Task<ServiceResponse<List<DocumentModel>>> GetAllDocumentsWithStudentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<DocumentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETALL");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
                
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
              
            }

            return service;
        }

        // Get pending documents with student info
        public async Task<ServiceResponse<List<DocumentModel>>> GetPendingDocumentsWithStudentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<DocumentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETPENDING");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
              
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
              
            }

            return service;
        }

        // Get approved documents with student info
        public async Task<ServiceResponse<List<DocumentModel>>> GetApprovedDocumentsWithStudentsAsync(string searchTerm = null)
        {
            var service = new ServiceResponse<List<DocumentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETAPPROVED");
                param.Add("@SearchTerm", searchTerm);

                var result = await conn.QueryAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
               
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
             
            }

            return service;
        }

        // Approve document
        public async Task<ServiceResponse<DocumentModel>> ApproveDocumentAsync(int documentId)
        {
            var service = new ServiceResponse<DocumentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "APPROVE");
                param.Add("@DocumentID", documentId);

                var result = await conn.QueryFirstOrDefaultAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );
                if (result != null)
                {
                    // ADD THIS - Send email notification
                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendDocumentApprovalEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.DocumentType
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                    service.Message = "Document approved successfully";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Document not found";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        // Reject document
        public async Task<ServiceResponse<DocumentModel>> RejectDocumentAsync(int documentId)
        {
            var service = new ServiceResponse<DocumentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "REJECT");
                param.Add("@DocumentID", documentId);

                var result = await conn.QueryFirstOrDefaultAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure);

                if (result != null)
                {
                    // ADD THIS - Send email notification
                    var studentInfo = await GetStudentInfo(result.StudentID);
                    if (!string.IsNullOrEmpty(studentInfo.Email))
                    {
                        await _emailService.SendDocumentRejectionEmail(
                            studentInfo.Email,
                            studentInfo.FullName,
                            result.DocumentType,
                            "Document does not meet requirements. Please upload a clear and valid copy."
                        );
                    }

                    service.Status = 200;
                    service.Data = result;
                    service.Message = "Document rejected";
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Document not found";
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        private async Task<(string Email, string FullName)> GetStudentInfo(int studentId)
        {
            var sql = "SELECT Email, FirstName + ' ' + LastName AS FullName FROM TBL_Student WHERE StudentID = @StudentID";
            var result = await conn.QueryFirstOrDefaultAsync<dynamic>(sql, new { StudentID = studentId });

            if (result != null)
            {
                return (result.Email, result.FullName);
            }
            return (string.Empty, string.Empty);
        }

        public async Task<ServiceResponse<DocumentModel>> GetDocumentById(int documentId)
        {
            var service = new ServiceResponse<DocumentModel>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETBYID");
                param.Add("@DocumentID", documentId);

                var result = await conn.QueryFirstOrDefaultAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null)
                {
                    service.Status = 200;
                    service.Data = result;
                   
                }
                else
                {
                    service.Status = 404;
                    service.Message = "Document not found";
                
                }
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
        
            }

            return service;
        }
        // Get documents by student ID
        public async Task<ServiceResponse<List<DocumentModel>>> GetDocumentsByStudentAsync(int studentId)
        {
            var service = new ServiceResponse<List<DocumentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@ActionType", "GETBYSTUDENT");
                param.Add("@StudentID", studentId);

                var result = await conn.QueryAsync<DocumentModel>(
                    "SP_DOCUMENTS_WITH_STUDENTS",
                    param,
                    commandType: CommandType.StoredProcedure
                );

                service.Status = 200;
                service.Data = result.ToList();
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

