using UnityEngine;

public class ChartoHard : MonoBehaviour
{
    public GameObject Hand;
    public GameObject HandCharter;

    void Start()
    {
        Hand = GameObject.Find("Hand");
        HandCharter.transform.SetParent(Hand.transform);
        HandCharter.transform.localScale = Vector3.one;
        HandCharter.transform.position = new Vector3(transform.position.x,transform.position.y,-48);
        HandCharter.transform.eulerAngles = new Vector3(25,0,0);
    }
}
