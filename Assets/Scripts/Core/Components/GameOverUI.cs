using Core.Components.Base;
using TMPro;
using UnityEngine;

namespace Core.Components
{
    public class GameOverUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI _resultScoreText;

        public void SetResultScore(int score)
        {
            _resultScoreText.SetText(score.ToString());
        }
    }
}
