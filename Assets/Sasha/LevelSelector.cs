using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Catavaneer
{
    public class LevelSelector : MonoBehaviour
    {
        public int maxdays;
        public List<TravelPoint> destinations = new List<TravelPoint>();
        public TravelPoint lastEncounter;
        public TravelPoint currentPoint;
        public GameObject Caravan;
        public Text dayttext;
        bool travelling = false;
        //public CharacterManager cman;
        float startLerpTime;
        float travelCurrentDistance;
        float travelTotalDistance;
        public float travelSpeed;
        // Start is called before the first frame update
        void Start()
        {
            //cman = FindObjectOfType<CharacterManager>();
            //cman.CurrentDay++;
            //lastEncounter = destinations[cman.LastEncounterIndex];
            //currentPoint = destinations[cman.LastEncounterIndex];

            GameManager.CurrentDay++;
            lastEncounter = destinations[GameManager.LastEncounterIndex];
            currentPoint = destinations[GameManager.LastEncounterIndex];
            Caravan.transform.position = currentPoint.transform.position;


            foreach (TravelPoint point in destinations)
            {
                if (destinations.IndexOf(point) <= destinations.IndexOf(currentPoint))
                {
                    point.GetComponentInParent<Renderer>().material = currentPoint.SelectedMat;
                }
            }
            //dayttext.text = "Day " + cman.CurrentDay + "/ " + maxdays;
            dayttext.text = "Day " + GameManager.CurrentDay + "/ " + maxdays;
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                GoToNext();
            }
        }

        void FixedUpdate()
        {
            if (travelling)
            {
                float travelDist = (Time.time - startLerpTime) * travelSpeed;

                Caravan.transform.position = Vector3.Lerp(lastEncounter.transform.position, currentPoint.transform.position, travelDist / travelTotalDistance);
                if (Vector3.Distance(Caravan.transform.position, currentPoint.transform.position) <= 1f)
                {
                    lastEncounter = currentPoint;
                    //cman.LastEncounterIndex = destinations.IndexOf(currentPoint);
                    GameManager.LastEncounterIndex = destinations.IndexOf(currentPoint);
                    travelling = false;
                    SceneManager.LoadScene(currentPoint.leveltoload);
                }
            }
        }

        public void GoToNext()
        {
            if (!travelling)
            {
                currentPoint = destinations[destinations.IndexOf(currentPoint) + 1];
                travelling = true;
                startLerpTime = Time.time;
                travelTotalDistance = Vector3.Distance(lastEncounter.transform.position, currentPoint.transform.position);
                Caravan.transform.LookAt(currentPoint.transform.position);
            }
            else
            {
                Debug.LogWarning("Wait until the caravan stops moving to travel!");
            }
        }
    }
}