// using Unity.Netcode;
// using UnityEngine;
// using UnityEngine.UI;

// public class GameNetworkUI : MonoBehaviour
// {
//     [SerializeField] private Button host;
//     [SerializeField] private Button join;

//     private void OnEnable()
//     {
//         host.onClick.AddListener(StartHost);
//         join.onClick.AddListener(StartClient);    
//     }

//     private void OnDisable()
//     {
//         host.onClick.RemoveListener(StartHost);
//         join.onClick.RemoveListener(StartClient);
//     }

//     public void StartHost()
//     {
//         NetworkManager.Singleton.StartHost();
//     }

//     public void StartClient()
//     {
//         NetworkManager.Singleton.StartClient();
//     }
// }
