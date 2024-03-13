
using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using System.Threading.Tasks;

namespace TrackerRepository
{

    public class TrackerRepositoryController : ControllerBase
    {

        private static DBTaskService taskService = new DBTaskService();
        private static DBProjectService projectService = new DBProjectService();
        private static DBUserService userService = new DBUserService();
        private static DBGPSService gpsService = new DBGPSService();


        [HttpGet]
        public ActionResult getUserByChatId(long chatId)
        //получить имя текущей задачи, проекта и время начала выполнения
        {
            Console.WriteLine("getUserByChatId called");
            UserEntity user = userService.getUserByChatIdAsync(chatId).Result.toUserEntity();

            if (user == null)
            {
                return NotFound(chatId);
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpGet]
        public ActionResult<GetInfoEntity> getInitDataByChatId(long chatId)
        //получить имя текущей задачи, проекта и время начала выполнения
        {
            Console.WriteLine("getInitDataByChatId called");
            DBUser user = userService.getUserByChatIdAsync(chatId).Result;

            if (user == null)
            {
                return NotFound(chatId);
            }
            else
            {
                GetInfoEntity getInfoEntity = new GetInfoEntity();
                getInfoEntity.FIO = user.FIO;
                getInfoEntity.ProjectName = user.CurrentTask.Project.Name;
                getInfoEntity.TaskName = user.CurrentTask.TaskName;
                getInfoEntity.StartTime = user.CurrentTask.StartTime;

                return Ok(getInfoEntity);
            }
        }

        [HttpPost]
        public bool postGPSEntity(GPSEntity dBGPS)
        {
            Console.WriteLine("postGPSEntity called");
            DBGPS newGPS = new DBGPS();
            newGPS.Latitude = dBGPS.Latitude;
            newGPS.Longitude = dBGPS.Longitude;
            gpsService.AddEntity(newGPS);

            newGPS = gpsService.GetAll().Where(x => newGPS.Equals(x)).ToList().First();
            if (newGPS != null)
            {
                Console.WriteLine("postGPSEntity - gps added");
                return true;
            }
            else
            {
                Console.WriteLine("postGPSEntity - error adding the gps");
                return false;
            }
        }

        [HttpPost]
        public void postProjectEntity(ProjectEntity project)
        {
            Console.WriteLine("postProjectEntity called");
            DBProject dBProject = new DBProject();
            dBProject.Name = project.ProjectName;
            dBProject.Description = project.ProjectDescription;
            projectService.AddEntity(dBProject);

            dBProject = projectService.GetAll().Where(x => dBProject.Equals(x)).ToList().FirstOrDefault();
            if (dBProject != null)
            {
                Console.WriteLine("postProjectEntity - project added");
            }
            else
            {
                Console.WriteLine("postProjectEntity - error adding the project");
            }
        }

        [HttpPost]
        public void postTaskEntity(TaskEntity task)
        {
            Console.WriteLine("postTaskEntity called");

            DBTask dBTask = new DBTask();
            dBTask.TaskName = task.TaskName;
            dBTask.TaskDescription = task.TaskDescription;
            dBTask.ChatId = task.ChatId;
            dBTask.StartTime = task.StartTime;
            dBTask.EndTime = task.EndTime;

            DBProject dBProject = projectService.GetAll().Where(x => x.Name == task.ProjectName && x.Description == task.ProjectDescription).ToList().FirstOrDefault();
            dBTask.Project = dBProject;

            if (task.StartLatitude != null && task.StartLongitude != null)
            {
                DBGPS? startGPS = new DBGPS();
                startGPS.Latitude = task.StartLatitude.Value;
                startGPS.Longitude = task.StartLongitude.Value;
                gpsService.AddEntity(startGPS);

                startGPS = gpsService.GetAll().Where(x => startGPS.Equals(x)).ToList().FirstOrDefault();
                if (startGPS != null)
                {
                    dBTask.StartPosition = startGPS;
                }

                if (task.EndLatitude != null && task.EndLongitude != null)
                {
                    DBGPS? endGPS = new DBGPS();
                    endGPS.Latitude = task.EndLatitude.Value;
                    endGPS.Longitude = task.EndLongitude.Value;
                    gpsService.AddEntity(endGPS);

                    endGPS = gpsService.GetAll().Where(x => endGPS.Equals(x)).ToList().FirstOrDefault();
                    if (endGPS != null)
                    {
                        dBTask.EndPosition = endGPS;
                    }
                }
            }

            taskService.AddEntity(dBTask);
        }

        [HttpPost]
        public ActionResult checkIfProjectExists(ProjectEntity projectEntity)
        {
            Console.WriteLine("checkIfProjectExists called");
            var projectsFound = projectService.GetAll().Where(x => projectEntity.EqualsDBProject(x));
            if (projectsFound == null || projectsFound.Count() == 0)
            {
                return NotFound(false);
            }
            return Ok(true);
        }

        [HttpPost]
        public ActionResult checkIfTaskExists(TaskEntity taskEntity)
        {
            Console.WriteLine("checkIfTaskExists called");
            var tasksFound = taskService.GetAll().Where(x => taskEntity.EqualsDBTask(x));
            if (tasksFound == null || tasksFound.Count() == 0)
            {
                return NotFound(false);
            }
            return Ok(true);
        }


        [HttpGet]
        public ActionResult getProjectsByChatId(long chatId)
        {
            Console.WriteLine("getProjectsByChatId called");
            if (userService.getUserByChatIdAsync(chatId).Result == null)
            {
                return NotFound(chatId);
            }

            List<ProjectEntity> projects = taskService.GetAll().FindAll(x => x.ChatId == chatId).Select(x => x.Project.toProjectEntity()).ToList();
            return Ok(projects);
        }

        [HttpGet]
        public ActionResult getTasksByChatId(long chatId)
        {
            Console.WriteLine("getTasksByChatId called");
            if (userService.getUserByChatIdAsync(chatId).Result == null)
            {
                return NotFound(chatId);
            }
            List<TaskEntity> tasks = taskService.GetAll().FindAll(x => x.ChatId == chatId).Select(x => x.toTaskEntity()).ToList();
            return Ok(tasks);
        }

        [HttpPut]
        public ActionResult putStartTask(TaskEntity task)
        {
            Console.WriteLine("putStartTask called");
            TaskEntity taskExisting = task.getDeepCopy();
            taskExisting.StartTime = null;
            taskExisting.StartLongitude = null;
            taskExisting.StartLatitude = null;

            DBTask dBTaskExisting = taskService.GetAll().Where(x => taskExisting.EqualsDBTask(x)).FirstOrDefault();

            if (dBTaskExisting != null)
            {
                Console.WriteLine("putStartTask - task found");
                DBTask dBTaskUpdated = dBTaskFromTaskEntity(task);
                dBTaskUpdated.Id = dBTaskExisting.Id;
                taskService.UpdateEntityAsync(dBTaskUpdated).Wait();
                return Ok(task);
            }
            Console.WriteLine("putStartTask - task not found");
            return NotFound(task);
        }

        [HttpPut]
        public ActionResult putEndTask(TaskEntity task)
        {
            Console.WriteLine("putEndTask called");
            TaskEntity taskExisting = task.getDeepCopy();
            taskExisting.EndTime = null;
            taskExisting.EndLatitude = null;
            taskExisting.EndLongitude = null;

            DBTask dBTaskExisting = taskService.GetAll().Where(x => taskExisting.EqualsDBTask(x)).FirstOrDefault();

            if (dBTaskExisting != null)
            {
                Console.WriteLine("putEndTask - task found");
                DBTask dBTaskUpdated = dBTaskFromTaskEntity(task);
                dBTaskUpdated.Id = dBTaskExisting.Id;
                taskService.UpdateEntityAsync(dBTaskUpdated).Wait();
                return Ok(task);
            }
            Console.WriteLine("putEndTask - task not found");
            return NotFound(task);
        }



        [HttpDelete]
        public ActionResult deleteProject(ProjectEntity project)
        {
            Console.WriteLine("deleteProject called");

            DeleteResult result;
            List<DBTask> projectTasks = taskService.GetAll().Where(x => project.EqualsDBProject(x.Project)).ToList();
            foreach (DBTask task in projectTasks)
            {
                result = taskService.DeleteEntity(task.Id).Result;
                if (!result.IsAcknowledged)
                {
                    return NotFound(project);
                }
            }

            DBProject dBProject = projectService.GetAll().Where(x => project.EqualsDBProject(x)).ToList().FirstOrDefault();
            result = projectService.DeleteEntity(dBProject.Id).Result;
            if (result.DeletedCount > 0 && result.IsAcknowledged)
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        [HttpDelete]
        public ActionResult deleteTask(TaskEntity task)
        {
            Console.WriteLine("deleteTask called");
            DBTask dBTask = taskService.GetAll().Where(x => task.EqualsDBTask(x)).ToList().FirstOrDefault();
            DeleteResult result = taskService.DeleteEntity(dBTask.Id).Result;
            if (result.DeletedCount > 0 && result.IsAcknowledged)
            {
                return Ok(true);
            }
            else
            {
                return NotFound(false);
            }
        }

        private DBTask dBTaskFromTaskEntity(TaskEntity taskEntity)
        {
            DBTask dBTask = new DBTask();
            dBTask.TaskName = taskEntity.TaskName;
            dBTask.TaskDescription = taskEntity.TaskDescription;
            dBTask.ChatId = taskEntity.ChatId;
            dBTask.StartTime = taskEntity.StartTime;
            dBTask.EndTime = taskEntity.EndTime;
            dBTask.Project = projectService.GetAll().Where(x => x.Name == taskEntity.ProjectName && x.Description == taskEntity.ProjectDescription).ToList().FirstOrDefault();
            dBTask.StartPosition = gpsService.GetAll().Where(x => x.Latitude.Equals(taskEntity.StartLatitude) && x.Longitude.Equals(taskEntity.StartLongitude)).FirstOrDefault();
            dBTask.EndPosition = gpsService.GetAll().Where(x => x.Latitude.Equals(taskEntity.EndLatitude) && x.Longitude.Equals(taskEntity.EndLongitude)).FirstOrDefault();

            return dBTask;
        }

    }
}
