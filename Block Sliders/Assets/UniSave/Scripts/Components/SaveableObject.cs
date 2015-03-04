using System;

#if UNITY_EDITOR
    using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace UniSave.Components
{
    [ExecuteInEditMode, AddComponentMenu("UniSave/SaveableObject")]
    public sealed class SaveableObject : MonoBehaviour
    {
        [SerializeField, HideInInspector] private bool _ignoreSaveScene;

        [SerializeField, HideInInspector] private Component[] _components;
        [SerializeField, HideInInspector] private List<string> _componentNames = new List<string>();
        [SerializeField, HideInInspector] private List<bool> _componentToggles = new List<bool>();

        [SerializeField, HideInInspector] private SaveableObjectManager _saveableObjectManager;
        [SerializeField, HideInInspector] private string _id;
        [SerializeField, HideInInspector] private bool _set;
        [SerializeField, HideInInspector] private bool _awakeCalled;

        private string _previousLevel;
        private string _currentLevel;
        private bool _isQuitting;
        private bool _isDuplicateComponent;

        public bool IgnoreSaveScene { get { return _ignoreSaveScene; } }

        /// <summary>
        /// Gets the unique identifier of this SaveableObject.
        /// </summary>
        public string Id
        {
            get
            {
                if (String.IsNullOrEmpty(_id))
                {
                    return Guid.Empty.ToString();
                }

                return new Guid(_id).ToString();
            }

            private set { _id = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the GameObject was instantiated at runtime.
        /// </summary>
        public bool WasInstantiated { get; private set; }

        private void OnValidate()
        {
            //Debug.Log(gameObject.name + ": OnValidate() called.");

            DetectComponents();

            // Handles when a prefab is being created from a gameobject in the scene.
            #if UNITY_EDITOR
            if (PrefabUtility.GetPrefabParent(gameObject) == null && PrefabUtility.GetPrefabObject(gameObject) != null)
            {
                //Debug.Log("Prefab created.");
                _set = false;
                _id = Guid.Empty.ToString();
            }
            
            // Makes sure the ID isn't reset if the Revert button is pressed in the editor if this is a prefab instance.
            else if (PrefabUtility.GetPrefabParent(gameObject) != null)
            {
                Init();

                if (String.IsNullOrEmpty(_saveableObjectManager.IdOfLastDestroyedSO))
                {
                    var id = _saveableObjectManager.Find(this);

                    // Makes sure handles correctly when adding a prefab instance to the scene.
                    if (id != null)
                    {
                        _id = id;
                        _set = true;
                    }
                }

                else
                {
                    var id = _saveableObjectManager.IdOfLastDestroyedSO;
                    //Debug.Log(id);

                    _id = id;
                    _saveableObjectManager.Add(_id, this);
                    _set = true;

                    _saveableObjectManager.IdOfLastDestroyedSO = null;
                    _saveableObjectManager.GameObjectOfLastDestroyedSO = null;
                }
            }
            #endif
        }

        private void OnEnable()
        {
            //Debug.Log(gameObject.name + ": OnEnable() called.");
        }

        private void Reset()
        {
            if (GetComponents<SaveableObject>().Length > 1)
            {
                _isDuplicateComponent = true;
                // Delayed Destroy to avoid Editor crash.
                Invoke("DelayedDestroyThis", 0);
            }

            DetectComponents();

            //Debug.Log(gameObject.name + ": Reset() called.");

            // Makes sure this only gets called when it's not a prefab and only when Reset() gets called manually in the editor.
            #if UNITY_EDITOR
            if (PrefabUtility.GetPrefabObject(gameObject) == null || PrefabUtility.GetPrefabParent(gameObject) != null)
            {
                if (!_awakeCalled)
                {
                    Init();
                    Undo.RecordObject(_saveableObjectManager, "Reset SaveableObject");

                    var id = _saveableObjectManager.Find(this);
                    _id = id;
                    _set = true;

                    //Debug.LogWarning("Reset called manually in the editor.");
                }
            }
            #endif
        }

        private void Init()
        {
            var manager = GameObject.Find("_SaveableObject Manager");

            if (manager == null)
            {
                manager = new GameObject("_SaveableObject Manager");

                _saveableObjectManager = manager.AddComponent<SaveableObjectManager>();
            }

            else
            {
                _saveableObjectManager = manager.GetComponent<SaveableObjectManager>();
            }
        }

        private void DestroyThis()
        {
            DestroyImmediate(this, true);
        }

        private void Awake()
        {
            //Debug.Log(Awake called.");
            _awakeCalled = true;

            Init();
            
            if (!Application.isPlaying)
            {
                if (_id != Guid.Empty.ToString() && !string.IsNullOrEmpty(_id))
                {
                    SaveableObject so = _saveableObjectManager.Find(_id);

                    if (so != this)
                    {
                        Id = Guid.NewGuid().ToString();
                        //Debug.Log("Duplicate GameObject found, Generating new ID: " + Id);
                        _saveableObjectManager.Add(_id, this);
                    }
                }
            }

            if (!_set)
            {
                _set = true;

                if (!Application.isPlaying)
                {
                    string id = _saveableObjectManager.Find(gameObject);

                    if (!string.IsNullOrEmpty(id))
                    {
                        Id = id;
                    }

                    else
                    {
                        WasInstantiated = false;
                        Id = Guid.NewGuid().ToString();
                        //Debug.Log(Id);

                        _saveableObjectManager.Add(_id, this);
                    }
                }

                else
                {
                    WasInstantiated = true;
                    DetectComponents();
                    //Debug.Log(name + ": Runtime Spawned GameObject");
                }
            }

            if (Application.isPlaying)
            {
                _currentLevel = Application.loadedLevelName;

                if (transform.root.name.Contains("(Clone)"))
                {
                    WasInstantiated = true;
                    //Debug.Log(name + ": Instantiated GameObject");
                }
            }

            //Debug.Log(name + ": " + Id);
        }

        /// <summary>
        /// Refreshes the internal list of components that are on the GameObject, and shows them in the editor.
        /// </summary>
        public void DetectComponents()
        {
            _components = GetComponents<Component>().Where(x => x != null).ToArray();

            foreach (string componentName in _componentNames.ToArray())
            {
                var tempNames = _components.Select(component => component.GetType().Name).ToList();

                if (!tempNames.Contains(componentName))
                {
                    int i = _componentNames.IndexOf(componentName);

                    //Debug.Log("Removed: " + _componentNames[i]);

                    _componentNames.RemoveAt(i);
                    _componentToggles.RemoveAt(i);
                }
            }

            foreach (Component component in _components)
            {
                if (!_componentNames.Contains(component.GetType().Name))
                {
                    // Ignores the Halo and FlareLayer components because their types 
                    // are recognized as 'Behaviour' and are therefore not saveable.
                    if (component.GetType().Name != "Behaviour")
                    {
                        _componentNames.Add(component.GetType().Name);
                        _componentToggles.Add(false);

                        //Debug.Log("Added: " + component.GetType().Name);
                    }
                }
            }
        }
        
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                if (_isDuplicateComponent)
                {
                    return;
                }

                _saveableObjectManager.GameObjectOfLastDestroyedSO = gameObject;
                _saveableObjectManager.IdOfLastDestroyedSO = Id;
            }

            if (Application.isPlaying && !Application.isLoadingLevel && !_isQuitting && !WasInstantiated)
            {
                Destroy(gameObject);
            }
        }

        private void OnLevelWasLoaded()
        {
            if (_currentLevel != Application.loadedLevelName)
            {
                _previousLevel = _currentLevel;
                _currentLevel = Application.loadedLevelName;

                if (!WasInstantiated)
                {
                    if (transform.parent == null)
                    {
                        var prefab = Resources.Load(gameObject.name);

                        if (prefab == null)
                        {
                            Debug.LogWarning(String.Format("UniSave: [{0}] is not a prefab! GameObjects with a SaveableObject on them that " +
                                                           "move between scenes must be a prefab or else they cannot be saved in the new scene.", 
                                                           gameObject.name));
                            return;
                        }
                    }
                }

                SaveManager.DestroyObjectInScene(_previousLevel, this);

                //Debug.LogWarning(String.Format("Deleted [{0}] from previous level data.", name));

                WasInstantiated = true;
            }
        }

        /// <summary>
        /// Gets a list of the components on this SaveableObject that were selected in the editor.
        /// </summary>
        /// <returns></returns>
        public List<Component> GetSelectedComponents()
        {
            var components = new List<Component>();

            for (int i = 0; i < _componentToggles.Count; i++)
            {
                if (_componentToggles[i])
                {
                    components.Add(_components[i]);
                }
            }

            return components;
        }
    }
}