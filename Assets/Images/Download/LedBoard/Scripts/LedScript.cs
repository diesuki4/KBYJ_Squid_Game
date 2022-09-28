using UnityEngine;

public class LedScript : MonoBehaviour
{
    public LedBoardScript LBS;

    public int IndexX;
    public int IndexY;
    public bool isOne;

    public MeshRenderer meshRend;
    private Material CurrentMatOn;
    private Material CurrentMatOff;

    private void OnEnable()
    {
        LBS = GetComponentInParent<LedBoardScript>();
    }

    /// <summary>
    /// Apply materail for LED
    /// </summary>
    public void ApplyMaterial()
    {
        LBS = GetComponentInParent<LedBoardScript>();
        CurrentMatOn = new Material(LBS.PrefOnMaterial);
        CurrentMatOff = new Material(LBS.PrefOffMaterial);
        CurrentMatOn.color = LBS.LedColor;
        CurrentMatOff.color = LBS.BoardColor;

        if (isOne)
        {
            meshRend.material = CurrentMatOn;
        }
        else
        {
            meshRend.material = CurrentMatOff;
        }
    }

    /// <summary>
    /// Turn off led
    /// </summary>
    public void TurnOff()
    {
        if (isOne)
        {
            CurrentMatOn.color = LBS.LedColor;
            meshRend.material = CurrentMatOn;
            isOne = false;
        }
    }

    /// <summary>
    /// Turn on led
    /// </summary>
    public void TurnOn()
    {
        if (!isOne)
        {
            CurrentMatOff.color = LBS.BoardColor;
            meshRend.material = CurrentMatOff;
            isOne = true;
        }
    }
}
