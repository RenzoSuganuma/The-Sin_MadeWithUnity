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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(this.gameObject.transform.position, Vector3.one * 5);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(this._transformTarget.gameObject.transform.position, Vector3.one * 5);
    }
}
