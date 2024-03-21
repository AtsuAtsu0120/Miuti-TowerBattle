using System;
using TMPro;
using UnityEngine;

namespace Core.Components.Base
{
    public class InGameUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _versionText;

        public void Start()
        {
            _versionText.SetText("v" + Application.version);
        }

        public void SetScore(int score)
        {
            _scoreText.SetText(score.ToString());
        }
    }
}