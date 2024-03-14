using Tracker.TelegramBot.Controllers.Entities;

namespace Tracker.TelegramBot.Services
{
    public class UserService
    {
        UserEntity user;

        private State _state;

        public UserService()
        {
            user = new UserEntity();
            _state = new Registration();
        }

        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
                Console.WriteLine("State: " + _state.GetType().Name);
            }
        }

        public void Request()
        {
            _state.Handle(this);
        }

        public void AddFIO(string fio)
        {
            user.FIO = fio;
        }
        public void AddUsername(string username)
        {
            user.Username = username;
        }
        public void AddNickname(string nickname)
        {
            user.Nickname = nickname;
        }
        public void AddStatus(string status)
        {
            user.Status = status;
        }
        public void AddCompany(CompanyEntity company)
        {
            user.Company = company;
        }
    }

    public abstract class State
    {
        public abstract void Handle(UserService context);
    }

    public class Registration : State
    {
        public override void Handle(UserService context)
        {
            context.State = new WaitingForAction();
        }
    }

    public class WaitingForAction : State
    {
        public override void Handle(UserService context)
        {
            context.State = new WritingMassage();
        }
    }

    public class WritingMassage : State
    {
        public override void Handle(UserService context)
        {
            context.State = new WaitingForAction();
        }
    }
}
