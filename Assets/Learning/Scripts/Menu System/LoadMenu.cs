using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Learning.Data;

namespace Learning.MenuSystem
{
    public class LoadMenu : Menu<LoadMenu>
    {
        [SerializeField] private TMP_Text slot1;
        [SerializeField] private TMP_Text slot2;
        [SerializeField] private TMP_Text slot3;
        [SerializeField] private Button loadButton;

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

        public void OnLoadPressed()
        {
            switch (slotIndex)
            {
                case 0:
                    if (slot1.text.ToLower() != DataManager.EmptyText.ToLower())
                        DataManager.Instance.Load(slotIndex);
                    break;
                case 1:
                    if (slot2.text.ToLower() != DataManager.EmptyText.ToLower())
                        DataManager.Instance.Load(slotIndex);
                    break;
                case 2:
                    if (slot3.text.ToLower() != DataManager.EmptyText.ToLower())
                        DataManager.Instance.Load(slotIndex);
                    break;
                default:
                    break;
            }
        }

        public void OnSlot1Pressed()
        {
            if (slot1.text.ToLower() != DataManager.EmptyText.ToLower())
            {
                slotIndex = 0;
                loadButton.interactable = true;
            }
            else
            {
                loadButton.interactable = false;
            }
        }

        public void OnSlot2Pressed()
        {
            if (slot2.text.ToLower() != DataManager.EmptyText.ToLower())
            {
                slotIndex = 1;
                loadButton.interactable = true;
            }
            else
            {
                loadButton.interactable = false;
            }
        }

        public void OnSlot3Pressed()
        {
            if (slot3.text.ToLower() != DataManager.EmptyText.ToLower())
            {
                slotIndex = 2;
                loadButton.interactable = true;
            }
            else
            {
                loadButton.interactable = false;
            }
        }
    }
}