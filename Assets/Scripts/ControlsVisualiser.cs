using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsVisualiser : MonoBehaviour
{
    public float flashDuration = 0.2f;
    public int flashCount = 3;

    [System.Flags]
    public enum ControlState
    {
        None = 0,
        LT = 1,
        RT = 2,
        LS = 4,
        A = 8,
        B = 16,
        X = 32,
        Y = 64,
    }

    public List<Image> controlImages;

    private void Awake()
    {
        foreach (var img in controlImages)
        {
            img.gameObject.SetActive(false);
        }
    }

    public ControlState currentState = ControlState.None;

    [ContextMenu("Start Flash")]
    public void StartFlash()
    {
        StartCoroutine(FlashControls());
    }

    private IEnumerator FlashControls()
    {
        for (int i = 0; i < flashCount; i++)
        {
            yield return new WaitForSeconds(flashDuration);
            for(int j = 0; j < controlImages.Count; j++)
            {
                if (((ControlState)(1 << j) & currentState) != 0)
                {
                    controlImages[j].gameObject.SetActive(true);
                }
            }
            yield return new WaitForSeconds(flashDuration);
            for (int j = 0; j < controlImages.Count; j++)
            {
                if (((ControlState)(1 << j) & currentState) != 0)
                {
                    controlImages[j].gameObject.SetActive(false);
                }
            }
        }
    }
}
