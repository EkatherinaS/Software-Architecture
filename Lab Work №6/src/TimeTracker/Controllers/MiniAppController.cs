using MongoDB.Driver;
using Tracker.TelegramBot.Controllers.Entities;
using Microsoft.AspNetCore.Mvc;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;


namespace Tracker.TelegramBot.Controllers
{

    public class MiniAppController : ControllerBase
    {
        private static TrackerRepositoryApi trackerRepositoryApi = new TrackerRepositoryApi("http://host.docker.public:8081/TrackerRepository/");


        [HttpGet]
        public ActionResult GetInitData(long chatId)
        //получить имя текущей задачи, проекта и время начала выполнения
        {
            Console.WriteLine("GetInitData called");
            GetInfoEntity info = trackerRepositoryApi.getInitDataByChatId(chatId);

            if (info == null)
            {
                return NotFound(chatId);
            }
            else
            {
                return Ok(info);
            }
        }


        [HttpGet]
        public ActionResult GetProjectList(long chatId)
        //получить список проектов для текущего пользователя
        {
            Console.WriteLine("GetProjectList called");
            if (trackerRepositoryApi.getUserByChatId(chatId) == null)
            {
                return NotFound(chatId);
            }

            List<ProjectEntity> projects = trackerRepositoryApi.getProjectsByChatId(chatId);
            return Ok(projects);
        }


        [HttpGet]
        public ActionResult GetTaskList(long chatId)
        //получить список задач для текущего пользователя
        {
            Console.WriteLine("GetTaskList called");
            if (trackerRepositoryApi.getUserByChatId(chatId) == null)
            {
                return NotFound(chatId);
            }

            List<TaskEntity> tasks = trackerRepositoryApi.getTasksByChatId(chatId);
            return Ok(tasks);
        }


        [HttpPost]
        public ActionResult<ProjectEntity> PostNewTask([FromBody] TaskEntity task)
        //создать новую задачу для текущего пользователя
        {
            Console.WriteLine("PostNewTask called");
            trackerRepositoryApi.postTaskEntity(task);
            if (trackerRepositoryApi.checkIfTaskExists(task))
            {
                Console.WriteLine("PostNewTask - task created");
                return CreatedAtAction(nameof(trackerRepositoryApi.postTaskEntity), new { task }, task);
            }
            return NotFound(task);

        }


        [HttpPost]
        public ActionResult<ProjectEntity> PostNewProject(ProjectEntity project)
        //создать новый проект для текущего пользователя
        {
            Console.WriteLine("PostNewProject called");
            trackerRepositoryApi.postProjectEntity(project);
            Console.WriteLine("PostNewProject - project created");

            return CreatedAtAction(nameof(trackerRepositoryApi.postProjectEntity), new { project }, project);
        }


        [HttpDelete]
        public ActionResult<TaskEntity> DeleteTask([FromBody] TaskEntity taskEntity)
        //удалить задачу
        {
            Console.WriteLine("DeleteTask called");
            if (taskEntity == null)
            {
                Console.WriteLine("DeleteTask - task not found");
                return NotFound();
            }

            bool deleteSucceded = trackerRepositoryApi.deleteTask(taskEntity);

            if (deleteSucceded)
            {
                Console.WriteLine("DeleteTask - task deleted");
                return Ok(taskEntity);
            }
            else
            {
                Console.WriteLine("DeleteTask - task not found");
                return NotFound(taskEntity);
            }
        }


        [HttpDelete]
        public ActionResult<ProjectEntity> DeleteProject([FromBody] ProjectEntity projectEntity)
        //удалить проект
        {
            Console.WriteLine("DeleteProject called");
            if (projectEntity == null)
            {
                Console.WriteLine("DeleteProject - project not found");
                return NotFound();
            }

            bool deleteSucceded = trackerRepositoryApi.deleteProject(projectEntity);

            if (deleteSucceded)
            {
                Console.WriteLine("DeleteProject - project deleted");
                return Ok(projectEntity);
            }
            else
            {
                Console.WriteLine("DeleteProject - project not found");
                return NotFound(projectEntity);
            }
        }


        [HttpPut]
        public ActionResult<TaskEntity> PutStartTask([FromBody] TaskEntity startedTask)
        //запустить задачу
        {
            Console.WriteLine("PutStartTask called");
            if (startedTask.StartTime == null)
            {
                return BadRequest("PutStartTask - StartTime is null");
            }

            if (startedTask.StartLatitude == null || startedTask.StartLongitude == null)
            {
                return BadRequest("PutStartTask - StartPosition is null");
            }

            GPSEntity dBGPS = new GPSEntity();
            dBGPS.Latitude = startedTask.StartLatitude.Value;
            dBGPS.Longitude = startedTask.StartLongitude.Value;
            trackerRepositoryApi.postGPSEntity(dBGPS);

            trackerRepositoryApi.putStartTask(startedTask);

            if (!trackerRepositoryApi.checkIfTaskExists(startedTask))
            {
                return BadRequest("PutStartTask - error updating the task");
            }

            Console.WriteLine("PutStartTask - task started");
            return Ok(startedTask);
        }


        [HttpPut]
        public ActionResult<TaskEntity> PutEndTask([FromBody] TaskEntity endedTask)
        //остановить задачу
        {
            Console.WriteLine("PutEndTask called");
            if (endedTask.EndTime == null)
            {
                return BadRequest("PutEndTask - EndTime is null");
            }

            if (endedTask.EndLatitude == null || endedTask.EndLongitude == null)
            {
                return BadRequest("PutEndTask - EndPosition is null");
            }

            GPSEntity dBGPS = new GPSEntity();
            dBGPS.Latitude = endedTask.EndLatitude.Value;
            dBGPS.Longitude = endedTask.EndLongitude.Value;
            trackerRepositoryApi.postGPSEntity(dBGPS);

            trackerRepositoryApi.putEndTask(endedTask);

            if (!trackerRepositoryApi.checkIfTaskExists(endedTask))
            {
                return BadRequest("PutEndTask - error updating the task");
            }

            Console.WriteLine("PutEndTask - task ended");
            return Ok(endedTask);
        }
    }
}
