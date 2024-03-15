using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CatFactToast : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;

    public void SetUp(CatFactSO data)
    {
        _title.text = data.title;
        _description.text = data.factText;
    }
}
