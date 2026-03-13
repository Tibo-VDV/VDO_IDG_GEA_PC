using UnityEngine;

public class ToggleCameraMode : MonoBehaviour
{
    enum CameraMode { firstPerson, thirdPerson }
    [SerializeField] CameraMode currentCameraMode = CameraMode.firstPerson;
    [SerializeField] GameObject[] firstPersonObjects; //Enable alle objecten die bij de fps camera horen
    [SerializeField] GameObject[] thirdPersonObjects;
    PlayerController getPlayerController => GetComponent<PlayerController>();

    void OnToggleCamera()
    {
        switch (currentCameraMode)
        {
            case CameraMode.firstPerson:
                print("Switching to third person");
                currentCameraMode = CameraMode.thirdPerson;
                SwitchCameraMode(true);
                getPlayerController.SetMoveMode(PlayerController.MoveMode.thirdPersonMove);
                break;
            case CameraMode.thirdPerson:
                print("Switching to first person");
                currentCameraMode = CameraMode.firstPerson;
                SwitchCameraMode(false);
                getPlayerController.SetMoveMode(PlayerController.MoveMode.firstPersonMove);
                break;
        }
    }
    // Door een bool parameter te gebruiken kunnen we zorgen dat we maar 1 functie nodig hebben.
    // we bekijken het van het perspectief van onze fps objects of deze aan of uit gezet moeten worden.
    //en onze third person objects worden dan automatisch het tegenovergestelde van onze first person objects door de bool te inverteren met !.

    void SwitchCameraMode(bool cameraBool)
    {
        foreach (GameObject obj in thirdPersonObjects)
        {
            obj.SetActive(cameraBool);
        }
        foreach (GameObject obj in firstPersonObjects)
        {
            obj.SetActive(!cameraBool);
        }
    }

    //unoptimized versie van hierboven
    /*
    void SwitchToThirdPerson()
    {
        foreach (GameObject obj in thirdPersonObjects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in firstPersonObjects)
        {
            obj.SetActive(false);
        }
    }

    void SwitchToFirstPerson()
    {
        foreach (GameObject obj in firstPersonObjects)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in thirdPersonObjects)
        {
            obj.SetActive(false);
        }
    }
    */

    void Start()
    {
        
    }
}
