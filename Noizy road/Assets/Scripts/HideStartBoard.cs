using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideStartBoard : MonoBehaviour
{
    [SerializeField] private GameObject startBoard;

    public void HideBoard()
    {
        startBoard.SetActive(false);
    }
}
