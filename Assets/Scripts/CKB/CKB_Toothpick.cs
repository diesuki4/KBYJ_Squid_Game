using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CKB_Toothpick : MonoBehaviour
{
    Image toothpickImage;

    void Start()
    {
        toothpickImage = GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (toothpickImage.enabled)
                toothpickImage.enabled = false;
            else
                toothpickImage.enabled = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (toothpickImage.enabled)
                toothpickImage.enabled = false;
            else
                toothpickImage.enabled = true;
        }
    }
}
