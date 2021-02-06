using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Power.Character
{
    ///<summary>
    ///角色马达
    ///</summary>
    public class CharacterMotor : MonoBehaviour
    {
        [Tooltip("移动速度")]
        public float moveSpeed = 10;

        public void MoveToTarget(Vector2 direction)
        {
            this.transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
}
