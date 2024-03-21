using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Color
{
    public class ColorPlayer : NetworkBehaviour
    {
        public NetworkVariable<int> AssignedMaterial = new NetworkVariable<int>(0);
        public List<Material> materials;
        private MeshRenderer mesh;
        private void Start(){
            mesh = GetComponent<MeshRenderer>();
        }
        public override void OnNetworkSpawn()
        {
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
            int newIndex = RandomIndex();
            AssignedMaterial.Value = newIndex;
        }
        int RandomIndex(){
            int newIndex = Random.Range(0,materials.Count);
            if(newIndex==AssignedMaterial.Value){
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
    }
}