using System;
using System.Collections.Generic;

namespace Gameplay.Tasks
{
    public interface ITaskService
    {
        void AssingTask(Guid characterId, Task task);
        bool TryGetTask(Guid characterId, out Task task);
    }

    public class TaskService : ITaskService
    {
        private readonly Dictionary<Guid, Task> tasks = new();
        public void AssingTask(Guid characterId, Task task)
        {
            tasks[characterId] = task;
        }

        public bool TryGetTask(Guid characterId, out Task task)
        {
            return tasks.TryGetValue(characterId, out task);
        }
    }
}
