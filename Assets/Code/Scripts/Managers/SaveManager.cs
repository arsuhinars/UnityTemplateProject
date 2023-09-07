using Game.Utils;
using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Game.Managers
{
    public class SaveManager : MonoBehaviour
    {
        public event Action<SaveData> OnDataChanged;

        public static SaveManager Instance { get; private set; }

        public SaveData Data => m_data;

        private string m_progressDataPath;
        private SaveData m_data;

        public void Save()
        {
            var json = JsonUtility.ToJson(m_data);
            var buffer = Encoding.UTF8.GetBytes(json);

            using var f = File.OpenWrite(m_progressDataPath);
            f.Write(buffer, 0, buffer.Length);
        }

        public void Load()
        {
            if (File.Exists(m_progressDataPath))
            {
                using var f = File.OpenRead(m_progressDataPath);

                var buffer = new byte[f.Length];
                f.Read(buffer, 0, buffer.Length);

                var json = Encoding.UTF8.GetString(buffer);
                m_data = JsonUtility.FromJson<SaveData>(json);
            }
            else
            {
                m_data = new SaveData();
            }
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
            m_progressDataPath = Application.persistentDataPath + Path.PathSeparator + "save.json";

            Load();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Update()
        {
            if (m_data.ChangesFlag)
            {
                m_data.ChangesFlag = false;
                OnDataChanged?.Invoke(m_data);
            }
        }
    }
}
