[System.Serializable]
public class AttackInstance
{
    public AttacksDataSO attackData;
    public float cooldownTimer = 0;

    public AttackInstance ( AttacksDataSO data )
    {
        attackData = data;
        cooldownTimer = 0;
    }
    public void UpdateCooldownTimer (ref float coldownTimer, float deltaTime )
    {
        if (cooldownTimer > attackData.coldownAttackTimerMax)
        {
            cooldownTimer = coldownTimer;
            coldownTimer += deltaTime;
        }
    }

    public bool IsReady ( float coldown )
    {
        return coldown > attackData.coldownAttackTimerMax;
    }
}