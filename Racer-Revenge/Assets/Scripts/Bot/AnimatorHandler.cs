using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    [SerializeField] Animator animator;
    //[SerializeField] private WeaponIK weaponIK;

    public void ResetAnimator()
    {
        animator.Rebind();
        animator.Play("Idle", -1, 0f);
    }
    public void SetWeaponHandles(Transform leftHand, Transform rightHand)
    {
        //weaponIK.SetHandles(leftHand, rightHand);
    }
    public void EnableWeaponHandles(bool enable)
    {
        //weaponIK.EnableIKHandles(enable);
    }
    public void SetLayerWeight(int index, float weight)
    {
        animator.SetLayerWeight(index, weight);
    }
    public void SetAnimatorTrigger(AnimatorTrigger trigger)
    {
       // Debug.Log($"SetAnimatorTrigger: {trigger} - {(int)trigger}");
        animator.SetInteger(AnimationParameters.TriggerNumber, (int)trigger);
        animator.SetTrigger(AnimationParameters.Trigger);
    }

    void SetActionTrigger( AnimatorTrigger trigger, int actionNumber)
    {
       // Debug.Log($"SetActionTrigger: {trigger} - {(int)trigger} - action {actionNumber}");
        animator.SetInteger(AnimationParameters.Action, actionNumber);
        SetAnimatorTrigger(trigger);
    }
    void SetAttackTrigger(AnimatorTrigger trigger, int attackNumber)
    {
        // Debug.Log($"SetAttackTrigger: {trigger} - {(int)trigger} - action {actionNumber}");
        animator.SetInteger(AnimationParameters.Attack, attackNumber);
        SetAnimatorTrigger(trigger);
    }
    void SetWeaponTrigger(AnimatorTrigger trigger, int weaponNumber)
    {
        // Debug.Log($"SetActionTrigger: {trigger} - {(int)trigger} - action {actionNumber}");
        animator.SetInteger(AnimationParameters.Weapon, weaponNumber);
        SetAnimatorTrigger(trigger);
    }

    public void TriggerInteraction(InteractionType interactionType)
    { 
        SetActionTrigger(AnimatorTrigger.InteractionTrigger, (int)interactionType); 
    }
    public void TriggerAttack(AttackType attackType)
    {
        SetActionTrigger(AnimatorTrigger.AttackTrigger, (int)attackType);
    }
    public void TriggerMeleeAttack(MeleeAttackType attackType)
    {
        SetAttackTrigger(AnimatorTrigger.AttackTrigger, (int)attackType);
    }
    public void TriggerZombieAttack(ZombieAttackType attackType)
    {
        SetAttackTrigger(AnimatorTrigger.ZombieTrigger, (int)attackType);
    }
    public void TriggerDeath(DeathType deathType)
    {
        SetActionTrigger(AnimatorTrigger.DeathTrigger, (int)deathType);
    }
    public void TriggerEmotion(EmotionType emotionType)
    {
        SetActionTrigger(AnimatorTrigger.EmotionTrigger, (int)emotionType);
    }

    public void TriggerWeapon(WeaponType weaponType)
    {
        SetWeaponTrigger(AnimatorTrigger.WeaponTrigger, (int)weaponType);
    }


    public void SetAnimatorInteger(int id, int value)
    {
        animator.SetInteger(id, value);
    }
    public void SetAnimatorBool(int id, bool value)
    {
        animator.SetBool(id, value);
    }
    public void SetAnimatorFloat(int id, float value)
    {
        animator.SetFloat(id, value);
    }
}
public class AnimationParameters
{
    public static readonly int Transport = Animator.StringToHash("Transport");
    public static readonly int TransportNumber = Animator.StringToHash("TransportNumber");
    
    public static readonly int TriggerNumber = Animator.StringToHash("TriggerNumber");
    public static readonly int Trigger = Animator.StringToHash("Trigger");
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Moving = Animator.StringToHash("Moving");
    public static readonly int Attacking = Animator.StringToHash("Attacking");
    public static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    public static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
    public static readonly int Action = Animator.StringToHash("Action");
    public static readonly int Interaction = Animator.StringToHash("Interaction");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Death = Animator.StringToHash("Death");
    public static readonly int Emotion = Animator.StringToHash("Emotion");
    public static readonly int Weapon = Animator.StringToHash("Weapon");
    public static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public static readonly int Vertical = Animator.StringToHash("Vertical");
    
    public static readonly int TimerAttack = Animator.StringToHash("TimerAttack");
}
public enum AnimatorTrigger
{
    NoTrigger = 0,
    IdleTrigger = 1,
    InteractionTrigger = 2,
    AttackTrigger = 3,
    DeathTrigger = 4,
    EmotionTrigger = 5,
    WeaponTrigger = 6,
    ZombieTrigger = 7,
    InstantSwitchTrigger = 99
}
public enum InteractionType
{
    None = 0,
    Mining = 1,
    Working1 = 2,
    Working2 = 3,
    Working3 = 4,
    PhoneCall = 5
}
public enum DeathType
{
    None = 0,
    Death1 = 1,
    Death2 = 2
}
public enum AttackType
{
    None = 0,
    Throw = 1,
    Shooting = 2,
    Hit = 3
}
public enum ZombieAttackType
{
    None = 0,
    Attack1 = 1,
    Attack2 = 2
}
public enum MeleeAttackType
{
    None = 0,
    Attack1 = 1,
    Attack2 = 2,
    Attack3 = 3,
    Attack4 = 4,
    Attack5 = 5,
}
public enum WeaponType
{
    None = 0,
    Pistol = 1,
    Riffle = 2,
    Shotgun = 3,
    Axe = 4,
    Zombie = 5

}
public enum EmotionType
{
    None = 0,
    Dance1 = 1,
    Dance2 = 2,
    Dance3 = 3,
    Angry1 = 4,
    Angry2 = 5,
    Angry3 = 6,
    Neutral1 = 7,
    Neutral2 = 8,
    Neutral3 = 9,
    Happy1 = 10,
    Happy2 = 11,
    Happy3 = 12
}