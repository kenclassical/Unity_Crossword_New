using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbWod : MonoBehaviour
{
    public GameObject DumbWodPrefab;
    public GameObject DumbWordButton;
    public GameObject Hand;
    public void OnDumb(){
        DumbWodPrefab.SetActive(true);
        DumbWordButton.SetActive(false);
    }

    public void OffDumb(){
        DumbWodPrefab.SetActive(false);
        DumbWordButton.SetActive(true);
    }

}
