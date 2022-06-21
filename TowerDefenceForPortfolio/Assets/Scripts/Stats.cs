using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField] private float startHeartCount;
    [HideInInspector] public static float heartCount;
    [SerializeField] private float startCoinCount;
    [HideInInspector] public static float coinCount;

    private Main main;

    private void Awake()
    {
        main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
    }

    // Start is called before the first frame update
    void Start()
    {
        heartCount = startHeartCount;
        main.UpdateHearts();

        coinCount = startCoinCount;
        main.UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
