using Dapper;
using OnlineEnrollment_DownpaymentSystem.API.IRepository;
using OnlineEnrollment_DownpaymentSystem.API.Model;
using OnlineEnrollment_DownpaymentSystem.API.Model.Response;
using System.Data;
using System.Data.SqlClient;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
   
        public class SubjectClass : ISubjectRepository
        {
            private readonly SqlConnection conn;

            public SubjectClass(IConfiguration config)
            {
                conn = new SqlConnection(config["ConnectionString:Enrollmentdb"]);
            }

            public async Task<ServiceResponse<SubjectModel>> AddSubject(SubjectModel subject)
            {
                var service = new ServiceResponse<SubjectModel>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@SubjectCode", subject.SubjectCode);
                    param.Add("@SubjectName", subject.SubjectName);
                    param.Add("@Course", subject.Course);
                    param.Add("@YearLevel", subject.YearLevel);
                    param.Add("@Semester", subject.Semester);
                    param.Add("@StatementType", "INSERT");

                    var result = await conn.QueryFirstOrDefaultAsync<SubjectModel>(
                        "SP_SUBJECTS",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    service.Status = 200;
                    service.Message = "Subject added successfully";
                    service.Data = result;
                }
                catch (Exception ex)
                {
                    service.Status = 500;
                    service.Message = ex.Message;
                }

                return service;
            }

            public async Task<ServiceResponse<List<SubjectModel>>> GetAllSubjects()
            {
                var service = new ServiceResponse<List<SubjectModel>>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@StatementType", "GETALL");

                    var result = await conn.QueryAsync<SubjectModel>(
                        "SP_SUBJECTS",
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

            public async Task<ServiceResponse<List<SubjectModel>>> GetSubjectsByFilter(string course, int yearLevel, string semester)
            {
                var service = new ServiceResponse<List<SubjectModel>>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@Course", course);
                    param.Add("@YearLevel", yearLevel);
                    param.Add("@Semester", semester);
                    param.Add("@StatementType", "GETBYFILTER");

                    var result = await conn.QueryAsync<SubjectModel>(
                        "SP_SUBJECTS",
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

            public async Task<ServiceResponse<SubjectModel>> UpdateSubject(SubjectModel subject)
            {
                var service = new ServiceResponse<SubjectModel>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@SubjectID", subject.SubjectID);
                    param.Add("@SubjectCode", subject.SubjectCode);
                    param.Add("@SubjectName", subject.SubjectName);
                    param.Add("@Course", subject.Course);
                    param.Add("@YearLevel", subject.YearLevel);
                    param.Add("@Semester", subject.Semester);
                    param.Add("@StatementType", "UPDATE");

                    var result = await conn.QueryFirstOrDefaultAsync<SubjectModel>(
                        "SP_SUBJECTS",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    service.Status = 200;
                    service.Message = "Subject updated successfully";
                    service.Data = result;
                }
                catch (Exception ex)
                {
                    service.Status = 500;
                    service.Message = ex.Message;
                }

                return service;
            }

            public async Task<ServiceResponse<string>> DeleteSubject(int subjectID)
            {
                var service = new ServiceResponse<string>();
                try
                {
                    var param = new DynamicParameters();
                    param.Add("@SubjectID", subjectID);
                    param.Add("@StatementType", "DELETE");

                    await conn.ExecuteAsync(
                        "SP_SUBJECTS",
                        param,
                        commandType: CommandType.StoredProcedure
                    );

                    service.Status = 200;
                    service.Message = "Subject deleted successfully";
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
