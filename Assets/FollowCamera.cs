using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    void LateUpdate()
    {
        transform.position = followTarget.position;
    }
}
