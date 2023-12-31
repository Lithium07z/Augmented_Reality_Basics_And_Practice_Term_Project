using UnityEngine;
using Firebase;
public class FirebaseManager : MonoBehaviour
{
    FirebaseApp _app;
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) 
            {
                _app = Firebase.FirebaseApp.DefaultInstance;
            } 
            else 
            {
                UnityEngine.Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }
}
