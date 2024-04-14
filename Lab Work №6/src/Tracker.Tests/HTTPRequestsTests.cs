using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;
using Tracker.TelegramBot.Controllers;
using Tracker.TelegramBot.Controllers.Entities;

namespace Tracker.Tests
{
    public class Tests
    {
        TrackerRepositoryApi trackerRepositoryApi;
        int chatId;

        [SetUp]
        public void Setup() 
        {
            trackerRepositoryApi = new TrackerRepositoryApi("http://ghcr.io/ekatherinas/timetrackerbot:main@sha256:59d6b77f7ef22b62cddfddc82d86441b5c5da712a0a6ab96fa3e534f098a9e90:8081/TimeTrackerBot/");
            chatId = 2;
        }

        [Test]
        public async Task GetInitData()
        {
            GetInfoEntity info = trackerRepositoryApi.getInitDataByChatId(chatId);

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
        }
    }
}