using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Learning.Data;
using TMPro;
using Learning.Utils;

namespace Learning.MenuSystem
{
    public class SaveMenu : Menu<SaveMenu>
    {
        [SerializeField] private TMP_Text slot1;
        [SerializeField] private TMP_Text slot2;
        [SerializeField] private TMP_Text slot3;

        private string timeStamp;
        public int slotIndex = 0;

        override protected void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            int currentSaveSlot = DataManager.Instance.CurrentSaveSlot;

            DataManager.Instance.Load(0);
            slot1.text = DataManager.Instance.GetSaveDetails(0);
            DataManager.Instance.Load(1);
            slot2.text = DataManager.Instance.GetSaveDetails(1);
            DataManager.Instance.Load(2);
            slot3.text = DataManager.Instance.GetSaveDetails(2);

            DataManager.Instance.CurrentSaveSlot = currentSaveSlot;
        }

        public void OnSavePressed()
        {
            DataManager.Instance.CurrentSaveSlot = slotIndex;
            timeStamp = System.DateTime.Now.ToString();
            DataManager.Instance.SaveTimeStamp = timeStamp;
            DataManager.Instance.Save();
            string saveDetails = DataManager.Instance.GetSaveDetails(slotIndex);

            switch (slotIndex)
            {
                case 0:
                    slot1.text = saveDetails;
                    break;
                case 1:
                    slot2.text = saveDetails;
                    break;
                case 2:
                    slot3.text = saveDetails;
                    break;
                default:
                    break;
            }
        }

        public void OnSlot1Pressed()
        {
            slotIndex = 0;
        }

        public void OnSlot2Pressed()
        {
            slotIndex = 1;
        }

        public void OnSlot3Pressed()
        {
            slotIndex = 2;
        }
    }
}