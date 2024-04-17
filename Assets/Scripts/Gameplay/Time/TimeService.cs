using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameplay.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
    }

    public class TimeService : ITimeService
    {
        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}
