 using Catavaneer.MenuSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Catavaneer
{
    public class LevelSelector : MonoBehaviour
    {
        public int maxDays;
        public List<TravelPoint> destinations = new List<TravelPoint>();
        public TravelPoint lastEncounter;
        public TravelPoint currentPoint;
        public Transform caravan;
        public TMP_Text daytText;
        //bool travelling = false;
        //float startLerpTime;
        //float travelCurrentDistance;
        //float travelTotalDistance;
        //public float travelSpeed;

        // Moving caravan
        private bool hasStartedTravelling = false;
        private float startTime;
        private float totalTravelDist;
        private float distTravelled;
        public float tolerantDist;
        public float maxSpeed;
        public float minSpeed;
        public float acceleration;
        private float currentSpeed;
        private Quaternion lookRotation;

        private int buttonPressCount = 0;

        public ConfirmPopup confirmPopup;

        void Start()
        {
            GameManager.CurrentDay++;
            lastEncounter = destinations[GameManager.LastEncounterIndex];
            currentPoint = destinations[GameManager.LastEncounterIndex];
            caravan.transform.position = currentPoint.transform.position;
            
            //foreach (TravelPoint point in destinations)
            //{
            //    if (destinations.IndexOf(point) <= destinations.IndexOf(currentPoint))
            //    {
            //        point.GetComponentInParent<Renderer>().material = currentPoint.SelectedMat;
            //    }
            //}

            daytText.text = "Day " + GameManager.CurrentDay + "/ " + maxDays;
        }

        private void Update()
        {
            if (confirmPopup && confirmPopup.IsFocused) return;

            if (Input.GetButtonDown("Submit/Interact") && ButtonSmashPreventor.ShouldProceed(ref buttonPressCount))
            {
                GoToNext();
            }
            else if (!hasStartedTravelling)
            {
                if (Input.GetButtonDown("Cancel/Shop3"))
                {
                    // Confirm action with popup warning
                    if (confirmPopup)
                        confirmPopup.ExecuteOnConfirm(() =>
                        {
                            GameManager.ResetCampaignParams();
                            MenuManager.LoadMainMenuLevel();
                        }, () => ResetButtonPressCount());
                }
            }
        }

        private void ResetButtonPressCount()
        {
            buttonPressCount = 0;
        }

        //void FixedUpdate()
        //{
        //    if (travelling)
        //    {
        //        float travelDist = (Time.time - startLerpTime) * travelSpeed;

        //        Caravan.transform.position = Vector3.Lerp(lastEncounter.transform.position, currentPoint.transform.position, travelDist / travelTotalDistance);
        //        if (Vector3.Distance(Caravan.transform.position, currentPoint.transform.position) <= 1f)
        //        {
        //            lastEncounter = currentPoint;
        //            GameManager.LastEncounterIndex = destinations.IndexOf(currentPoint);
        //            travelling = false;
        //            //SceneManager.LoadScene(currentPoint.leveltoload);
        //            MenuManager.LoadGameLevel(currentPoint.leveltoload);
        //        }
        //    }
        //}

        private IEnumerator TravelToNextPoint()
        {
            int nextPointIndex = destinations.IndexOf(currentPoint) + 1;

            if (nextPointIndex < destinations.Count)
            {
                SetUpTrip(nextPointIndex);

                while (totalTravelDist - distTravelled > tolerantDist)
                {
                    Move();
                    yield return null;
                }

                if (!GameManager.Instance.HasFinishedAllLevel)
                {
                    lastEncounter = currentPoint;
                    GameManager.LastEncounterIndex = destinations.IndexOf(currentPoint);
                    MenuManager.LoadGameLevel(currentPoint.leveltoload);
                }
                else
                {
                    MenuManager.LoadMainMenuLevel(true);
                }
            }
        }

        private void SetUpTrip(int nextPointIndex)
        {
            currentPoint = destinations[nextPointIndex];

            distTravelled = 0;
            hasStartedTravelling = true;

            totalTravelDist = Vector3.Distance(currentPoint.transform.position, caravan.transform.position);
            lookRotation = Quaternion.LookRotation(currentPoint.transform.position - lastEncounter.transform.position);
        }

        private void Move()
        {
            if (distTravelled < totalTravelDist / 2)
            {
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }
            else
            {
                currentSpeed -= acceleration * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, minSpeed);
            }

            distTravelled += currentSpeed * Time.deltaTime;

            float t = distTravelled / totalTravelDist;
            caravan.position = Vector3.Lerp(lastEncounter.transform.position, currentPoint.transform.position, t);
            caravan.rotation = Quaternion.Slerp(caravan.transform.rotation, lookRotation, t);
        }

        public void GoToNext()
        {
            if (!hasStartedTravelling)
            {
                //currentPoint = destinations[destinations.IndexOf(currentPoint) + 1];
                //travelling = true;
                //startLerpTime = Time.time;
                //travelTotalDistance = Vector3.Distance(lastEncounter.transform.position, currentPoint.transform.position);
                //Caravan.transform.LookAt(currentPoint.transform.position);

                StartCoroutine(TravelToNextPoint());
            }
            else
            {
                Debug.LogWarning("Wait until the next level is loaded!");
            }
        }
    }
}