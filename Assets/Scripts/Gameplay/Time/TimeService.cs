using System;
using UnityEngine;

namespace Gameplay.Time
{
    public interface ITimeService
    {
        event Action<float> Tick;
    }

    public class TimeService : MonoBehaviour, ITimeService
    {
        public event Action<float> Tick;

        private void Update()
        {
            Tick?.Invoke(UnityEngine.Time.deltaTime);
        }
    }
}
