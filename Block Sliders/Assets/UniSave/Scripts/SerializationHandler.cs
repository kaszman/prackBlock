using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using ProtoBuf;
using System.Xml.Serialization;
using Numeria.IO;
using UnityEngine;

namespace UniSave
{
    // Handles all serialization related operations, including IO, and encryption/decryption.
    public sealed class SerializationHandler
    {
        private FastZip _fastZip;

        public SerializationHandler()
        {
            _fastZip = new FastZip { Password = "euWSPxcFdNX4lsph" };
            _fastZip.UseZip64 = UseZip64.Off;
        }

        private void CreateFilePath()
        {
            if (!Directory.Exists(SaveManager.CurrentFilePath))
            {
                Directory.CreateDirectory(SaveManager.CurrentFilePath);
            }
        }

        public FileType? GetFileType(string filePath)
        {
            var dbFilePath = String.Copy(filePath);
            dbFilePath += "_db";

            if (!File.Exists(filePath))
            {
                return null;
            }

            _fastZip.ExtractZip(filePath, SaveManager.CurrentFilePath, "");

            EntryInfo[] files = FileDB.ListFiles(dbFilePath);
            var file = files.FirstOrDefault(f => f.FileName == "metafile.");

            if (file != null)
            {
                Metafile metaFile;

                using (var memStream = new MemoryStream())
                {
                    FileDB.Read(dbFilePath, file.ID, memStream);
                    memStream.Position = 0;
                    metaFile = Deserialize<Metafile>(memStream);
                }

                return metaFile.GetFileType();
            }

            return null;
        }

        private void Init(string filePath)
        {
            CreateFilePath();

            FileDB.CreateEmptyFile(filePath, true);
        }

        public void Serialize(string filePath, List<SceneData> sceneData, Metafile metafile)
        {
            var dbFilePath = String.Copy(filePath);
            dbFilePath += "_db";

            if (File.Exists(filePath))
            {
                _fastZip.ExtractZip(filePath, SaveManager.CurrentFilePath, "");
            }

            else
            {
                Init(filePath);
            }

            EntryInfo[] files = FileDB.ListFiles(dbFilePath);

            foreach (SceneData data in sceneData)
            {
                SceneData tempData = data;

                foreach (var file in files.Where(file => file.FileName == tempData.SceneName + "."))
                {
                    FileDB.Delete(dbFilePath, file.ID);
                    //Debug.Log(String.Format("Previous scene file for {0} has been deleted.", data.SceneName));
                }

                using (var memStream = new MemoryStream())
                {
                    Serialize(memStream, data);
                    memStream.Position = 0;
                    FileDB.Store(dbFilePath, data.SceneName, memStream);
                }
            }

            var metaFile = files.FirstOrDefault(x => x.FileName == "metafile.");

            if (metaFile != null)
            {
                FileDB.Delete(dbFilePath, metaFile.ID);
                //Debug.Log("Previous Metafile for current save file has been deleted.");
            }

            using (var memStream = new MemoryStream())
            {
                Serialize(memStream, metafile);
                memStream.Position = 0;
                FileDB.Store(dbFilePath, "metafile", memStream);
            }

            _fastZip.CreateZip(filePath, SaveManager.CurrentFilePath, false, metafile.Name + "_db");
        }

        public void Serialize(string filePath, ObjectData objectData, Metafile metafile)
        {
            var dbFilePath = String.Copy(filePath);
            dbFilePath += "_db";

            if (File.Exists(filePath))
            {
                _fastZip.ExtractZip(filePath, SaveManager.CurrentFilePath, "");
            }

            else
            {
                Init(filePath);
            }

            EntryInfo[] files = FileDB.ListFiles(dbFilePath);

            var objectFile = files.FirstOrDefault(file => file.FileName == "object.");

            if (objectFile != null)
            {
                FileDB.Delete(dbFilePath, objectFile.ID);
            }

            using (var memStream = new MemoryStream())
            {
                Serialize(memStream, objectData);
                memStream.Position = 0;
                FileDB.Store(dbFilePath, "object", memStream);
            }

            var metaFile = files.FirstOrDefault(x => x.FileName == "metafile.");

            if (metaFile != null)
            {
                FileDB.Delete(dbFilePath, metaFile.ID);
                //Debug.Log("Previous Metafile for current save file has been deleted.");
            }

            using (var memStream = new MemoryStream())
            {
                Serialize(memStream, metafile);
                memStream.Position = 0;
                FileDB.Store(dbFilePath, "metafile", memStream);
            }

            _fastZip.CreateZip(filePath, SaveManager.CurrentFilePath, false, metafile.Name + "_db");
        }

        private void Serialize(Stream stream, object data)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(stream, data);
            }

            else
            {
                Serializer.Serialize(stream, data);
            }
        }

        private T Deserialize<T>(Stream stream)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var serializer = new XmlSerializer(typeof (T));
                return (T) serializer.Deserialize(stream);
            }

            return Serializer.Deserialize<T>(stream);
        }

        public T Deserialize<T>(string filePath, string sceneName)
        {
            var dbFilePath = String.Copy(filePath);
            dbFilePath += "_db";

            _fastZip.ExtractZip(filePath, SaveManager.CurrentFilePath, "");

            EntryInfo sceneFile = null;
            EntryInfo[] files = FileDB.ListFiles(dbFilePath);
            var file = files.FirstOrDefault(f => f.FileName == "metafile.");

            if (file != null)
            {
                Metafile metaFile;

                using (var memStream = new MemoryStream())
                {
                    FileDB.Read(dbFilePath, file.ID, memStream);
                    memStream.Position = 0;
                    metaFile = Deserialize<Metafile>(memStream);
                }

                FileType fileType = metaFile.GetFileType();
                if (fileType == FileType.SceneFile)
                {
                    if (!String.IsNullOrEmpty(sceneName))
                    {
                        sceneFile = files.FirstOrDefault(f => f.FileName == sceneName + ".");
                    }

                    else
                    {
                        sceneFile = files.FirstOrDefault(f => f.FileName == metaFile.LastLevel + ".");
                    }
                }

                else
                {
                    sceneFile = files.FirstOrDefault(f => f.FileName == "object.");
                }
            }

            if (sceneFile != null)
            {
                using (var memStream = new MemoryStream())
                {
                    FileDB.Read(dbFilePath, sceneFile.ID, memStream);
                    memStream.Position = 0;

                    return Deserialize<T>(memStream);
                }
            }

            return default(T);
        }

        public Metafile GetSaveMeta(string filePath)
        {
            Metafile metaFile = null;

            if (File.Exists(filePath))
            {
                var dbFilePath = String.Copy(filePath);
                dbFilePath += "_db";

                _fastZip.ExtractZip(filePath, SaveManager.CurrentFilePath, "");

                EntryInfo[] files = FileDB.ListFiles(dbFilePath);
                EntryInfo file = files.FirstOrDefault(f => f.FileName == "metafile.");

                if (file != null)
                { 
                    using (var memStream = new MemoryStream())
                    {
                        FileDB.Read(dbFilePath, file.ID, memStream);
                        memStream.Position = 0;
                        metaFile = Deserialize<Metafile>(memStream);
                    }
                }

                File.Delete(dbFilePath);
            }

            return metaFile;
        }

        public Metafile[] GetSaves(FileFilterMode filterMode, Metafield[] customFilters)
        {
            var directoryInfo = new DirectoryInfo(SaveManager.CurrentFilePath);

            FileInfo[] files;
            var metaFiles = new List<Metafile>();
            try
            {
                files = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTimeUtc).ToArray();
            }
            catch (Exception ex)
            {
                
                Debug.Log(ex.Message);
                return metaFiles.ToArray();
            }

            //Debug.Log("Save files found: " + files.Length);

            foreach (var file in files.Where(x => !x.Name.EndsWith("_db")))
            {
                _fastZip.ExtractZip(file.FullName, SaveManager.CurrentFilePath, "");

                EntryInfo[] list = {};

                try
                {
                    list = FileDB.ListFiles(file.FullName + "_db");
                }
                catch (Exception ex)
                {
                    Debug.Log(String.Format("Error reading '{0}'. {1}.", file.Name, ex.Message));
                    continue;
                }

                var saveInfoFile = list.FirstOrDefault(f => f.FileName == "metafile.");

                if (saveInfoFile != null)
                {
                    //Debug.Log(saveInfoFile.FileName + " " + saveInfoFile.FileLength);

                    using (var memStream = new MemoryStream())
                    {
                        FileDB.Read(file.FullName + "_db", saveInfoFile.ID, memStream);
                        memStream.Position = 0;

                        var metaFile = Deserialize<Metafile>(memStream);
                        metaFiles.Add(metaFile);
                    }
                }

                File.Delete(file.FullName + "_db");
            }

            return FilterMetafiles(metaFiles, filterMode, customFilters).ToArray();
        }

        private List<Metafile> FilterMetafiles(List<Metafile> metaFiles, FileFilterMode filterMode, Metafield[] filter)
        {
            foreach (var metaField in filter)
            {
                metaFiles.RemoveAll(x => x.FindMetafield(metaField.Name) == null);
            }

            foreach (var metaFile in metaFiles)
            {
                if (filterMode == FileFilterMode.SceneFile && metaFile.GetFileType() != FileType.SceneFile)
                {
                    metaFiles.Remove(metaFile);
                }

                else if (filterMode == FileFilterMode.ObjectFile && metaFile.GetFileType() != FileType.ObjectFile)
                {
                    metaFiles.Remove(metaFile);
                }
            }

            return metaFiles;
        }
    }
}
