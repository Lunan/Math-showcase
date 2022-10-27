using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxControl : MonoBehaviour
{
    public GameObject paper;

    public int boxNumber;
    public int paperNumber;

    public void OpenBox()
    {
        paper.SetActive(true);
    }

    public void CloseBox()
    {
        paper.SetActive(false);
    }

    public void SetBoxNumber(int number)
    {
        GetComponentInChildren<TMP_Text>().text = number.ToString();
        boxNumber = number;
    }

    public void SetPaperNumber(int number)
    {
        transform.Find("Paper").GetComponentInChildren<TMP_Text>().text = number.ToString();
        paperNumber = number;
    }
    
}
