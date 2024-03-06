using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Tracker.MongoDB.DBServices;
using Tracker.MongoDB.DBEntities;
using MongoDB.Driver;
using Tracker.MongoDB;
using MongoDB.Bson;
using Tracker.MongoDB.Models;
using Tracker.TelegramBot.Middleware;

class Program
{
    // Это клиент для работы с Telegram Bot API, который позволяет отправлять сообщения, управлять ботом, подписываться на обновления и многое другое.
    private static ITelegramBotClient _botClient;

    // Это объект с настройками работы бота. Здесь мы будем указывать, какие типы Update мы будем получать, Timeout бота и так далее.
    private static ReceiverOptions _receiverOptions;

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
        dProjectService.AddEntity(project1 );

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
        IMongoDatabase db = client.GetDatabase(Constants.DatabaseName);
        List<BsonDocument> collections = db.ListCollections().ToList();

        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();
        app.UseMiddleware<OptionsMiddleware>();
        app.MapControllers();
        app.UseCors();

        //app.UseCors(MyAllowSpecificOrigins);
        app.MapControllerRoute(name: "default", pattern: "{controller}/{action}");
        app.Run();




        /*
        _botClient = new TelegramBotClient("6546281602:AAGTAQP6uneF_QQbi7DEBqzmjX0iqP7FL1o"); // Присваиваем нашей переменной значение, в параметре передаем Token, полученный от BotFather
        _receiverOptions = new ReceiverOptions // Также присваем значение настройкам бота
        {
            AllowedUpdates = new[] // Тут указываем типы получаемых Update`ов, о них подробнее расказано тут https://core.telegram.org/bots/api#update
            {
                UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
            },
            // Параметр, отвечающий за обработку сообщений, пришедших за то время, когда ваш бот был оффлайн
            // True - не обрабатывать, False (стоит по умолчанию) - обрабаывать
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        // UpdateHander - обработчик приходящих Update`ов
        // ErrorHandler - обработчик ошибок, связанных с Bot API
        _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await _botClient.GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно*/
    }

    private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Обязательно ставим блок try-catch, чтобы наш бот не "падал" в случае каких-либо ошибок
        try
        {
            // Сразу же ставим конструкцию switch, чтобы обрабатывать приходящие Update
            switch (update.Type)
            {
                case UpdateType.Message:
                    {
                        // эта переменная будет содержать в себе все связанное с сообщениями
                        var message = update.Message;

                        // From - это от кого пришло сообщение (или любой другой Update)
                        var user = message.From;

                        // Выводим на экран то, что пишут нашему боту, а также небольшую информацию об отправителе
                        Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                        // Chat - содержит всю информацию о чате
                        var chat = message.Chat;
                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            message.Text, // отправляем то, что написал пользователь
                            replyToMessageId: message.MessageId // по желанию можем поставить этот параметр, отвечающий за "ответ" на сообщение
                            );

                        return;
                    }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        // Тут создадим переменную, в которую поместим код ошибки и её сообщение 
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}