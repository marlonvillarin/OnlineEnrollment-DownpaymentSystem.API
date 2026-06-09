using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentDocumentRepository
    {
        Task<ServiceResponse<List<DocumentModel>>> GetAllDocumentsWithStudentsAsync(string searchTerm = null);
        
        Task<ServiceResponse<List<DocumentModel>>> GetPendingDocumentsWithStudentsAsync(string searchTerm = null);
        Task<ServiceResponse<List<DocumentModel>>> GetApprovedDocumentsWithStudentsAsync(string searchTerm = null);
        Task<ServiceResponse<DocumentModel>> GetDocumentById(int documentId);
        Task<ServiceResponse<DocumentModel>> ApproveDocumentAsync(int documentId);
        Task<ServiceResponse<DocumentModel>> RejectDocumentAsync(int documentId);
        Task<ServiceResponse<List<DocumentModel>>> GetDocumentsByStudentAsync(int studentId);
    }
}
