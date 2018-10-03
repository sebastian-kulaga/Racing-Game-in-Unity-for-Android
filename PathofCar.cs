using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathofCar : MonoBehaviour
{

    public Color line;

    private List<Transform> nodes = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = line;
        //lista nodow
        Transform[] pathTransform = GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
        for (int i = 0; i < nodes.Count; i++)
        {
            Vector3 currentNode = nodes[i].position;
            Vector3 previousNode = Vector3.zero; ;
            if (i > 0) //jezeli node wieksze od 0 czyli 1
            {
                previousNode = nodes[i - 1].position;
            }
            else if (i == 0 && nodes.Count > 1) // jezeli 0 to ostatnia jest poprzednia
            {
                previousNode = nodes[nodes.Count - 1].position;
            }

            Gizmos.DrawLine(previousNode, currentNode);
            Gizmos.DrawWireSphere(currentNode, 0.3f);
        }
    }
}

