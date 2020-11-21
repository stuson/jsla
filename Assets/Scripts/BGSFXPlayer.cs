using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSFXPlayer : MonoBehaviour {
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform);
    }
}
