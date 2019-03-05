using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    bool isCheck = false;
    bool isValide = false;

    public bool CanCheck()
    {
        if (!isCheck && !isValide)
            return true;
        return false;
    }

    public void CheckCard()
    {
        isCheck = true;
        transform.rotation = Quaternion.Euler(Vector3.up * 180);
    }

    public void ValideCard()
    {
        isValide = true;
        isCheck = false;
        transform.rotation = Quaternion.Euler(Vector3.up * 180);
    }

    public void ResetCard()
    {
        isCheck = false;
        isValide = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
