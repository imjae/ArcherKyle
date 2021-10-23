using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �÷��̾ �Ѵ� �߰ݲ�
public class Chaser : Monster
{
    public enum Type
    {
        Archer,
        Swordsman,
        Mage,
        Grunt,
        King
    }

    // ������ �÷��̾� ���� ���� ��ƾ
    protected virtual IEnumerator DetectionRoutine()
    {
        // ���� �ʾ��� ���� �÷��̾� ����
        while (!IsDie)
        {
            if (!IsAttacking)
            {
                OnIdleStatus();
                yield return new WaitForSeconds(DetectionTime);
                OnRunStatus();
                yield return new WaitForSeconds(DetectionIntervalTime);
            }
            yield return null;
        }
    }
}
