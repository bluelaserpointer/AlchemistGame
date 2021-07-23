using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AttackButton : MonoBehaviour
{
    [SerializeField]
    Image arrowImage;
    [SerializeField]
    Button button;
    public Button Button => button;
    public void SetDirection(bool upside)
    {
        arrowImage.transform.localEulerAngles = new Vector3(0, 0, upside ? 90 : -90);
    }
}
