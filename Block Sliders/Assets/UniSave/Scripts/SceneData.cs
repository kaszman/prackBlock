using System.Collections.Generic;
using ProtoBuf;
using UniSave.Containers;

namespace UniSave
{
    [ProtoContract]
	public sealed class SceneData
    {
        [ProtoMember(1)] public List<GameObjectContainer> GameObjects { get { return _gameObjects ?? (_gameObjects = new List<GameObjectContainer>()); }  }
        [ProtoMember(2)] public List<string> DestroyedGameObjectIDs { get { return _destroyedGameObjectIDs ?? (_destroyedGameObjectIDs = new List<string>()); }  }
        [ProtoMember(3)] public string SceneName { get; private set; }

        [ProtoMember(4)] public List<string> DestroyedGameObjectTags { get {return _destroyedGameObjectTags ?? (_destroyedGameObjectTags = new List<string>()); } }

        [ProtoMember(5)] public OptimizationContainer Optimization = new OptimizationContainer();

        private List<GameObjectContainer> _gameObjects;
        private List<string> _destroyedGameObjectIDs;
        private List<string> _destroyedGameObjectTags; 

        public SceneData(string sceneName)
        {
            SceneName = sceneName;
            OptimizationContainer._instance = Optimization;
        }

        // For ProtoBuf deserialization.
        private SceneData()
        {
        }
    }
}
