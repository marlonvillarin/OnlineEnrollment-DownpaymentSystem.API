using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data.SqlClient;
using System.Data;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class StudentClass : IStudentRepository
    {
        private readonly SqlConnection conn;

        public StudentClass(IConfiguration config)
        {
            conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
        }

        public async Task<ServiceResponse<StudentModel>> CreateStudent(StudentModel student)
        {
            var service = new ServiceResponse<StudentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@FirstName", student.FirstName);
                param.Add("@LastName", student.LastName);
                param.Add("@MiddleName", student.MiddleName);
                param.Add("@Gender", student.Gender);
                param.Add("@BirthDate", student.BirthDate);
                param.Add("@ContactNumber", student.ContactNumber);
                param.Add("@Email", student.Email);
                param.Add("@Address", student.Address);
                param.Add("@StudentType", student.StudentType);
                param.Add("@StatementType", "INSERT");

                var studentID = await conn.QueryFirstOrDefaultAsync<int>("SP_STUDENT", param, commandType: CommandType.StoredProcedure);

                student.StudentID = studentID;
                service.Status = 200;
                service.Data = student;
                service.Message = "Student created successfully";
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }

            return service;
        }

        public async Task<ServiceResponse<StudentModel>> UpdateStudent(StudentModel student)
        {
            var service = new ServiceResponse<StudentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", student.StudentID);
                param.Add("@FirstName", student.FirstName);
                param.Add("@LastName", student.LastName);
                param.Add("@MiddleName", student.MiddleName);
                param.Add("@Gender", student.Gender);
                param.Add("@BirthDate", student.BirthDate);
                param.Add("@ContactNumber", student.ContactNumber);
                param.Add("@Email", student.Email);
                param.Add("@Address", student.Address);
                param.Add("@StatementType", "UPDATE");

                await conn.ExecuteAsync("SP_STUDENT", param, commandType: CommandType.StoredProcedure);

                service.Status = 200;
                service.Data = student;
                service.Message = "Student updated successfully";
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<StudentModel>> GetStudentByID(int studentID)
        {
            var service = new ServiceResponse<StudentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYID");

                var student = await conn.QueryFirstOrDefaultAsync<StudentModel>("SP_STUDENT", param, commandType: CommandType.StoredProcedure);

                service.Status = 200;
                service.Data = student;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<StudentDocumentModel>> UploadDocument(int studentID, string documentType, string filePath)
        {
            var service = new ServiceResponse<StudentDocumentModel>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@DocumentType", documentType);
                param.Add("@FilePath", filePath);
                param.Add("@StatementType", "INSERT");

                var documentID = await conn.QueryFirstOrDefaultAsync<int>("SP_STUDENTDOCUMENTS", param, commandType: CommandType.StoredProcedure);

                service.Status = 200;
                service.Message = "Document uploaded successfully";
                service.Data = new StudentDocumentModel
                {
                    DocumentID = documentID,
                    StudentID = studentID,
                    DocumentType = documentType,
                    FilePath = filePath,
                    IsApproved = false,
                    UploadedDate = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }

        public async Task<ServiceResponse<List<StudentDocumentModel>>> GetDocumentsByStudent(int studentID)
        {
            var service = new ServiceResponse<List<StudentDocumentModel>>();
            try
            {
                var param = new DynamicParameters();
                param.Add("@StudentID", studentID);
                param.Add("@StatementType", "GETBYSTUDENT");

                var documents = (await conn.QueryAsync<StudentDocumentModel>("SP_STUDENTDOCUMENTS", param, commandType: CommandType.StoredProcedure)).ToList();

                service.Status = 200;
                service.Data = documents;
            }
            catch (Exception ex)
            {
                service.Status = 500;
                service.Message = ex.Message;
            }
            return service;
        }
        public async Task<ServiceResponse<List<StudentModel>>> GetAllStudents()
        {
            var service = new ServiceResponse<List<StudentModel>>();

            try
            {
                var param = new DynamicParameters();
                param.Add("@StatementType", "GETALL");

                var result = (await conn.QueryAsync<StudentModel>(
                    "SP_STUDENT",
                    param,
                    commandType: CommandType.StoredProcedure
                )).ToList();

                service.Status = 200;
                service.Data = result;
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