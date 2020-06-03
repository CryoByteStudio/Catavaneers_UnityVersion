using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Catavaneer.MenuSystem
{
    public class TabGroup : MonoBehaviour
    {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private Color tabIdle;
        [SerializeField] private Color tabHover;
        [SerializeField] private Color tabActive;

        private TabButton selectedTab;
        private static EventSystem eventSystem;
        private int currentTabIndex = 0;

        private void OnEnable()
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        private void OnDisable()
        {
            eventSystem = null;
        }

        private IEnumerator GetEventSystem()
        {
            if (eventSystem) yield return null;

            eventSystem = FindObjectOfType<EventSystem>();
            yield return null;
        }

        private void Update()
        {
            StartCoroutine(GetEventSystem());

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                currentTabIndex = ((currentTabIndex - 1) % tabButtons.Count + tabButtons.Count) % tabButtons.Count;
                OnTabSelected(tabButtons[currentTabIndex]);
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                currentTabIndex = ((currentTabIndex + 1) % tabButtons.Count + tabButtons.Count) % tabButtons.Count;
                OnTabSelected(tabButtons[currentTabIndex]);
            }
        }

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null) tabButtons = new List<TabButton>();

            if (!tabButtons.Contains(button))
                tabButtons.Add(button);

            if (tabButtons.Count > 0 && !selectedTab)
            {
                if (tabButtons[0])
                {
                    currentTabIndex = 0;
                    selectedTab = tabButtons[0];
                    OnTabSelected(selectedTab);
                }
            }
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();

            if (selectedTab && selectedTab != button)
            {
                button.tabLabel.color = tabIdle;
                button.background.color = tabHover;
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            if (selectedTab != button)
            {
                currentTabIndex = tabButtons.IndexOf(button);
                selectedTab.tabPage.SetActive(false);
                selectedTab = button;
            }

            selectedTab.tabPage.SetActive(true);

            if (!eventSystem) throw new ArgumentNullException("evetSystem");
            if (!button) throw new ArgumentNullException("button");
            if (!button.firstSelected) throw new ArgumentNullException("button.firstSelected");

            eventSystem.SetSelectedGameObject(button.firstSelected);

            ResetTabs();
            button.tabLabel.color = tabIdle;
            button.background.color = tabActive;
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (selectedTab && selectedTab == button) continue;
                button.tabLabel.color = tabActive;
                button.background.color = tabIdle;
            }
        }
    }
}