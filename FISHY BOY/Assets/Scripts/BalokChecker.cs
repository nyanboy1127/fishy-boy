using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalokChecker : MonoBehaviour
{
    public GameObject Gerbang, GerbangOpen;

  
    // Start is called before the first frame update
    void Start()
    {
        //animGerbang = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PiringBuahMerah.buah2Matched == true && BuahIjo1.buah1Matched == true && BuahIjo2.buah3Matched == true)
        {
            Destroy(Gerbang);
            GerbangOpen.SetActive(true);
        }
    }
}
