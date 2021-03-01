using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;

        /// <summary>
        /// We are using LateUpdate in order to solve the gap between player
        /// movement and camera movement
        /// </summary>
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
