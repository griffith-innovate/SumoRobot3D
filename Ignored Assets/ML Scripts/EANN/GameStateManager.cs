#region Includes
using UnityEngine;
using UnityEngine.SceneManagement;
#endregion

/*
================================================================================
Singleton class managing the overall simulation.


Original code by Samuel Arzt (March 2017) at:
https://github.com/ArztSamuel/Applying_EANNs
================================================================================
 */


public class GameStateManager : MonoBehaviour {
    #region Members
    // The camera object, to be referenced in Unity Editor.
    // [SerializeField]
    // private CameraMovement Camera;

    // The name of the track to be loaded
    [SerializeField]
    public string TrackName;


    // The UIController object.
    // public UIController UIController { get; set; }

    public static GameStateManager Instance { get; private set; }

    // private CarController prevBest, prevSecondBest;
    #endregion

    #region Constructors
    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Multiple GameStateManagers in the Scene.");
            return;
        }
        Instance = this;

        //Load gui scene
        SceneManager.LoadScene("GUI", LoadSceneMode.Additive);

        //Load track
        SceneManager.LoadScene(TrackName, LoadSceneMode.Additive);
    }

    void Start() {
        // TrackManager.Instance.BestCarChanged += OnBestCarChanged;
        EvolutionManager.Instance.StartEvolution();
    }
    #endregion

    #region Methods
    // Callback method for when the best car has changed.
    // private void OnBestCarChanged(CarController bestCar) {
    //     // Old code from car GA
    //     // if (bestCar == null)
    //     //     Camera.SetTarget(null);
    //     // else
    //     //     Camera.SetTarget(bestCar.gameObject);
    //     //
    //     // if (UIController != null)
    //     //     UIController.SetDisplayTarget(bestCar);
    // }
    #endregion
}
