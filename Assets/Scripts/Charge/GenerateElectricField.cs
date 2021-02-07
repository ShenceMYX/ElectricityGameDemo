using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Power.Charge
{
    ///<summary>
    ///生成电场
    ///</summary>
    public class GenerateElectricField : MonoBehaviour
    {
        public GameObject electricField;
        private GameObject electricFieldGO;
        [Tooltip("电场素材长度")]
        public float electricFieldLength = 5;
        [Tooltip("电荷半径（电场偏移起点距离）")]
        public float chargeOffset = 0.5f;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Charge")
            {
                ElectricCharge otherElectricCharge = collision.GetComponent<ElectricCharge>();
                //如果电荷种类不同
                if (otherElectricCharge.type != this.GetComponent<ElectricCharge>().type)
                {
                    if (electricFieldGO != null) return;

                    float distance = Vector3.Distance(collision.transform.position, transform.position);
                    //两点电荷中心位置
                    //Vector3 centerPoint = (transform.position + collision.transform.position) / 2;
                    //两点电荷连线与水平x轴夹角
                    Quaternion rotationAngle = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.right, collision.transform.position - transform.position));
                    //创建电场
                    electricFieldGO = GameObjectPool.Instance.CreateObject("EletricField", electricField, transform.position + new Vector3(chargeOffset, 0, 0), rotationAngle);
                    //electricFieldGO = Instantiate(electricField, transform.position + new Vector3(chargeOffset, 0, 0), rotationAngle);
                    //将电场长度变为电荷间距
                    electricFieldGO.transform.localScale = new Vector3(distance / electricFieldLength, electricFieldGO.transform.localScale.y, 0);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            
        }
    }
}
