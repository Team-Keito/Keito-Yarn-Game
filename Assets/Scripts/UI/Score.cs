using UnityEngine;
using UnityEngine.Events;


namespace Manager.Score
{
    [System.Serializable]
    public class ScoreSystem
    {
        public float ScoreMultiplier = 1;
        public float FavoriteColorMultiplier = 2;
        public float FavoriteColorFlatBonus = 0;

        public UnityEvent<ScoreData> OnCatScored;

        [System.NonSerialized]
        public float Score;
        [System.NonSerialized]
        public float HighScore;

        public ScoreData UpdateScore(float value, ColorSO favColor, bool isFavoriteColor)
        {
            //Score based on Suika scoring.
            float scaledValue = value * ScoreMultiplier;
            float baseScore = Mathf.Max(1, (scaledValue * (scaledValue + 1) / 2));
            float bonusScore = 0;

            if (isFavoriteColor)
            {
                bonusScore = (baseScore * (FavoriteColorMultiplier - 1)) + FavoriteColorFlatBonus;
            }

            Score += baseScore + bonusScore;
            HighScore = Mathf.Max(Score, HighScore);

            ScoreData data = new(Mathf.RoundToInt(baseScore), Mathf.RoundToInt(bonusScore), isFavoriteColor, favColor, FavoriteColorMultiplier);

            OnCatScored.Invoke(data);

            return data;
        }
    }


    public struct ScoreData
    {
        public int value;
        public int baseValue;
        public int bonus;

        public float multiplier;

        public bool isFavoriteColor;
        public ColorSO color;

        public ScoreData(int baseValue, int bonus, bool isFavoritColor, ColorSO color, float multiplier)
        {
            value = baseValue + bonus;

            this.isFavoriteColor = isFavoritColor;
            this.color = color;
            this.baseValue = baseValue;
            this.bonus = bonus;
            this.multiplier = multiplier;
        }
    }
}

