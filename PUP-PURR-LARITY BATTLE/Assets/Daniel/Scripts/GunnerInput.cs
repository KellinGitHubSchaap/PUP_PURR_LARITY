using UnityEngine;

public class GunnerInput : MonoBehaviour
{
    [SerializeField] private Dog dog;
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        dog.ExitGunnerMode();
        gameObject.SetActive(false);
    }
}