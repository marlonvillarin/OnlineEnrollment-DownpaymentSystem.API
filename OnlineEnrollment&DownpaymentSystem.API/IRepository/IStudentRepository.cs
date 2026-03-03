using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;

namespace OnlineEnrollment_DownpaymentSystem.API.IRepository
{
    public interface IStudentRepository
    {
        Task<ServiceResponse<StudentModel>> CreateStudent(StudentModel student);
        Task<ServiceResponse<StudentModel>> UpdateStudent(StudentModel student);
        Task<ServiceResponse<StudentModel>> GetStudentByID(int studentID);
        Task<ServiceResponse<StudentDocumentModel>> UploadDocument(int studentID, string documentType, string filePath);
        Task<ServiceResponse<List<StudentDocumentModel>>> GetDocumentsByStudent(int studentID);
    }
}