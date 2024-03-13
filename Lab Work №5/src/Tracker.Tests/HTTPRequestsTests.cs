using Microsoft.AspNetCore.Mvc;
using TrackerRepository;

namespace Tracker.Tests
{
    public class Tests
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


        TrackerRepositoryController repositoryController;

        int chatId;

        [SetUp]
        public void Setup()
        {
            repositoryController = new TrackerRepositoryController();
            chatId = 2;
            createTestData();
        }

        [Test]
        public async Task GetUser()
        {
            ObjectResult result = (ObjectResult) repositoryController.getUserByChatId(2);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.Pass();
        }

        [Test]
        public async Task GetInitData()
        {
            ActionResult<GetInfoEntity> result = repositoryController.getInitDataByChatId(2);
            ObjectResult objectResult = (ObjectResult) result.Result;
            GetInfoEntity entity = (GetInfoEntity) objectResult.Value;

            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
            Assert.That(entity.FIO.ToString(), Is.EqualTo("FIO 2"));
            Assert.That(entity.TaskName.ToString(), Is.EqualTo("Task 1"));
            Assert.That(entity.ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(entity.StartTime.ToString(), Is.EqualTo("02/03/2024 16:30:03"));
            Assert.Pass();
        }

        [Test]
        public async Task PostGPSEntity()
        {
            GPSEntity dBGPS = new GPSEntity();
            dBGPS.Latitude = 1;
            dBGPS.Longitude = 2;
            bool result = repositoryController.postGPSEntity(dBGPS);

            Assert.That(result);
            Assert.Pass();
        }

        [Test]
        public async Task PostProjectEntity()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };
            repositoryController.postProjectEntity(project);

            ActionResult<bool> result = repositoryController.checkIfProjectExists(project);
            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.That((bool)objectResult.Value);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
            Assert.Pass();
        }

        [Test]
        public async Task PostTaskEntity()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            repositoryController.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = null,
                EndTime = null,
                StartLatitude = null,
                StartLongitude = null,
                EndLatitude = null,
                EndLongitude = null
            };

            repositoryController.postTaskEntity(task);

            ActionResult<bool> result = repositoryController.checkIfTaskExists(task);
            ObjectResult objectResult = (ObjectResult) result.Result;
            Assert.That((bool)objectResult.Value);
            Assert.That(objectResult.StatusCode, Is.EqualTo(200));
            Assert.Pass();
        }

        [Test]
        public async Task GetProjectList()
        {
            ActionResult<List<ProjectEntity>> result = repositoryController.getProjectsByChatId(chatId);
            ObjectResult objectResult = (ObjectResult) result.Result;
            List<ProjectEntity> projects = (List<ProjectEntity>)objectResult.Value;

            Assert.That(projects[0].ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(projects[0].ProjectDescription.ToString(), Is.EqualTo("Test project 1"));
            Assert.That(projects[1].ProjectName.ToString(), Is.EqualTo("Project 2"));
            Assert.That(projects[1].ProjectDescription.ToString(), Is.EqualTo("Test project 2"));
            Assert.Pass();
        }

        [Test]
        public async Task GetTaskList()
        {
            ActionResult<List<ProjectEntity>> result = repositoryController.getTasksByChatId(chatId);
            ObjectResult objectResult = (ObjectResult)result.Result;
            List<TaskEntity> tasks = (List<TaskEntity>)objectResult.Value;

            Assert.That(tasks[0].ChatId.ToString(), Is.EqualTo("2"));
            Assert.That(tasks[0].TaskName.ToString(), Is.EqualTo("Task 1"));
            Assert.That(tasks[0].ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(tasks[0].ProjectDescription.ToString(), Is.EqualTo("Test project 1"));
            Assert.Pass();
        }

        [Test]
        public async Task PutStartTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            repositoryController.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = null,
                EndTime = null,
                StartLatitude = null,
                StartLongitude = null,
                EndLatitude = null,
                EndLongitude = null
            };

            repositoryController.postTaskEntity(task);

            TaskEntity taskStarted = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = null,
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = null,
                EndLongitude = null
            };

            GPSEntity dBGPS = new GPSEntity();
            dBGPS.Latitude = taskStarted.StartLatitude.Value;
            dBGPS.Longitude = taskStarted.StartLongitude.Value;
            repositoryController.postGPSEntity(dBGPS);

            repositoryController.putStartTask(taskStarted);

            ActionResult<bool> result = repositoryController.checkIfTaskExists(taskStarted);
            ObjectResult objectResult = (ObjectResult)result.Result;

            Assert.That((bool)objectResult.Value);
            Assert.Pass();
        }

        [Test]
        public async Task PutEndTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            repositoryController.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = null,
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = null,
                EndLongitude = null
            };

            repositoryController.postTaskEntity(task);

            TaskEntity taskEnded = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = DateTime.Parse("2019-01-31T10:05:11.000Z"),
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = 10,
                EndLongitude = 21
            };

            GPSEntity dBGPSEnded = new GPSEntity();
            dBGPSEnded.Latitude = taskEnded.EndLatitude.Value;
            dBGPSEnded.Longitude = taskEnded.EndLongitude.Value;
            repositoryController.postGPSEntity(dBGPSEnded);

            repositoryController.putEndTask(taskEnded);

            ActionResult<bool> result = repositoryController.checkIfTaskExists(taskEnded);
            ObjectResult objectResult = (ObjectResult)result.Result;

            Assert.That((bool)objectResult.Value);
            Assert.Pass();
        }

        [Test]
        public async Task DeleteTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            repositoryController.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = DateTime.Parse("2019-01-31T10:05:11.000Z"),
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = 10,
                EndLongitude = 21
            };
            repositoryController.postTaskEntity(task);

            ActionResult<bool> result = repositoryController.deleteTask(task);
            ObjectResult objectResult = (ObjectResult)result.Result;

            Assert.That((bool)objectResult.Value);
            Assert.Pass();
        }

        [Test]
        public async Task DeleteProject()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            repositoryController.postProjectEntity(project);

            ActionResult<bool> result = repositoryController.deleteProject(project);
            ObjectResult objectResult = (ObjectResult)result.Result;

            Assert.That((bool)objectResult.Value);
            Assert.Pass();
        }
        /*
        TrackerRepositoryApi trackerRepositoryApi;
        int chatId;

        [SetUp]
        public void Setup() 
        {
            trackerRepositoryApi = new TrackerRepositoryApi("127.0.0.1:7270/TrackerRepository/");
            chatId = 2;
        }

        [Test]
        public async Task GetInitData()
        {
            GetInfoEntity info = trackerRepositoryApi.getInitDataByChatId(chatId);я

            Assert.That(info.FIO.ToString(), Is.EqualTo("FIO 2"));
            Assert.That(info.TaskName.ToString(), Is.EqualTo("Task 1"));
            Assert.That(info.ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(info.StartTime.ToString(), Is.EqualTo("03.02.2024 16:30:03"));
            Assert.Pass();
        }

        [Test]
        public async Task GetProjectList()
        {
            List<ProjectEntity> projects = trackerRepositoryApi.getProjectsByChatId(chatId);

            Assert.That(projects[0].ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(projects[0].ProjectDescription.ToString(), Is.EqualTo("Test project 1"));
            Assert.That(projects[1].ProjectName.ToString(), Is.EqualTo("Project 2"));
            Assert.That(projects[1].ProjectDescription.ToString(), Is.EqualTo("Test project 2"));
            Assert.Pass();
        }

        [Test]
        public async Task GetTaskList()
        {
            List<TaskEntity> tasks = trackerRepositoryApi.getTasksByChatId(chatId);

            Assert.That(tasks[0].ChatId.ToString(), Is.EqualTo("2"));
            Assert.That(tasks[0].TaskName.ToString(), Is.EqualTo("Task 1"));
            Assert.That(tasks[0].ProjectName.ToString(), Is.EqualTo("Project 1"));
            Assert.That(tasks[0].ProjectDescription.ToString(), Is.EqualTo("Test project 1"));
            Assert.Pass();
        }
        
        [Test]
        public async Task PostNewProject()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            trackerRepositoryApi.postProjectEntity(project);

            Assert.That(trackerRepositoryApi.checkIfProjectExists(project));
            Assert.Pass();
        }

        
        [Test]
        public async Task PostNewTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            trackerRepositoryApi.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = null,
                EndTime = null,
                StartLatitude = null,
                StartLongitude = null,
                EndLatitude = null,
                EndLongitude = null
            };

            trackerRepositoryApi.postTaskEntity(task);
            Assert.That(trackerRepositoryApi.checkIfTaskExists(task));
            Assert.Pass();
        }

        
        [Test]
        public async Task PutStartTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            trackerRepositoryApi.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = null,
                EndTime = null,
                StartLatitude = null,
                StartLongitude = null,
                EndLatitude = null,
                EndLongitude = null
            };

            trackerRepositoryApi.postTaskEntity(task);

            TaskEntity taskStarted = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = null,
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = null,
                EndLongitude = null
            };

            GPSEntity dBGPS = new GPSEntity();
            dBGPS.Latitude = taskStarted.StartLatitude.Value;
            dBGPS.Longitude = taskStarted.StartLongitude.Value;
            trackerRepositoryApi.postGPSEntity(dBGPS);

            trackerRepositoryApi.putStartTask(taskStarted);

            Assert.That(trackerRepositoryApi.checkIfTaskExists(taskStarted));
            Assert.Pass();
        }

        [Test]
        public async Task PutEndTask()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            trackerRepositoryApi.postProjectEntity(project);

            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = null,
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = null,
                EndLongitude = null
            };

            trackerRepositoryApi.postTaskEntity(task);

            TaskEntity taskEnded = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = DateTime.Parse("2019-01-31T10:05:11.000Z"),
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = 10,
                EndLongitude = 21
            };

            GPSEntity dBGPSEnded = new GPSEntity();
            dBGPSEnded.Latitude = taskEnded.EndLatitude.Value;
            dBGPSEnded.Longitude = taskEnded.EndLongitude.Value;
            trackerRepositoryApi.postGPSEntity(dBGPSEnded);

            trackerRepositoryApi.putEndTask(taskEnded);
            Assert.That(trackerRepositoryApi.checkIfTaskExists(taskEnded));
            Assert.Pass();
        }

        [Test]
        public async Task DeleteTask()
        {
            TaskEntity task = new TaskEntity
            {
                ChatId = 1,
                TaskName = "Task 11",
                TaskDescription = "New task",
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription",
                StartTime = DateTime.Parse("2019-01-31T10:02:00.000Z"),
                EndTime = DateTime.Parse("2019-01-31T10:05:11.000Z"),
                StartLatitude = 1,
                StartLongitude = 2,
                EndLatitude = 10,
                EndLongitude = 21
            };
            trackerRepositoryApi.postTaskEntity(task);
            Assert.That(trackerRepositoryApi.deleteTask(task));
            Assert.Pass();
        }

        [Test]
        public async Task DeleteProject()
        {
            ProjectEntity project = new ProjectEntity
            {
                ProjectName = "ProjectName",
                ProjectDescription = "ProjectDescription"
            };

            trackerRepositoryApi.postProjectEntity(project);
            
            Assert.That(trackerRepositoryApi.deleteProject(project));
            Assert.Pass();
        }*/
    }
}
