using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class BossFightUI : MonoBehaviour
{
    public Player_Golden player;
    public Slider bossHealthBar;
    public Slider playerHealthBar;
    [SerializeField] private Button jumpBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button attackBtn;
    [SerializeField] private EventTrigger spinBtn;
    [SerializeField] private EventTrigger leftMoveBtn;
    [SerializeField] private EventTrigger rightMoveBtn;

    private void OnEnable()
    {
        jumpBtn.onClick.AddListener(JumpEvent);
        attackBtn.onClick.AddListener(AttackEvent);
        skillBtn.onClick.AddListener(SkillEvent);
        SpinEvent();
        LeftEvent();
        RightEvent();
    }
    private void OnDisable()
    {
        jumpBtn.onClick.RemoveAllListeners();
        attackBtn.onClick.RemoveAllListeners();
        skillBtn.onClick.RemoveAllListeners();
    }

    #region Health
    public void SetBossMaxHealth(int health)
    {
        bossHealthBar.maxValue = health;
        bossHealthBar.value = health;
    } 
    public void SetBossHealth(int health)
    {
        bossHealthBar.value = health;
    }
    public void SetPlayerMaxHealth(int health)
    {
        playerHealthBar.maxValue = health;
        playerHealthBar.value = health;
    }

    public void SetPlayerHealth(int health)
    {
        playerHealthBar.value = health;
    }
    #endregion

    #region JumpEvent
    void JumpEvent()
    {
        player.Jump();
    }
    #endregion

    #region AttackEvent
    void AttackEvent()
    {
        player.ComboAttack();
    }
    #endregion

    #region SpinEvent
    void SpinEvent()
    {
        Entry entrySpin = new Entry();
        Entry exitSpin = new Entry();

        entrySpin.eventID = EventTriggerType.PointerDown;
        exitSpin.eventID = EventTriggerType.PointerExit;

        entrySpin.callback.AddListener((data) => { EntrySpin((PointerEventData)data); });
        exitSpin.callback.AddListener((data) => { ExitSpin((PointerEventData)data); });

        spinBtn.triggers.Add(entrySpin);
        spinBtn.triggers.Add(exitSpin);
    }
    void EntrySpin(PointerEventData data)
    {
        player.IsSpinning(true);
    }
    void ExitSpin(PointerEventData data)
    {
        player.IsSpinning(false);
    }
    #endregion

    #region SkillEvent
    void SkillEvent()
    {
        player.IsSkill(true);
    }
    #endregion

    #region Left
    void LeftEvent()
    {
        Entry entry = new Entry();
        Entry exit = new Entry();

        entry.eventID = EventTriggerType.PointerDown;
        exit.eventID = EventTriggerType.PointerExit;

        entry.callback.AddListener((data) => { EntryLeftMove((PointerEventData)data); });
        exit.callback.AddListener((data) => { ExitLeftMove((PointerEventData)data); });

        leftMoveBtn.triggers.Add(entry);
        leftMoveBtn.triggers.Add(exit);
    }
    void EntryLeftMove(PointerEventData data)
    {
        player.SetHorizontal(-1);
    }
    void ExitLeftMove(PointerEventData data)
    {
        player.SetHorizontal(0);
    }
    #endregion

    #region Right
    void RightEvent()
    {
        Entry entry = new Entry();
        Entry exit = new Entry();

        entry.eventID = EventTriggerType.PointerDown;
        exit.eventID = EventTriggerType.PointerExit;

        entry.callback.AddListener((data) => { EntryRightMove((PointerEventData)data); });
        exit.callback.AddListener((data) => { ExitRightMove((PointerEventData)data); });

        rightMoveBtn.triggers.Add(entry);
        rightMoveBtn.triggers.Add(exit);
    }
    void EntryRightMove(PointerEventData data)
    {
        player.SetHorizontal(1);
    }
    void ExitRightMove(PointerEventData data)
    {
        player.SetHorizontal(0);
    }
    #endregion

}
