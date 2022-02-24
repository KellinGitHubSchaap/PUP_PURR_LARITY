using UnityEngine;
using UnityEngine.UI;

public enum MonitorType {CatDog, BirdFish, Oil, Oxygen}
public class Monitor : MonoBehaviour
{
    public MonitorType type;
    [SerializeField] private float holdOnBlink;
    [SerializeField] private Sprite[] blinkSprites;

    private Sprite _blinkSprite;
    private Image _image;
    private Sprite _defaultSprite;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultSprite = _image.sprite;
    }

    public void BlinkRepeating(int index = 1)
    {
        index = Mathf.Clamp(index, 0, blinkSprites.Length - 1);
        
        _blinkSprite = blinkSprites[index];
        InvokeRepeating(nameof(Blink), 0, holdOnBlink * 2);
    }

    public void BlinkOnce(int index = 1)
    {
        index = Mathf.Clamp(index, 0, blinkSprites.Length - 1);
        
        _blinkSprite = blinkSprites[index];
        Blink();
    }
    
    private void Blink()
    {
        _image.sprite = _blinkSprite;
        Invoke(nameof(RevertToDefault), holdOnBlink);
    }

    public void CancelBlink()
    {
        CancelInvoke(nameof(Blink));
    }

    public void BlueScreen()
    {
        CancelInvoke(nameof(Blink));
        _image.sprite = blinkSprites[0];
    }
    
    public void RevertToDefault()
    {
        _image.sprite = _defaultSprite;
    }
}
