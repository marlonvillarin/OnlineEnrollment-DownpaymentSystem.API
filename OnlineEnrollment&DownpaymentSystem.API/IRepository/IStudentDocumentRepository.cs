using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentDocumentRepository
    {
        Task<ServiceResponse<List<DocumentModel>>> GetAllDocuments();
        Task<ServiceResponse<object>> InsertStudentDocument(DocumentModel studentDocument);
    }
}
