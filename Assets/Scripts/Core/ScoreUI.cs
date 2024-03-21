using System;
using System.Collections.Generic;
using Core.Components.Base;
using TMPro;
using UnityEngine;

namespace Core
{
    public class ScoreUI : UIBase
    {
        [SerializeField] private List<TextMeshProUGUI> _highScoreTexts;
        public void Start()
        {
            _highScoreTexts[0].SetText("1 : {0}", ScoreManager.Score1);
            _highScoreTexts[1].SetText("2 : {0}", ScoreManager.Score2);
            _highScoreTexts[2].SetText("3 : {0}", ScoreManager.Score3);
            _highScoreTexts[3].SetText("4 : {0}", ScoreManager.Score4);
            _highScoreTexts[4].SetText("5 : {0}", ScoreManager.Score5);
        }
        
        public void SetHighScore(int score, int index)
        {
            _highScoreTexts[index].SetText("{0} : {1}", index, score);
        }
    }
}