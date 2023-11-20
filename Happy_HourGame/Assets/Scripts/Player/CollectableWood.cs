using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CollectableWood : MonoBehaviourPunCallbacks
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wood")
        {
            PhotonNetwork.Destroy(other.gameObject);
        }
    }
}
