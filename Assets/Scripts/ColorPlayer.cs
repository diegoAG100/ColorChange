using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Color
{
    public class ColorPlayer : NetworkBehaviour
    {
        public NetworkVariable<int> AssignedMaterial = new NetworkVariable<int>(0);
        public List<Material> materials;
        private MeshRenderer mesh;
        private int maxPlayers = 6;
        private List<int> asignedIndex;
        private bool firsTime = true;
        private void Start(){
            mesh = GetComponent<MeshRenderer>();
        }
        public override void OnNetworkSpawn()
        {
            if(IsServer){
                if(NetworkManager.Singleton.ConnectedClientsIds.Count>maxPlayers){
                    Invoke("DisconectPlayer",0.01f);
                }
            }
            if (IsOwner)
            {
                Recolor();
            }
        }

        public void Recolor()
        {
            SubmitColorRequestServerRpc();
        }

        [Rpc(SendTo.Server)]
        void SubmitColorRequestServerRpc(RpcParams rpcParams = default)
        {
            Debug.Log("Entrada");
            Debug.Log(ColorManager.instance.materialAvaliable[0]);
            Debug.Log(ColorManager.instance.materialAvaliable[1]);
            Debug.Log(ColorManager.instance.materialAvaliable[2]);
            Debug.Log(ColorManager.instance.materialAvaliable[3]);
            Debug.Log(ColorManager.instance.materialAvaliable[4]);
            Debug.Log(ColorManager.instance.materialAvaliable[5]);
            int newIndex = RandomIndex();
            ColorManager.instance.materialAvaliable[newIndex]=null;
            if(firsTime==true){
                firsTime=false;
            }
            else{
                ColorManager.instance.materialAvaliable[AssignedMaterial.Value]=materials[AssignedMaterial.Value];
            }

            AssignedMaterial.Value = newIndex;
                        Debug.Log("Salida");
            Debug.Log(ColorManager.instance.materialAvaliable[0]);
            Debug.Log(ColorManager.instance.materialAvaliable[1]);
            Debug.Log(ColorManager.instance.materialAvaliable[2]);
            Debug.Log(ColorManager.instance.materialAvaliable[3]);
            Debug.Log(ColorManager.instance.materialAvaliable[4]);
            Debug.Log(ColorManager.instance.materialAvaliable[5]);
        }
        int RandomIndex(){
            int newIndex = Random.Range(0,materials.Count);
            if(ColorManager.instance.materialAvaliable[newIndex]==null){
                return RandomIndex();
            }
            else{
                return newIndex;
            }
        }

        void Update()
        {   
            mesh.material = materials[AssignedMaterial.Value];
        }

        void DisconectPlayer(){
            NetworkManager.DisconnectClient(NetworkManager.Singleton.ConnectedClientsIds.Last());
        }
    }
}