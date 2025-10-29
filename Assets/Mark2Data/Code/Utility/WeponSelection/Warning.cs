using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.DropDownData;
using Utility.Weapons_Data;

namespace Utility.WarningSignal
{
    public class Warning : MonoBehaviour
    {
        public DropDownMenu Data;
        public EnemyDropDownMenu Data2;
        public GameObject playButton;
        public GameObject warningSign;

        private int reftence_total_data;
        private void Start()
        {
            Data = GetComponent<DropDownMenu>();
            Data2 = GetComponent<EnemyDropDownMenu>();
            warningSign.SetActive(false);
            if (Data)
                reftence_total_data = Data.totalWeight;
            else { reftence_total_data = Data2.totalWeight; }
        }
        private void FixedUpdate()
        {
            if (Data)
                reftence_total_data = Data.totalWeight;
            else { reftence_total_data = Data2.totalWeight; }
            if (reftence_total_data > 10800)
            {
                HidePlayBtn();
            }
            else
            {
                ShowPlayBtn();
            }
        }

        private void ShowPlayBtn()
        {
            playButton.SetActive(true);
            warningSign.SetActive(false);
        }

        private void HidePlayBtn()
        {
            playButton.SetActive(false);
            warningSign.SetActive(true);
        }
    }
}

