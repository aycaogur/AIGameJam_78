using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{
    public GameObject player1, player2;
    public CameraManager cameraManager;

    int whichPlayerIsEnabled = 1;

    private void Start()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(false);
        cameraManager.targetTransform = player1.transform;
    }

    public void SwitchPlayer()
    {
        if (!Input.GetKey(KeyCode.Space)) 
        {
            switch (whichPlayerIsEnabled)
            {
                case 1:
                    whichPlayerIsEnabled = 2;
                    player2.transform.position = player1.transform.position;
                    player1.GetComponent<InputManager>().enabled = false; 
                    player2.GetComponent<InputManager>().enabled = true;  
                    player1.gameObject.SetActive(false);
                    player2.gameObject.SetActive(true);
                    cameraManager.targetTransform = player2.transform;
                    break;

                case 2:
                    whichPlayerIsEnabled = 1;
                    player1.transform.position = player2.transform.position;
                    player1.GetComponent<InputManager>().enabled = true;  
                    player2.GetComponent<InputManager>().enabled = false; 
                    player1.gameObject.SetActive(true);
                    player2.gameObject.SetActive(false);
                    cameraManager.targetTransform = player1.transform;
                    break;
            }
        }
    }
}

