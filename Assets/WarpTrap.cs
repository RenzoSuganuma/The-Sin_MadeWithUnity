using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpTrap : MonoBehaviour
{
    [SerializeField] Transform _transformTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("WARP!");
                if (this.transform != null)
                {
                    other.gameObject.SetActive(false);
                    other.gameObject.transform.position = this._transformTarget.position;
                    other.gameObject.SetActive(true);
                }
            }
        }
    }
}
