using System.Collections;
using System.Collections.Generic;
using Common;
using Power.Charge;
using UnityEngine;

namespace Power.Character
{
    ///<summary>
    ///输入控制器
    ///</summary>
    public class CharacterInputController : MonoBehaviour
    {
        private CharacterMotor motor;
        private CharacterChargeManager manager;
        private float moveX, moveY;

        private float carryStartTimer;
        private float throwStartTimer;
        [Tooltip("可以扔电荷的开始时间(携带电荷过xx秒后可以投掷电荷)")]
        public float throwAvaibleStartTime = 1;
        [Tooltip("能量蓄力最大时间")]
        public float throwMaxTime = 1;

        private Transform energyTF;
        private float maxEnergyScale;

        private float colliderRadius;
        private float randomThrowAngle;

        private void Start()
        {
            motor = GetComponent<CharacterMotor>();
            manager = GetComponent<CharacterChargeManager>();
            //在子物体中找到黑色能量条
            energyTF = transform.FindChildByName("black");
            //记录黑色能量条最长距离并初始化黑色能量条为0
            InitEnergyScale();
            //获取角色碰撞器半径
            colliderRadius = GetComponent<CircleCollider2D>().radius;
        }

        private void InitEnergyScale()
        {
            //记录黑色能量条最长距离
            maxEnergyScale = energyTF.localScale.x;
            //初始化黑色能量条为0
            energyTF.localScale = new Vector3(0, energyTF.localScale.y, 0);
        }

        private void Update()
        {
            //角色移动控制检测
            CharacterMovementControl();
            //角色携带/投掷电荷控制检测
            CharacterChargeControl();
        }

        private GameObject chargeGO;
        private void CharacterChargeControl()
        {
            //空格键按下时
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //产生随机角度 电荷随机初始化位置
                randomThrowAngle = Random.Range(0, 360);
                Vector3 initPos = transform.position + new Vector3(Mathf.Cos(randomThrowAngle), Mathf.Sin(randomThrowAngle), 0) * colliderRadius;
                //生成电荷并携带
                chargeGO = manager.CarryCharge(initPos);
            }

            //空格键按住时
            if (Input.GetKey(KeyCode.Space))
            {
                carryStartTimer += Time.deltaTime;
                //如果按住超过可以投掷时间 则开始能量条蓄力
                if (carryStartTimer > throwAvaibleStartTime)
                {
                    //能量蓄力时间小于最大蓄力时间时 则增加蓄力开始时间并增加蓄力黑色能量条长度
                    if (throwStartTimer < throwMaxTime)
                    {
                        if (!Input.GetKeyUp(KeyCode.Space))
                        {
                            throwStartTimer += Time.deltaTime;
                            EnergyIncreaseCharging();
                        }
                    }
                    else//蓄力达到最大蓄力时间-》蓄力时间不能再增加 永远等于最大蓄力时间
                        throwStartTimer = throwMaxTime;
                }

            }

            //空格键抬起时
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //携带电荷计时器归零
                carryStartTimer = 0;
                //根据蓄力时间/最大蓄力时间 计算投掷距离
                float throwDistance = throwStartTimer / throwMaxTime * manager.throwMaxDistance;
                //重置蓄力开始时间
                throwStartTimer = 0;
                //产生随机角度投掷随机角度
                Vector3 destination = chargeGO.transform.position + new Vector3(Mathf.Cos(randomThrowAngle), Mathf.Sin(randomThrowAngle), 0) * throwDistance;
                manager.ThrowCharge(destination);
                //蓄力黑色能量条归零
                energyTF.localScale = new Vector3(0, energyTF.localScale.y, 0);
            }
        }

        private void CharacterMovementControl()
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");

            motor.MoveToTarget(new Vector2(moveX, moveY));
        }

        //能量条蓄力长度增加
        private void EnergyIncreaseCharging()
        {
            energyTF.localScale = new Vector3(throwStartTimer / throwMaxTime * maxEnergyScale, energyTF.localScale.y, 0);
        }
    }
}
