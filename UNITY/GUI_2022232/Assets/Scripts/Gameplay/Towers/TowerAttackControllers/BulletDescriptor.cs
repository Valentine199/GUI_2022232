using System;
using UnityEngine;

namespace TowerDefense.Towers.TowerAttackControllers
{
    public class BulletDescriptor: MonoBehaviour
    {
        private int towerDamage;

        public int TowerDamage
        {
            get { return towerDamage; }
            private set { towerDamage = value; }
        }

        private BulletTypeEnums bulletTypeEnum;

        public BulletTypeEnums BulletTypeEnum
        {
            get { return bulletTypeEnum; }
            private set { bulletTypeEnum = value; }
        }

        public void Init(int towerDamage, BulletTypeEnums bulletTypeEnum)
        {
            this.towerDamage =towerDamage;
            this.bulletTypeEnum =bulletTypeEnum;
        }
    }
}