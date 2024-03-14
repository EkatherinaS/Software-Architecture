namespace Tracker.TelegramBot.Controllers.Entities
{
    public class TaskIterator
    {
        private object[] _objects;
        private int current = -1;
        public TaskIterator(object[] objects) 
        {
            _objects = objects;
        }

        public bool hasNext() { return current < _objects.Length; }
        public object next() { current++; return _objects[current]; }
    }
}


