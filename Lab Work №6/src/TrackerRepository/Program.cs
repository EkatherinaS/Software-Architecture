using MongoDB.Driver;
using Constants = TrackerRepository.Constants;
using TrackerRepository;


class Program
{
    private static void createTestData()
    {
        DBCompanyService dBCompanyService = new DBCompanyService();
        DBGPSService dBGPSService = new DBGPSService();
        DBProjectService dProjectService = new DBProjectService();
        DBTaskService dTaskService = new DBTaskService();
        DBUserService dBUserService = new DBUserService();

        DBCompany company1 = new DBCompany();
        company1.Name = "CoffeeCat";
        dBCompanyService.AddEntity(company1);

        DBCompany company2 = new DBCompany();
        company2.Name = "CoolCompany";
        dBCompanyService.AddEntity(company2);

        DBGPS gps1 = new DBGPS();
        gps1.Latitude = 0;
        gps1.Longitude = 0;
        dBGPSService.AddEntity(gps1);

        DBGPS gps2 = new DBGPS();
        gps2.Latitude = 1;
        gps2.Longitude = 2;
        dBGPSService.AddEntity(gps2);

        DBProject project1 = new DBProject();
        project1.Name = "Project 1";
        project1.Description = "Test project 1";
        dProjectService.AddEntity(project1);

        DBProject project2 = new DBProject();
        project2.Name = "Project 2";
        project2.Description = "Test project 2";
        dProjectService.AddEntity(project2);

        DBTask task1 = new DBTask();
        task1.ChatId = 2;
        task1.TaskName = "Task 1";
        task1.Project = project1;
        task1.StartTime = DateTime.Parse("2024-02-03T16:30:03.351Z");
        task1.StartPosition = gps1;
        dTaskService.AddEntity(task1);

        DBTask task2 = new DBTask();
        task2.ChatId = 2;
        task2.TaskName = "Task 2";
        task2.Project = project2;
        task2.StartTime = DateTime.Now;
        task2.StartPosition = gps2;
        dTaskService.AddEntity(task2);

        DBUser user1 = new DBUser();
        user1.IdChat = 1;
        user1.IsAdmin = true;
        user1.Status = "test";
        user1.Company = company1;
        user1.FIO = "FIO 1";
        user1.Username = "username";
        dBUserService.AddEntity(user1);

        DBUser user2 = new DBUser();
        user2.IdChat = 2;
        user2.IsAdmin = false;
        user2.Status = "test";
        user2.Company = company2;
        user2.CurrentTask = task1;
        user2.FIO = "FIO 2";
        user2.Username = "username 2";
        dBUserService.AddEntity(user2);
    }

    static async Task Main(string[] args)
    {
        createTestData();
        MongoClient client = new MongoClient(Constants.ConnectionString);

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();
        app.UseMiddleware<OptionsMiddleware>();
        app.MapControllers();
        app.UseCors();

        app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");
        app.Run();
    }
}