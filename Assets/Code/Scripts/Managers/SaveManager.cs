using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game.Managers
{
    public class SaveManager : MonoBehaviour
    {
        [Serializable]
        public struct GlobalSaveData
        {
            public int lastAvailableLevelIndex;
        }

        [Serializable]
        public struct LevelSaveData
        {
            public bool isFinished;
            public float recordTime;
        }

        [Serializable]
        private struct LevelDataWrapper
        {
            public List<LevelSaveData> levelsData;
        }

        public event Action OnDataChanged;

        public static SaveManager Instance { get; private set; }

        private static readonly GlobalSaveData DEFAULT_GLOBAL_SAVE = new()
        {
            lastAvailableLevelIndex = 0
        };
        private static readonly LevelSaveData DEFAULT_LEVEL_SAVE = new()
        {
            isFinished = false,
            recordTime = float.NaN
        };
         
        public GlobalSaveData Data
        {
            get => m_data;
            set
            {
                m_data = value;
                OnDataChanged?.Invoke();
            }
        }
        
        private GlobalSaveData m_data;
        private List<LevelSaveData> m_levelsData;
        private string m_globalDataPath;
        private string m_levelsDataPath;

        public void Load()
        {
            m_data = LoadJsonFile(m_globalDataPath, in DEFAULT_GLOBAL_SAVE);

            var defaultLevelsData = new LevelDataWrapper()
            {
                levelsData = new()
            };
            var levelsDataWrapper = LoadJsonFile(m_levelsDataPath, in defaultLevelsData);
            m_levelsData = levelsDataWrapper.levelsData;
        }

        public void Save()
        {
            SaveJsonFile(m_globalDataPath, in m_data);

            var levelsDataWrapper = new LevelDataWrapper()
            {
                levelsData = m_levelsData
            };
            SaveJsonFile(m_levelsDataPath, levelsDataWrapper);
        }

        public LevelSaveData GetLevelData(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= LevelManager.Instance.LevelsCount)
            {
                throw new IndexOutOfRangeException();
            }

            if (levelIndex >= m_levelsData.Count)
            {
                return DEFAULT_LEVEL_SAVE;
            }

            return m_levelsData[levelIndex];
        }

        public void SetLevelData(int levelIndex, LevelSaveData data)
        {
            if (levelIndex < 0 || levelIndex >= LevelManager.Instance.LevelsCount)
            {
                throw new IndexOutOfRangeException();
            }

            if (levelIndex >= m_levelsData.Count)
            {
                m_levelsData.Capacity = levelIndex + 1;
                for (int i = 0; i < levelIndex - m_levelsData.Count + 1; ++i)
                {
                    m_levelsData.Add(DEFAULT_LEVEL_SAVE);
                }
            }
            
            m_levelsData[levelIndex] = data;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            m_globalDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "globalSave.json";
            m_levelsDataPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "levelsSave.json";

            Load();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private static T LoadJsonFile<T>(string path, in T defaultValue) where T : struct
        {
            if (!File.Exists(path))
            {
                return defaultValue;
            }

            using var f = File.OpenRead(path);

            var buffer = new byte[f.Length];
            f.Read(buffer, 0, buffer.Length);

            var json = Encoding.UTF8.GetString(buffer);
            return JsonUtility.FromJson<T>(json);
        }

        private static void SaveJsonFile<T>(string path, in T value) where T : struct
        {
            var json = JsonUtility.ToJson(value);
            var buffer = Encoding.UTF8.GetBytes(json);

            using var f = File.OpenWrite(path);
            f.Write(buffer, 0, buffer.Length);
        }
    }
}
