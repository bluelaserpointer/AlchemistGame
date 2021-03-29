using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HandCards : MonoBehaviour
{
    public static HandCards instance;

    //inspector
    [SerializeField] public float cardsMargin;

    //data
    private static Vector3 cardMarginXShift;
    private static List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
            instance = this;
        cardMarginXShift = new Vector3(instance.cardsMargin / 2, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void BandLast (GameObject newObject)
    {
        objects.Add(newObject);
        newObject.transform.SetParent(instance.transform);
        PositionUpdate();
    }
    public static void Disband (GameObject removingObject)
    {
        if(objects.Remove(removingObject))
        {
            PositionUpdate();
        }
    }
    public static void PositionUpdate()
    {
        int i = 0, count = objects.Count;
        foreach(GameObject eachObject in objects)
        {
            eachObject.transform.position = instance.transform.position + cardMarginXShift*(i*2 - count);
            ++i;
        }
    }
}
