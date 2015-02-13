using System;
using UniSave.Components;
using UnityEngine;

namespace UniSave
{
    [Serializable]
    internal sealed class SaveableObjectListItem
    {
        [SerializeField]    private string _id;
        [SerializeField]    private SaveableObject _saveableObject;
        [SerializeField]    private GameObject _gameObject;

        public string Id { get { return _id; } private set { _id = value; } }
        public SaveableObject SaveableObject { get { return _saveableObject; } private set { _saveableObject = value; } }
        public GameObject GameObject { get { return _gameObject; } private set { _gameObject = value; } }

        public SaveableObjectListItem(string id, SaveableObject obj)
        {
            Id = id;
            SaveableObject = obj;
            GameObject = obj.gameObject;
        }
	}
}
