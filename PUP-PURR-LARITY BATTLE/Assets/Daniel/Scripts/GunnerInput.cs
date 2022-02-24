using UnityEngine;

public class GunnerInput : MonoBehaviour
{
    [SerializeField] private Dog dog;
    [SerializeField] private CanvasGroup monitorCanvas;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        dog.ExitGunnerMode();
        monitorCanvas.alpha = 0;
        gameObject.SetActive(false);
    }
}