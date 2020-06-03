using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Catavaneer.MenuSystem
{
    [RequireComponent(typeof(Image), typeof(TMP_Text))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TabGroup tabGroup;
        public Image background;
        public GameObject tabPage;
        public TMP_Text tabLabel;
        public GameObject firstSelected;

        private void Start()
        {
            if (tabGroup) tabGroup.Subscribe(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        private void OnValidate()
        {
            if (!background) background = GetComponent<Image>();
            if (!tabLabel) tabLabel = GetComponentInChildren<TMP_Text>();
            if (!tabGroup) tabGroup = FindObjectOfType<TabGroup>();
        }
    }
}