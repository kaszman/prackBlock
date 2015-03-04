using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using UniSave.Components;
using UniSave.Containers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace UniSave
{
    /// <summary>
    /// Manages the saving and loading of data.
    /// </summary>
    public static class SaveManager
    {
        private static readonly Stopwatch StopWatch = new Stopwatch();
        private static readonly List<SceneData> SceneData = new List<SceneData>();

        private static Metafile _metafile;
        private static SerializationHandler _serializationHandler;
        private static MonoBehaviour _saveCoroutineHelper;

        /// <summary>
        /// Gets the path to the currently set directory where saves file will be saved to and loaded from.
        /// </summary>
        public static string CurrentFilePath { get; private set; }

        /// <summary>
        /// Gets a value indicating whether UniSave is currently saving.
        /// </summary>
        /// <returns>true if UniSave is saving; otherwise false.</returns>
        public static bool IsSaving { get; private set; }

        /// <summary>
        /// Gets a value indicating whether UniSave is currently loading.
        /// </summary>
        /// <returns>true if UniSave is loading; otherwise false.</returns>
        public static bool IsLoading { get; private set; }

        /// <summary>
        /// Gets the name of last loaded scene save file.
        /// </summary>
        public static string LastLoadedSceneSaveName { get; set; }

        /// <summary>
        /// Gets the name of last loaded object save file.
        /// </summary>
        public static string LastLoadedObjectSaveName { get; set; }

        static SaveManager()
        {
            SetFilePath("");
            _serializationHandler = new SerializationHandler();
        }

        #region Obsolete
        [Obsolete("A custom file extension can now be included in the file name itself.")]
        public const string FileExtension = ".sav";

        [Obsolete("Please use CurrentFilePath instead.")]
        public static string SavesDirectory
        {
            get { return CurrentFilePath; }
        }

        [Obsolete("Please use LastLoadedSceneFileName instead.")]
        public static string CurrentOrLastLoadedSaveName 
        { get { return LastLoadedSceneSaveName; } }

        [Obsolete("Please use SaveScene instead.")]
        public static void Save(string saveName)
        {
            SaveScene(saveName);
        }

        [Obsolete("Please use SaveScene instead.")]
        public static void Save(string saveName, Metafield[] metafields)
        {
            SaveScene(saveName, metafields);
        }
        #endregion

        /// <summary>
        /// Changes the current path where saves files are located to the path specified. Paths are relative to the location of '(Application.persistentDataPath)/Saves'. 
        /// For example, setting the path to "MyDir" will change the current location to  '(Application.persistentDataPath)/Saves/MyDir'.
        /// </summary>
        /// <param name="filePath"></param>
        public static void SetFilePath(string filePath)
        {
            CurrentFilePath = Application.persistentDataPath + Path.DirectorySeparatorChar + "Saves";

            if (!String.IsNullOrEmpty(filePath))
            {
                CurrentFilePath += Path.DirectorySeparatorChar + filePath;
            }
        }

        /// <summary>
        /// Clears all cached save data in memory. This gets called automatically when loading a new save file, 
        /// but could be called manually in a situation where you can quit the game back to the main menu.
        /// </summary>
        public static void ClearCache()
        {
            SceneData.Clear();
        }

        /// <summary>
        /// This is used internally by UniSave. Don't use this, but use DestroyObjectInScene(string sceneName, string tag) instead.
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="saveableObject"></param>
        public static void DestroyObjectInScene(string sceneName, SaveableObject saveableObject)
        {   
            var data = SceneData.Find(x => x.SceneName == sceneName);

            if (data == null)
            {
                //Debug.LogWarning(String.Format("Scene '{0}' not found in cached save data. Checking file now...", sceneName));

                if (String.IsNullOrEmpty(LastLoadedSceneSaveName))
                {
                    //Debug.LogWarning("UniSave: No save file was found.");
                    return;
                }

                string filePath = CurrentFilePath + Path.DirectorySeparatorChar + LastLoadedSceneSaveName;
                data = _serializationHandler.Deserialize<SceneData>(filePath, sceneName);

                if (data == null)
                {
                    //Debug.LogWarning(String.Format("Scene '{0}' not found in save file.", sceneName));
                    return;
                }
            }

            data.GameObjects.RemoveAll(x => x.ObjectId == saveableObject.Id);

            if (!saveableObject.WasInstantiated)
            {
                data.DestroyedGameObjectIDs.Add(saveableObject.Id);
            }
        }

        /// <summary>
        /// Destroys a GameObject in another scene.
        /// </summary>
        /// <param name="sceneName">The name of the scene that the GameObject is in.</param>
        /// <param name="tag">The tag of the GameObject.</param>
        public static void DestroyObjectInScene(string sceneName, string tag)
        {
            if (sceneName == Application.loadedLevelName)
            {
                Debug.LogWarning("UniSave: Don't use DestroyObjectInScene to destroy GameObjects in the current scene.");
                return;
            }

            var data = SceneData.Find(x => x.SceneName == sceneName);

            if (data == null)
            {
                //Debug.LogWarning(String.Format("Scene '{0}' not found in cached save data. Checking file now...", sceneName));

                string filePath = CurrentFilePath + Path.DirectorySeparatorChar + LastLoadedSceneSaveName;

                if (File.Exists(filePath))
                {
                    data = _serializationHandler.Deserialize<SceneData>(filePath, sceneName);
                }

                else
                {
                    //Debug.LogWarning("No save file was found.");
                }

                if (data == null)
                {
                    //Debug.LogWarning(String.Format("Scene [{0}] not found in save file.", sceneName));

                    var sceneData = new SceneData(sceneName);
                    sceneData.DestroyedGameObjectTags.Add(tag);
                    SceneData.Add(sceneData);

                    Debug.LogWarning(String.Format("UniSave: GameObject(s) with tag [{0}] will be destroyed in scene [{1}].", tag, sceneName));

                    return;
                }
            }

            foreach (var obj in data.GameObjects.ToArray().Where(obj => obj.GetValueOfMember<string>("tag") == tag))
            {
                data.GameObjects.Remove(obj);

                if (!obj.WasInstantiated)
                {
                    data.DestroyedGameObjectIDs.Add(obj.ObjectId);
                }
            }
        }

        /// <summary>
        /// Saves scene data to a file.
        /// </summary>
        /// <param name="saveName">The name of the save file that will hold the data.</param>
        public static void SaveScene(string saveName)
        {
            _saveCoroutineHelper = new GameObject("Saving").AddComponent<MonoBehaviour>();
            _saveCoroutineHelper.StartCoroutine(SaveEnumerator(saveName, null, FileType.SceneFile, null));
        }

        /// <summary>
        /// Saves scene data to a file with custom metadata.
        /// </summary>
        /// <param name="saveName">The name of the save file that will hold the data.</param>
        /// <param name="metafields">Custom metadata.</param>
        public static void SaveScene(string saveName, Metafield[] metafields)
        {
            _saveCoroutineHelper = new GameObject("Saving").AddComponent<MonoBehaviour>();
            _saveCoroutineHelper.StartCoroutine(SaveEnumerator(saveName, metafields, FileType.SceneFile, null));
        }

        /// <summary>
        /// Saves the specified game object to a file.
        /// </summary>
        /// <param name="fileName">The name of the save file that will hold the data.</param>
        /// <param name="gameObject">The game object that will be saved.</param>
        public static void SaveObject(string fileName, GameObject gameObject)
        {
            if (gameObject.GetComponent<SaveableObject>() == null)
            {
                Debug.LogWarning("The game object you're trying to save requires a SaveableObject component to be attached to it.");
                return;
            }

            _saveCoroutineHelper = new GameObject("Saving").AddComponent<MonoBehaviour>();
            _saveCoroutineHelper.StartCoroutine(SaveEnumerator(fileName, null, FileType.ObjectFile, gameObject));
        }

        /// <summary>
        /// Saves the specified game object to a file with custom metadata.
        /// </summary>
        /// <param name="fileName">The name of the save file that will hold the data.</param>
        /// <param name="gameObject">The game object that will be saved.</param>
        public static void SaveObject(string fileName, GameObject gameObject, Metafield[] metafields)
        {
            if (gameObject.GetComponent<SaveableObject>() == null)
            {
                Debug.LogWarning("The game object you're trying to save requires a SaveableObject component to be attached to it.");
                return;
            }

            _saveCoroutineHelper = new GameObject("Saving").AddComponent<MonoBehaviour>();
            _saveCoroutineHelper.StartCoroutine(SaveEnumerator(fileName, metafields, FileType.ObjectFile, gameObject));
        }

        /// <summary>
        /// Saves the current scene in memory.
        /// </summary>
        private static void SaveCurrentSceneData()
        {
            SceneData.RemoveAll(x => x.SceneName == Application.loadedLevelName);
            var data = new SceneData(Application.loadedLevelName);
            GameObjectContainer.BuildContainers(data);
            SceneData.Add(data);
        }

        private static IEnumerator SaveEnumerator(string saveName, Metafield[] metafields, FileType fileType, GameObject gameObject)
        {
            IsSaving = true;
            StopWatch.Start();

            string levelName = string.Empty;
            string filePath = CurrentFilePath + Path.DirectorySeparatorChar + saveName;
            Thread serializationThread;

            if (fileType == FileType.SceneFile)
            {
                levelName = Application.loadedLevelName;
                SaveCurrentSceneData();
                _metafile = new Metafile(saveName, DateTime.UtcNow.ToString("M/d/yyyy hh:mm tt"), levelName, metafields, fileType);
                serializationThread = new Thread(() => _serializationHandler.Serialize(filePath, SceneData, _metafile));
            }

            else
            {
                var objectData = new ObjectData();
                GameObjectContainer.BuildContainers(gameObject, objectData);
                _metafile = new Metafile(saveName, DateTime.UtcNow.ToString("M/d/yyyy hh:mm tt"), levelName, metafields, fileType);
                serializationThread = new Thread(() => _serializationHandler.Serialize(filePath, objectData, _metafile));
            }

            serializationThread.Start();

            while (serializationThread.IsAlive)
            {
                yield return null;
            }

            File.Delete(filePath + "_db");

            _metafile = null;

            if (fileType == FileType.SceneFile)
            {
                LastLoadedSceneSaveName = saveName;
            }

            else
            {
                LastLoadedObjectSaveName = saveName;
            }

            IsSaving = false;

            StopWatch.Stop();
            //Debug.Log(String.Format("{0}:{1}", StopWatch.Elapsed.Seconds, StopWatch.Elapsed.Milliseconds));
            StopWatch.Reset();

            SendMessage("UniSave_OnSavingCompleted", saveName);

            GameObject.Destroy(_saveCoroutineHelper.gameObject);
        }

        /// <summary>
        /// Loads a save file.
        /// </summary>
        /// <param name="saveName">The save file that will be loaded.</param>
        public static void Load(string saveName)
        {
            Load(saveName, null, false, 0, 0);
        }

        /// <summary>
        /// Loads a save file with a static loading screen.
        /// </summary>
        /// <param name="saveName">Name of the save file..</param>
        /// <param name="loadScreen">Path of a loading screen image located in a resources folder.</param>
        public static void Load(string saveName, string loadScreen)
        {
            Load(saveName, loadScreen, false, 0, 0);
        }

        /// <summary>
        /// Loads a save file with a fade-in/fade-out effect.
        /// </summary>
        /// <param name="saveName">Name of the save file.</param>
        /// <param name="fadeInTime">The amount of time (in seconds) it takes to fully fade in.</param>
        /// <param name="fadeOutTime">The amount of time (in seconds) it takes to fully fade out.</param>
        public static void Load(string saveName, float fadeInTime, float fadeOutTime)
        {
            Load(saveName, null, true, fadeInTime, fadeOutTime);
        }

        private static void Load(string saveName, string loadScreen, bool fadeScreen, float fadeInTime, float fadeOutTime)
        {
            if (!File.Exists(CurrentFilePath + Path.DirectorySeparatorChar + saveName))
            {
                Debug.LogWarning(String.Format("UniSave: Save file [{0}] could not be found.", saveName));
                return;
            }

            IsLoading = true;
            StopWatch.Start();

            //Metafile info = SerializationHandler.GetSaveMeta(_fileLocation + Path.DirectorySeparatorChar + saveName + FileExtension);

            var sceneTransition = new GameObject("Loading Screen").AddComponent<SceneTransition>();

            if (!String.IsNullOrEmpty(loadScreen))
            {
                sceneTransition.Init(loadScreen, false);
            }

            else if (fadeScreen)
            {
                sceneTransition.Init(fadeInTime, fadeOutTime, false);
            }

            else
            {
                sceneTransition.Init();
            }

            sceneTransition.StartCoroutine(LoadEnumerator(null, false, saveName, fadeInTime));
        }

        private static IEnumerator LoadEnumerator(string levelName, bool isPersistent, string saveName, float loadDelay)
        {
            yield return new WaitForSeconds(loadDelay);

            SceneData sceneData = null;


            if (isPersistent)
            {
                //SaveInMemory();

                sceneData = SceneData.Find(x => x.SceneName == levelName);

                // If not in memory
                if (sceneData == null)
                {
                    //Debug.LogWarning("No scene data found in memory.");

                    // Check if there's a current save file
                    if (!String.IsNullOrEmpty(LastLoadedSceneSaveName))
                    {
                        string filePath = CurrentFilePath + Path.DirectorySeparatorChar + LastLoadedSceneSaveName;
                        sceneData = _serializationHandler.Deserialize<SceneData>(filePath, levelName);

                        File.Delete(filePath + "_db");
                    }
                }

                Application.LoadLevel(levelName);

                while (Application.isLoadingLevel)
                {
                    yield return null;
                }

                if (sceneData != null)
                {
                    GameObjectContainer.UnpackContainers(sceneData);
                }
            }

            else
            {
                var type = _serializationHandler.GetFileType(CurrentFilePath + Path.DirectorySeparatorChar + saveName);
                if (type != null)
                {
                    var fileType = (FileType) type;

                    switch (fileType)
                    {
                        case FileType.SceneFile:
                        {
                            var saveableObjects =
                                (SaveableObject[]) UnityEngine.Object.FindObjectsOfType(typeof (SaveableObject));

                            foreach (var obj in saveableObjects)
                            {
                                UnityEngine.Object.Destroy(obj.gameObject);
                            }

                            // Explicit file loading
                            if (!String.IsNullOrEmpty(saveName))
                            {
                                ClearCache();
                                string filePath = CurrentFilePath + Path.DirectorySeparatorChar + saveName;
                                sceneData = _serializationHandler.Deserialize<SceneData>(filePath, string.Empty);

                                levelName = sceneData.SceneName;
                                LastLoadedSceneSaveName = saveName;
                                //Debug.Log(String.Format("UniSave: Current save file is now [{0}].", saveName));
                            }

                            Application.LoadLevel(levelName);

                            while (Application.isLoadingLevel)
                            {
                                yield return null;
                            }

                            if (sceneData != null)
                            {
                                GameObjectContainer.UnpackContainers(sceneData);
                            }
                        }

                            break;

                        case FileType.ObjectFile:
                        {
                            string filePath = CurrentFilePath + Path.DirectorySeparatorChar + saveName;
                            var objectData = _serializationHandler.Deserialize<ObjectData>(filePath, string.Empty);

                            if (objectData != null)
                            {
                                GameObjectContainer.UnpackContainers(objectData);
                            }
                        }
                            break;
                    }
                }

                File.Delete(CurrentFilePath + Path.DirectorySeparatorChar + saveName + "_db");
            }

            IsLoading = false;

            StopWatch.Stop();
            //Debug.Log(String.Format("{0}:{1}", StopWatch.Elapsed.Seconds, StopWatch.Elapsed.Milliseconds));
            StopWatch.Reset();

            SendMessage("UniSave_OnLoadingCompleted");
        }

        /// <summary>
        /// Loads a level.
        /// </summary>
        /// <param name="sceneName">The name of the scene that will be loaded.</param>
        /// <param name="isPersistent">Sets a value whether the scene will be loaded in MultiScene Persistency mode.</param>
        public static void LoadLevel(string sceneName, bool isPersistent)
        {
            LoadLevel(sceneName, isPersistent, null, false, 0, 0);
        }

        /// <summary>
        /// Loads a level with a static loading screen.
        /// </summary>
        /// <param name="sceneName">The name of the scene that will be loaded.</param>
        /// <param name="isPersistent">Sets a value whether the scene will be loaded in MultiScene Persistency mode.</param>
        /// <param name="loadScreen">Path of a loading screen image located in a resources folder.</param>
        public static void LoadLevel(string sceneName, bool isPersistent, string loadScreen)
        {
            LoadLevel(sceneName, isPersistent, loadScreen, false, 0, 0);
        }

        /// <summary>
        /// Loads a level with a fade-in/fade-out effect.
        /// </summary>
        /// <param name="sceneName">The name of the level that will be loaded.</param>
        /// <param name="isPersistent">Sets a value whether the scene will be loaded in MultiScene Persistency mode.</param>
        /// <param name="fadeInTime">The amount of time (in seconds) it takes to fully fade in.</param>
        /// <param name="fadeOutTime">The amount of time (in seconds) it takes to fully fade out.</param>
        public static void LoadLevel(string sceneName, bool isPersistent, float fadeInTime, float fadeOutTime)
        {
            LoadLevel(sceneName, isPersistent, null, true, fadeInTime, fadeOutTime);
        }

        private static void LoadLevel(string sceneName, bool isPersistent, string loadScreen , bool fadeScreen, float fadeInTime, float fadeOutTime)
        {
            IsLoading = true;
            StopWatch.Start();
            
            if (isPersistent)
            {
                SaveCurrentSceneData();
            }

            var sceneTransition = new GameObject("Scene Transition").AddComponent<SceneTransition>();

            if (!String.IsNullOrEmpty(loadScreen))
            {
                sceneTransition.Init(loadScreen, isPersistent);
            }

            else if (fadeScreen)
            {
                sceneTransition.Init(fadeInTime, fadeOutTime, isPersistent);
            }

            else
            {
                sceneTransition.Init();
            }

            sceneTransition.StartCoroutine(LoadEnumerator(sceneName, isPersistent, null, fadeInTime));

        }

        /// <summary>
        /// Deletes a save file from disk in the directory set by CurrentFilePath.
        /// </summary>
        /// <param name="saveName">The name of the save file that will be deleted.</param>
        public static void Delete(string saveName)
        {
            var path = CurrentFilePath + Path.DirectorySeparatorChar + saveName;

            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }

                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Returns a list of all save files found in the directory set by CurrentFilePath.
        /// </summary>
        /// <returns></returns>
        public static Metafile[] GetSavesList()
        {
            return GetSavesList(FileFilterMode.All);
        }

        /// <summary>
        /// Returns a list of all save files found in the directory set by CurrentFilePath.
        /// Filter the result by the type of save file and/or the names of custom metafields.
        /// </summary>
        /// <returns></returns>
        public static Metafile[] GetSavesList(FileFilterMode filterMode, params Metafield[] customFilters)
        {
            return _serializationHandler.GetSaves(filterMode, customFilters);
        }

        private static void SendMessage(string callbackName, object value = null)
        {
            var gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (var gameObject in gameObjects)
            {
                gameObject.SendMessage(callbackName, value, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
