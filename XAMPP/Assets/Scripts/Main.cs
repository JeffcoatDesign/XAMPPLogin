using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main instance;
    public Web Web;

    private void Awake()
    {
        instance = this;
        Web = GetComponent<Web>();
    }
}
