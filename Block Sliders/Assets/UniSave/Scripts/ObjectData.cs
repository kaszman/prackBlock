using System.Collections.Generic;
using ProtoBuf;
using UniSave.Containers;

namespace UniSave
{
    [ProtoContract]
    public sealed class ObjectData
    {
        [ProtoMember(1)] public List<GameObjectContainer> GameObjects { get { return _gameObjects ?? (_gameObjects = new List<GameObjectContainer>()); } }
        [ProtoMember(2)] public OptimizationContainer Optimization = new OptimizationContainer();

        private List<GameObjectContainer> _gameObjects;

        public ObjectData()
        {
            OptimizationContainer._instance = Optimization;
        }
    }
}
