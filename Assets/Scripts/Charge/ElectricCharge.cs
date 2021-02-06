using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Power.Charge
{
    ///<summary>
    ///电荷
    ///</summary>
    public class ElectricCharge : MonoBehaviour, IResetable
    {
        public ChargeType type;
        [Tooltip("电荷投掷飞行速度")]
        public float flySpeed = 1;

        public IEnumerator FLy(Vector3 targetPos)
        {
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                yield return new WaitForEndOfFrame();
                transform.position = Vector3.MoveTowards(transform.position, targetPos, flySpeed * Time.deltaTime);
            }
            //transform.Translate(targetPos);
        }

        public void OnReset()
        {
            transform.rotation = Quaternion.identity;
            //electricFieldGO.transform.rotation = Quaternion.identity;
        }
    }
}
