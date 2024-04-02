using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Color
{
    public class ColorManager : MonoBehaviour
    {
        public static ColorManager instance;
        public List<Material> materialAvaliable;

        void Awake(){
            instance = this;
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                SubmitNewColor();
            }

            GUILayout.EndArea();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        static void SubmitNewColor()
        {
            if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Change Color" : "Request Change Color"))
            {
                if (NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient )
                {
                    foreach (ulong uid in NetworkManager.Singleton.ConnectedClientsIds)
                        NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<ColorPlayer>().Recolor();
                }
                else
                {
                    var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<ColorPlayer>();
                    player.Recolor();
                }
            }
        }
    }

}