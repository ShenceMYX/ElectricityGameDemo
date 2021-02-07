using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Power.Charge
{
    ///<summary>
    ///电荷技能管理器
    ///</summary>
    public class CharacterChargeManager : MonoBehaviour
    {
        public GameObject[] charges;

        private GameObject chargeGO;

        [Tooltip("最远投掷电荷距离")]
        public float throwMaxDistance = 2;

        //携带电荷
        public GameObject CarryCharge(Vector3 initialPos)
        {
            //随机选择一种种类的电荷
            GameObject randomCharge = charges[Random.Range(0, charges.Length)];
            //在角色当前位置创建电荷
            chargeGO = GameObjectPool.Instance.CreateObject(randomCharge.GetComponent<ElectricCharge>().type.ToString(), randomCharge, initialPos, Quaternion.identity);
            //Instantiate(randomCharge, releasePos, Quaternion.identity);
            //电荷父物体设为角色
            chargeGO.transform.parent = this.transform;

            return chargeGO;
        }

        //投掷电荷
        public void ThrowCharge(Vector3 destPos)
        {
            //电荷父物体为空 取消和角色绑定状态
            chargeGO.transform.parent = null;
            //电荷向目标位置投掷
            StartCoroutine(chargeGO.GetComponent<ElectricCharge>().FLy(destPos));
            //chargeGO.GetComponent<ElectricCharge>().Fly(destPos);
        }
    }
}
