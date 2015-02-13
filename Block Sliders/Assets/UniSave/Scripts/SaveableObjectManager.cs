using System.Collections.Generic;
using System.Linq;
using UniSave.Components;
using UnityEngine;
using System;

namespace UniSave
{
    [ExecuteInEditMode]
    internal sealed class SaveableObjectManager : MonoBehaviour
    {
        [SerializeField] private List<SaveableObjectListItem> _saveableObjects = new List<SaveableObjectListItem>();

        public GameObject GameObjectOfLastDestroyedSO;
        public string IdOfLastDestroyedSO;

        private void Awake()
        {
            if (gameObject.name != "_SaveableObject Manager")
            {
                // Delayed Destroy to avoid Editor crash.
                Invoke("DelayDestroyThis", 0);
            }

            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                ClearDeletedGameObjects();
            }
        }

        private void ClearDeletedGameObjects()
        {
            foreach (var saveableObject in _saveableObjects.ToArray().Where(id => id.GameObject == null))
            {
                int i = _saveableObjects.FindIndex(x => x.Id == saveableObject.Id);
                _saveableObjects.RemoveAt(i);

                //Debug.Log("Non-existing GameObject removed from the SaveableObject Manager list.");
            }
        }

        private void DelayDestroyThis()
        {
            DestroyImmediate(this);
        }

        public void Add(string id, SaveableObject so)
        {
            if (_saveableObjects.Exists(x => x.GameObject == so.gameObject))
            {
                int index = _saveableObjects.FindIndex(x => x.GameObject == so.gameObject);
                _saveableObjects.RemoveAt(index);
            }

            _saveableObjects.Add(new SaveableObjectListItem(id, so));
        }

        public List<string> GetAllDestroyed()
        {
            var list = new List<string>();

            foreach (var item in _saveableObjects)
            {
                if (item.GameObject == null)
                {
                    list.Add(new Guid(item.Id).ToString());
                }
            }

            return list;
        }

        public SaveableObject Find(string id)
        {
            if (_saveableObjects.Exists(x => x.Id == id))
            {
                return _saveableObjects.Find(x => x.Id == id).SaveableObject;
            }

            return null;
        }

        public string Find(SaveableObject obj)
        {
            if (_saveableObjects.Exists(x => x.SaveableObject == obj))
            {
                return _saveableObjects.Find(x => x.SaveableObject == obj).Id;
            }

            return null;
        }

        public string Find(GameObject gameObj)
        {
            if (_saveableObjects.Exists(x => x.GameObject == gameObj))
            {
                int i = _saveableObjects.FindIndex(x => x.GameObject == gameObj);
                _saveableObjects[i] = new SaveableObjectListItem(_saveableObjects[i].Id, gameObj.GetComponent<SaveableObject>());

                return _saveableObjects[i].Id;
            }

            return null;
        }
    }
}