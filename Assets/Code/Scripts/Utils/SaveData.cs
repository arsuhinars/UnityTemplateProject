using System;
using UnityEngine;

namespace Game.Utils
{
    [Serializable]
    public class SaveData
    {
        public int MaxPassedLevelIndex
        {
            get => m_maxPassedLevelIndex;
            set => m_maxPassedLevelIndex = value;
        }
        public bool ChangesFlag
        {
            get => m_changesFlag;
            set => m_changesFlag = value;
        }

        [SerializeField] private float[] m_levelsRecords = new float[0];
        [SerializeField] private int m_maxPassedLevelIndex = -1;

        private bool m_changesFlag = false;

        public float GetLevelRecord(int levelIndex)
        {
            if (levelIndex >= m_levelsRecords.Length)
            {
                return 0f;
            }

            return m_levelsRecords[levelIndex];
        }

        public void SetLevelRecord(int levelIndex, float record)
        {
            if (levelIndex >= m_levelsRecords.Length)
            {
                var oldRecords = m_levelsRecords;
                m_levelsRecords = new float[levelIndex + 1];
                oldRecords.CopyTo(m_levelsRecords, 0);
            }

            m_levelsRecords[levelIndex] = record;
        }
    }
}
