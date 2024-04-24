using Gameplay.Core.Data;
using Unity.Entities;

namespace Gameplay.Core.Components
{
    public struct DataContent : IComponentData
    {
        public GameDataId DataId;

        public DataContent(GameData data)
        {
            DataId = data.Id;
        }
    }
}
