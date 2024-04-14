using MongoDB.Driver;
using Newtonsoft.Json;
using RestSharp;
using Tracker.TelegramBot.Controllers.Entities;

namespace Tracker.TelegramBot.Controllers
{
    public class TrackerRepositoryApi
    { 
        private static RestClient client;

        public TrackerRepositoryApi(string connectionString) 
        {
            client = new RestClient(connectionString);
        }

        public void postGPSEntity(GPSEntity dBGPS)
        {
            RestRequest request = new RestRequest("postGPSEntity");
            request.AddJsonBody(dBGPS);
            client.Post(request);
        }

        public void postProjectEntity(ProjectEntity project)
        {
            RestRequest request = new RestRequest("postProjectEntity");
            request.AddJsonBody(project);
            client.Post(request);
        }

        public void postTaskEntity(TaskEntity task)
        {
            RestRequest request = new RestRequest("postTaskEntity");
            request.AddJsonBody(task);
            client.Post(request);
        }

        public bool deleteProject(ProjectEntity ProjectEntity)
        {
            RestRequest request = new RestRequest("deleteProject");
            request.AddJsonBody(ProjectEntity);
            return JsonConvert.DeserializeObject<bool>(client.Delete(request).Content);
        }

        public bool deleteTask(TaskEntity TaskEntity)
        {
            RestRequest request = new RestRequest("deleteTask");
            request.AddJsonBody(TaskEntity);
            return JsonConvert.DeserializeObject<bool>(client.Delete(request).Content);
        }

        public bool checkIfProjectExists(ProjectEntity projectEntity)
        {
            RestRequest request = new RestRequest("checkIfProjectExists");
            request.AddJsonBody(projectEntity);
            return JsonConvert.DeserializeObject<bool>(client.Post(request).Content);
        }

        public bool checkIfTaskExists(TaskEntity taskEntity)
        {
            RestRequest request = new RestRequest("checkIfTaskExists");
            request.AddJsonBody(taskEntity);
            return JsonConvert.DeserializeObject<bool>(client.Post(request).Content);
        }

        public List<ProjectEntity> getProjectsByChatId(long chatId)
        {
            RestRequest request = new RestRequest("getProjectsByChatId");
            request.AddParameter("chatId", chatId);
            return JsonConvert.DeserializeObject<ProjectEntity[]>(client.Get(request).Content).ToList();
        }

        public List<TaskEntity> getTasksByChatId(long chatId)
        {
            RestRequest request = new RestRequest("getTasksByChatId");
            request.AddParameter("chatId", chatId);
            return JsonConvert.DeserializeObject<List<TaskEntity>>(client.Get(request).Content);
        }

        public UserEntity getUserByChatId(long chatId)
        {
            RestRequest request = new RestRequest("getUserByChatId");
            request.AddParameter("chatId", chatId);
            return JsonConvert.DeserializeObject<UserEntity>(client.Get(request).Content);
        }

        public GetInfoEntity getInitDataByChatId(long chatId)
        {
            RestRequest request = new RestRequest("getInitDataByChatId");
            request.AddParameter("chatId", chatId);
            return JsonConvert.DeserializeObject<GetInfoEntity>(client.Get(request).Content);
        }

        public void putStartTask(TaskEntity updatedTaskEntity)
        {
            RestRequest request = new RestRequest("putStartTask");
            request.AddJsonBody(updatedTaskEntity);
            client.Put(request);
        }

        public void putEndTask(TaskEntity updatedTaskEntity)
        {
            RestRequest request = new RestRequest("putEndTask");
            request.AddJsonBody(updatedTaskEntity);
            client.Put(request);
        }
    }
}
