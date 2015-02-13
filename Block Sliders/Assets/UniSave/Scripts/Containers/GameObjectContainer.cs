using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using UniSave.Components;
using UnityEngine;

namespace UniSave.Containers
{
    [ProtoContract]
	public sealed class GameObjectContainer : Container
	{
        [ProtoMember(1)] public List<GameObjectContainer> _children = new List<GameObjectContainer>();
        [ProtoMember(2)] public List<ComponentContainer> _components = new List<ComponentContainer>();

        private const string InstanceNameSuffix = "(Clone)";

        [ProtoMember(3)] public string ObjectId { get; private set; }
        [ProtoMember(4)] public bool WasInstantiated { get; private set; }
        [ProtoMember(5)] public int ObjectNameIndex { get; private set; }
       
        private GameObjectContainer(SaveableObject saveableObject)
        {
            ObjectId = saveableObject.Id;
            WasInstantiated = saveableObject.WasInstantiated;

            if (WasInstantiated)
            {
                var separator = new[] { InstanceNameSuffix };
                ObjectNameIndex = (OptimizationContainer.AddObjectName(saveableObject.gameObject.name.Split(separator, 2, StringSplitOptions.None)[0]));
            }

            else
            {
                ObjectNameIndex = OptimizationContainer.AddObjectName(saveableObject.gameObject.name);
            }

            //Debug.Log("ObjectName: " + ObjectName);
        }

        public static void BuildContainers(SceneData data)
        {
            var saveableObjects = ((SaveableObject[]) UnityEngine.Object.FindObjectsOfType(typeof(SaveableObject))).Where(obj => !obj.IgnoreSaveScene);

            var manager = (SaveableObjectManager) UnityEngine.Object.FindObjectOfType(typeof(SaveableObjectManager));

            if (manager != null)
            {
                foreach (var id in manager.GetAllDestroyed())
                {
                    data.DestroyedGameObjectIDs.Add(id);
                    //Debug.LogWarning("Added to destroyed " + id);
                }
            }

            foreach (var saveableObject in saveableObjects.Where(x => x.transform.parent == null && x.enabled))
            {
                //Debug.Log(saveableObject.gameObject.name);
                var container = CreateContainer(saveableObject);
                data.GameObjects.Add(container);
            }
        }

        public static void BuildContainers(GameObject gameObject, ObjectData data)
        {
            var saveableObject = gameObject.GetComponent<SaveableObject>();
            var container = CreateContainer(saveableObject);
            data.GameObjects.Add(container);
        }

        public static void UnpackContainers(SceneData sceneData)
        {
            OptimizationContainer._instance = sceneData.Optimization;

            var defaultObjects = GameObject.FindObjectsOfType(typeof(SaveableObject)) as SaveableObject[];

            foreach (var gameObjectContainer in sceneData.GameObjects)
            {
                GameObject gameObject;

                if (!gameObjectContainer.WasInstantiated)
                {
                    gameObject = defaultObjects.Where(x => x.Id == gameObjectContainer.ObjectId).Select(x => x.gameObject).FirstOrDefault();

                    if (gameObject == null)
                    {
                        Debug.LogWarning(String.Format("UniSave: Non-instantiated GameObject [{0}] was not found in the scene. Skipping.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                        continue;
                    }
                }

                else
                {
                    //Debug.LogWarning(gameObjectContainer.ObjectName);
                    var inst = Resources.Load(OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex), typeof(GameObject));

                    if (inst != null)
                    {
                        try
                        {
                            gameObject = (GameObject)UnityEngine.Object.Instantiate(inst);
                        }

                        catch (InvalidCastException)
                        {
                            Debug.LogWarning(String.Format("UniSave: Prefab [{0}] cannot be found. It has either been removed, or it is located outside of a Resources folder.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                            continue;
                        }

                        //Debug.Log("Instantiated: " + gameObject.name);
                    }

                    else
                    {
                        Debug.LogWarning(String.Format("UniSave: Prefab [{0}] cannot be found. It has either been removed, or it is located outside of a Resources folder.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                        continue;
                    }
                }

                Unpack(gameObjectContainer, gameObject);
            }

            foreach (var name in sceneData.DestroyedGameObjectIDs)
            {
                string name1 = name;

                if (defaultObjects != null)
                {
                    //Debug.LogWarning(name1);
                    GameObject gameObject = defaultObjects.Where(x => x.Id == name1 && !x.WasInstantiated).Select(x => x.gameObject).FirstOrDefault();

                    if (gameObject != null)
                    {
                        //Debug.LogWarning(gameObject.name + " destroyed.");
                        GameObject.Destroy(gameObject);
                    }
                }
            }

            foreach (var name in sceneData.DestroyedGameObjectTags)
            {
                string name1 = name;

                if (defaultObjects != null)
                {
                    //Debug.LogWarning(name1);
                    GameObject gameObject = defaultObjects.Where(x => x.tag == name1 && !x.WasInstantiated).Select(x => x.gameObject).FirstOrDefault();

                    if (gameObject != null)
                    {
                        //Debug.LogWarning(gameObject.name + " destroyed.");
                        GameObject.Destroy(gameObject);
                    }
                }
            }
        }

        public static void UnpackContainers(ObjectData objectData)
        {
            OptimizationContainer._instance = objectData.Optimization;

            var defaultObjects = GameObject.FindObjectsOfType(typeof(SaveableObject)) as SaveableObject[];

            foreach (var gameObjectContainer in objectData.GameObjects)
            {
                GameObject gameObject;

                if (!gameObjectContainer.WasInstantiated)
                {
                    gameObject = defaultObjects.Where(x => x.Id == gameObjectContainer.ObjectId).Select(x => x.gameObject).FirstOrDefault();

                    if (gameObject == null)
                    {
                        Debug.LogWarning(String.Format("UniSave: Non-instantiated GameObject [{0}] was not found in the scene. Skipping.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                        continue;
                    }
                }

                else
                {
                    //Debug.LogWarning(gameObjectContainer.ObjectName);
                    var inst = Resources.Load(OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex), typeof(GameObject));

                    if (inst != null)
                    {
                        try
                        {
                            gameObject = (GameObject)UnityEngine.Object.Instantiate(inst);
                        }

                        catch (InvalidCastException)
                        {
                            Debug.LogWarning(String.Format("UniSave: Prefab [{0}] cannot be found. It has either been removed, or it is located outside of a Resources folder.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                            continue;
                        }

                        //Debug.Log("Instantiated: " + gameObject.name);
                    }

                    else
                    {
                        Debug.LogWarning(String.Format("UniSave: Prefab [{0}] cannot be found. It has either been removed, or it is located outside of a Resources folder.", OptimizationContainer.GetObjectName(gameObjectContainer.ObjectNameIndex)));
                        continue;
                    }
                }

                Unpack(gameObjectContainer, gameObject);
            }
        }

        private static GameObjectContainer CreateContainer(SaveableObject saveableObject)
        {
            var container = new GameObjectContainer(saveableObject);

            GetMembers(saveableObject.gameObject, container, typeof(GameObject));
            GetComponents(saveableObject, container);

            foreach (Transform child in saveableObject.transform)
            {
                //Debug.LogWarning(child.name + " is a child of " + saveableObject.name);
                var comp = child.GetComponent<SaveableObject>();

                if (comp != null)
                {
                    var childContainer = CreateContainer(comp);
                    container._children.Add(childContainer);
                }
            }
            
            return container;
        }

        private static void GetComponents(SaveableObject saveableObject, GameObjectContainer container)
        {
            foreach (var component in saveableObject.GetSelectedComponents())
            {
                ComponentContainer componentContainer = ComponentContainer.CreateContainer(component);
                container._components.Add(componentContainer);
            }
        }

        private static void Unpack(GameObjectContainer container, GameObject gameObject)
        {
            // Set Fields/Properties on the GameObject
            foreach (var member in container.Members)
            {
                SetMembers(gameObject, member);
            }

            // Set Components on the GameObject
            foreach (var compContainer in container._components)
            {
                ComponentContainer.Unpack(compContainer, gameObject, container);
            }

            //Debug.LogWarning("CHILDREN COUNT: " + container._children.Count);

            // Set Children
            foreach (var child in container._children)
            {
                //Debug.LogWarning(child.ObjectName);

                GameObject obj;

                try 
                {
                    obj = (gameObject.transform.Cast<Transform>().
                               Select(t => t.GetComponent<SaveableObject>()).
                               Where(s => s.Id == child.ObjectId).
                               Select(s => s)
                              ).
                               Select(s => s.gameObject).
                               FirstOrDefault();
                }

                catch
                {
                    obj = null;
                }

                if (child.WasInstantiated)
                {
                    //Debug.LogWarning(child.ObjectName + "| Is Instantiated");

                    foreach (Transform trans in gameObject.transform)
                    {
                        var so = trans.GetComponent<SaveableObject>();

                        /*
                        if (so.Id == container.ObjectId)
                        {
                            Debug.LogWarning("CHILD FOUND!");
                            obj = so.gameObject;
                            break;
                        }
                        */

                        if (trans.name == OptimizationContainer.GetObjectName(child.ObjectNameIndex))
                        {
                            //Debug.LogWarning("CHILD FOUND!");
                            obj = so.gameObject;
                            break;
                        }
                    }

                    //Debug.LogWarning(child.ObjectName + "| Is Prefab");

                    if (obj == null)
                    {
                        Debug.LogWarning(String.Format("UniSave: Prefab child [{0}] was not found. Skipping.", OptimizationContainer.GetObjectName(child.ObjectNameIndex)));
                        return;
                    }
                }

                else
                {
                    if (obj == null)
                    {
                        Debug.LogWarning(String.Format("UniSave: Non-instantiated GameObject child [{0}] was not found. Skipping.", OptimizationContainer.GetObjectName(child.ObjectNameIndex)));
                        return;
                    }
                }

                Unpack(child, obj);
            }
        }

        public T GetValueOfMember<T>(string memberName)
        {
            Type type = (typeof (T));

            object value;

            try
            {
                value =
                    Members.Where(x => OptimizationContainer.GetMemberName(x.NameIndex) == memberName && OptimizationContainer.TypesAreIdentical(type, x.ObjectTypeIndex))
                        .Select(x => OptimizationContainer.GetValue(x))
                        .First();
            }

            catch (ArgumentNullException exception)
            {
                Debug.LogException(exception);
                return default(T);
            }

            return (T) Convert.ChangeType(value, typeof(T));
        }

        public object GetValueOfMember(string memberName, Type type)
        {
            object value;

            try
            {
                value =
                    Members.Where(x => OptimizationContainer.GetMemberName(x.NameIndex) == memberName && OptimizationContainer.TypesAreIdentical(type, x.ObjectTypeIndex))
                        .Select(x => OptimizationContainer.GetValue(x))
                        .First();
            }

            catch (ArgumentNullException exception)
            {
                Debug.LogException(exception);
                return null;
            }

            return Convert.ChangeType(value, type);
        }

        // For ProtoBuf deserialization.
        private GameObjectContainer()
        {
        }
	}
}
