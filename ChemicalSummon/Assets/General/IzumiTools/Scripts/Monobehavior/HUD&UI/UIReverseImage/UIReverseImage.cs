using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class UIReverseImage : MonoBehaviour
{
    [SerializeField]
    bool loadCurrentSpriteAsFront = true;
    public Sprite reverseSprite;
    [Header("Child Objects (Nullable)")]
    public GameObject frontObjectsParent;
    public GameObject reverseObjectsParent;

    //data
    Image image;
    [HideInInspector]
    public Sprite frontSprite;
    public bool IsFront { get; protected set; }
    private void Start()
    {
        image = GetComponent<Image>();
        if (loadCurrentSpriteAsFront)
            frontSprite = image.sprite;
        IsFront = !(270 < transform.eulerAngles.y || transform.eulerAngles.y < 90);
        UpdateSprite();
    }
    public void UpdateSprite()
    {
        if (270 < transform.eulerAngles.y || transform.eulerAngles.y < 90)
        {
            if (!IsFront)
            {
                IsFront = true;
                image.sprite = frontSprite;
                frontObjectsParent?.SetActive(true);
                reverseObjectsParent?.SetActive(false);
            }
        }
        else
        {
            if (IsFront)
            {
                IsFront = false;
                image.sprite = reverseSprite;
                frontObjectsParent?.SetActive(false);
                reverseObjectsParent?.SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateSprite();
    }
}
