using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Power.Charge
{
    ///<summary>
    ///电荷
    ///</summary>
    public class ElectricCharge : MonoBehaviour
    {
        public ChargeType type;
        [Tooltip("电荷投掷飞行速度")]
        public float flySpeed = 1;
        //电荷飞行终点
        private Vector3 destination;
        private Vector3 gizmoDest;
        [Tooltip("电荷飞行停止距离(电荷半径)")]
        public float stopDistance;
        [Tooltip("射线检测层")]
        public LayerMask layer;
        //射线检测结果
        private RaycastHit2D hit;

        public IEnumerator FLy(Vector3 targetPos)
        {
            CalculateDestination(targetPos);
            while (Vector3.Distance(transform.position, destination) > stopDistance)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.MoveTowards(transform.position, destination, flySpeed * Time.deltaTime);
            }
            SetChargeLayer();
        }

        public void SetChargeLayer()
        {
            this.gameObject.layer = 9;
        }

        private void CalculateDestination(Vector3 targetPos)
        {
            gizmoDest = targetPos;
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, Vector3.Distance(targetPos, transform.position), layer);
            if (hit.collider != null)
            {
                //击中目标
                destination = hit.point;
                print(hit.collider.gameObject.name);
            }
            else
                destination = targetPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawLine(transform.position, transform.position + (gizmoDest - transform.position) * Vector3.Distance(gizmoDest, transform.position));
        }
    }
}
