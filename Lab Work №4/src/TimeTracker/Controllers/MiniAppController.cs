
using MongoDB.Driver;
using Tracker.MongoDB.DBEntities;
using Tracker.MongoDB.DBServices;
using Tracker.TelegramBot.Controllers.Entities;
using Microsoft.AspNetCore.Mvc;
using Tracker.MongoDB.Models;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace Tracker.TelegramBot.Controllers
{

    public class MiniAppController : ControllerBase
    {
        
        private static DBTaskService taskService = new DBTaskService();
        private static DBProjectService projectService = new DBProjectService();
        private static DBUserService userService = new DBUserService();
        private static DBGPSService gpsService = new DBGPSService();


        

        [HttpGet]
        public ActionResult GetInitData(long chatId)
        //получить имя текущей задачи, проекта и время начала выполнения
        {
            DBUser user = userService.getUserByChatIdAsync(chatId).Result;

            if (user == null)
            {
                return NotFound(chatId);
            }
            else
            {
                GetInfoEntity postTaskEntity = new GetInfoEntity();

                //Если текущей задачи нет или она завершена
                if (user.CurrentTask == null || user.CurrentTask.EndTime != null)
                {
                    //Передаем пустую задачу
                    postTaskEntity = new GetInfoEntity { FIO = user.FIO };
                }
                //Если задача есть и она не завершена
                else
                {
                    //Передаем текущую задачу
                    postTaskEntity = new GetInfoEntity
                    {
                        FIO = user.FIO,
                        TaskName = user.CurrentTask.TaskName,
                        ProjectName = user.CurrentTask.Project.Name,
                        StartTime = user.CurrentTask.StartTime
                    };
                }
                return  Ok(postTaskEntity);
            }
        }


        [HttpGet]
        public ActionResult GetProjectList(long chatId)
        //получить список проектов для текущего пользователя
        {
            if (userService.getUserByChatIdAsync(chatId).Result == null)
            {
                return NotFound(chatId);
            }

            List<DBProject> projects = taskService.GetAll().FindAll(x => x.ChatId == chatId).Select(x => x.Project).ToList();
            return Ok(projects);
        }


        [HttpGet]
        public ActionResult GetTaskList(long chatId)
        //получить список задач для текущего пользователя
        {
            if (userService.getUserByChatIdAsync(chatId).Result == null)
            {
                return NotFound(chatId);
            }
            List<DBTask> tasks = taskService.GetAll().FindAll(x => x.ChatId == chatId);
            return Ok(tasks);
        }


        [HttpPost]
        public ActionResult<DBProject> PostNewTask([FromBody] TaskEntity newTask)
        //создать новую задачу для текущего пользователя
        {
            DBTask task = new DBTask();
            task.ChatId = newTask.ChatId;
            task.TaskName = newTask.TaskName;
            task.TaskDescription = newTask.TaskDescription;
            task.StartTime = newTask.StartTime;
            task.EndTime = newTask.EndTime;

            ProjectEntity projectEntity = new ProjectEntity();
            projectEntity.ProjectName = newTask.ProjectName;
            projectEntity.ProjectDescription = newTask.ProjectDescription;

            var projectsFound = projectService.GetAll().Where(x => projectEntity.EqualsDBProject(x));
            if (projectsFound == null || projectsFound.Count() == 0)
            {
                return NotFound(task);
            }

            task.Project = projectsFound.First();

            if (newTask.StartLatitude != null && newTask.StartLongitude != null)
            {
                DBGPS startGps = new DBGPS();
                startGps.Latitude = newTask.StartLatitude.Value;
                startGps.Longitude = newTask.StartLongitude.Value;
                gpsService.AddEntity(startGps);
                task.StartPosition = startGps;


                if (newTask.EndLatitude != null && newTask.EndLongitude != null)
                {
                    DBGPS endGps = new DBGPS();
                    endGps.Latitude = newTask.EndLatitude.Value;
                    endGps.Longitude = newTask.EndLongitude.Value;
                    gpsService.AddEntity(endGps);
                    task.EndPosition = endGps;
                }
            }
            taskService.AddEntity(task);

            return CreatedAtAction(nameof(taskService.AddEntity), new { task }, task);
        }
        

        [HttpPost]
        public ActionResult<DBProject> PostNewProject(ProjectEntity newProject)
        //создать новый проект для текущего пользователя
        {
            DBProject project = new DBProject();
            project.Name = newProject.ProjectName;
            project.Description = newProject.ProjectDescription;

            projectService.AddEntity(project);

            return CreatedAtAction(nameof(projectService.AddEntity), new { project }, project);
        }

        
        [HttpDelete]
        public ActionResult<DBTask> DeleteTask([FromBody] TaskEntity task)
        //удалить задачу
        {
            var tasksFound = taskService.GetAll().Where(x => task.EqualsDBTask(x));
            if (tasksFound == null || tasksFound.Count() == 0)
            {
                return NotFound();
            }
            DBTask dBTask = tasksFound.First();
            DeleteResult result = taskService.DeleteEntity(dBTask).Result;

            if (result.DeletedCount > 0 && result.IsAcknowledged)
            {
                return Ok(dBTask);
            }
            else
            {
                return NotFound(task);
            }
        }


        [HttpDelete]
        public ActionResult<DBProject> DeleteProject([FromBody] ProjectEntity project)
        //удалить проект
        {
            DeleteResult result;
            List<DBTask> projectTasks = taskService.GetAll().Where(x => project.EqualsDBProject(x.Project)).ToList();
            foreach (DBTask task in projectTasks)
            {
                result = taskService.DeleteEntity(task).Result;
                if (!result.IsAcknowledged)
                {
                    return NotFound(project);
                }
            }

            var projectsFound = projectService.GetAll().Where(x => project.EqualsDBProject(x));
            if (projectsFound == null || projectsFound.Count() == 0)
            {
                return NotFound(project);
            }
            DBProject dBProject = projectsFound.First();

            result = projectService.DeleteEntity(dBProject).Result;
            if (result.DeletedCount > 0 && result.IsAcknowledged)
            {
                return Ok(dBProject);
            }
            else
            {
                return NotFound(project);
            }
        }

        
        [HttpPut]
        public ActionResult<DBTask> PutStartTask([FromBody] TaskEntity startedTask)
        //запустить задачу
        {
            var tasksFound = taskService.GetAll().Where(x => startedTask.EqualsNotStartedDBTask(x));
            if (tasksFound == null || tasksFound.Count() == 0)
            {
                return NotFound();
            }
            DBTask dBTask = tasksFound.First();

            if (startedTask.StartTime == null)
            {
                return BadRequest("StartTime is null");
            }
            dBTask.StartTime = startedTask.StartTime;


            if (startedTask.StartLatitude == null || startedTask.StartLongitude == null)
            {
                return BadRequest("StartPosition is null");
            }
            else
            {
                DBGPS dBGPS = new DBGPS();
                dBGPS.Latitude = startedTask.StartLatitude.Value;
                dBGPS.Longitude = startedTask.StartLongitude.Value;
                gpsService.AddEntity(dBGPS);

                dBTask.StartPosition = dBGPS;
            }

            taskService.UpdateEntityAsync(dBTask).Wait();

            DBTask dBTaskUpdated = taskService.GetByEntity(dBTask).Result;
            if (dBTaskUpdated == null)
            {
                return BadRequest(startedTask);
            }
            return Ok(dBTaskUpdated);
        }


        [HttpPut]
        public ActionResult<DBTask> PutEndTask([FromBody] TaskEntity endedTask)
        //остановить задачу
        {
            var tasksFound = taskService.GetAll().Where(x => endedTask.EqualsNotEndedDBTask(x));
            if (tasksFound == null || tasksFound.Count() == 0)
            {
                return NotFound();
            }
            DBTask dBTask = tasksFound.First();

            if (endedTask.EndTime == null)
            {
                return BadRequest("EndTime is null");
            }
            dBTask.EndTime = endedTask.EndTime;


            if (endedTask.EndLatitude == null || endedTask.EndLongitude == null)
            {
                return BadRequest("EndPosition is null");
            }
            else
            {
                DBGPS dBGPS = new DBGPS();
                dBGPS.Latitude = endedTask.EndLatitude.Value;
                dBGPS.Longitude = endedTask.EndLongitude.Value;
                gpsService.AddEntity(dBGPS);

                dBTask.EndPosition = dBGPS;
            }

            taskService.UpdateEntityAsync(dBTask).Wait();

            DBTask dBTaskUpdated = taskService.GetByEntity(dBTask).Result;
            if (dBTaskUpdated == null)
            {
                return BadRequest(endedTask);
            }
            return Ok(dBTaskUpdated);
        }
    }
}
