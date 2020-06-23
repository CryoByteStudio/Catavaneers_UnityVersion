using System.Collections;
using UnityEngine;
using TMPro;
using SpawnSystem;
using SpawnSystem.Standard;
using DG.Tweening;

public class WaveCountdownDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textField;
    [SerializeField] private SpawnManager spawnManagerReference;
    [SerializeField] private float scaleSize;
    [SerializeField] private bool isTextDisplay;

    private bool hasStartedCountDown = false;
    private float countDownTimer = 0;

    private void Start()
    {
        if (!textField)
            textField = GetComponentInChildren<TMP_Text>();
        if (!spawnManagerReference)
            spawnManagerReference = FindObjectOfType<SpawnManager>();
    }

    private void Update()
    {
        if (spawnManagerReference && textField)
        {
            countDownTimer = GetCountdownTime();
            textField.text = !IsValidCountdown(countDownTimer) ? "" : isTextDisplay ? "Next Wave In" : string.Format("{0:0}s", countDownTimer);

            if (IsValidCountdown(countDownTimer) && !hasStartedCountDown && !isTextDisplay)
            {
                StartCoroutine(Popup());
            }
        }

        if (!spawnManagerReference)
            StartCoroutine(FindSpawnManagerReference());
    }

    private IEnumerator Popup()
    {
        hasStartedCountDown = true;
        textField.transform.DOScale(scaleSize, 0.45f).SetLoops(2, LoopType.Yoyo);
        Debug.Log("Popup: " + countDownTimer);
        yield return new WaitForSeconds(1);
        hasStartedCountDown = false;
    }

    private bool IsValidCountdown(float countDown)
    {
        return countDown != Mathf.Infinity && countDown != Mathf.NegativeInfinity;
    }

    private float GetCountdownTime()
    {
        return spawnManagerReference.NextWaveTime - spawnManagerReference.TimeElapsed;
    }

    private IEnumerator FindSpawnManagerReference()
    {
        spawnManagerReference = FindObjectOfType<SpawnManager>();
        yield return null;
    }
}
