using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class InGameUI : MonoBehaviour
{
    public Player player;

    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text timesText;
    [HideInInspector] public string currentTime;
    private float secondsCount;
    private int minuteCount;
    
    [SerializeField] private Button jumpBtn;
    [SerializeField] private EventTrigger sprintBtn;
    [SerializeField] private EventTrigger spinBtn;
    [SerializeField] private EventTrigger leftMoveBtn;
    [SerializeField] private EventTrigger rightMoveBtn;
    [SerializeField] private EventTrigger upMoveBtn;
    [SerializeField] private EventTrigger downMoveBtn;

    private void OnEnable()
    {
        secondsCount = 0;
        minuteCount = 0;
        jumpBtn.onClick.AddListener(JumpEvent);
        SprintEvent();
        SpinEvent();
        LeftEvent();
        RightEvent();
        UpEvent();
        DownEvent();
    }
    private void OnDisable()
    {
        jumpBtn.onClick.RemoveAllListeners();
    }
    private void Update()
    {
        UpdateTimerUI();
        coinsText.SetText(player.coin.ToString());
        livesText.SetText(player.lives.ToString());
        timesText.SetText(currentTime);
    }
    public void UpdateTimerUI()
    {
        //set timer UI
        secondsCount += Time.deltaTime;
        currentTime = minuteCount + ":" + (int)secondsCount;
        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= 60)
        {
            minuteCount = 0;
        }
    }
    #region JumpEvent
    void JumpEvent()
    {
        player.Jump();
    }
    #endregion

    #region SprintEvent
    void SprintEvent()
    {
        Entry entrySprint = new Entry();
        Entry exitSprint = new Entry();

        entrySprint.eventID = EventTriggerType.PointerDown;
        exitSprint.eventID = EventTriggerType.PointerExit;

        entrySprint.callback.AddListener((data) => { EntrySprint((PointerEventData)data); });
        exitSprint.callback.AddListener((data) => { ExitSprint((PointerEventData)data); });

        sprintBtn.triggers.Add(entrySprint);
        sprintBtn.triggers.Add(exitSprint);
    }
    void EntrySprint(PointerEventData data)
    {
        player.Sprinting(true);
    }
    void ExitSprint(PointerEventData data)
    {
        player.Sprinting(false);
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
        player.Spinning(true);
    }
    void ExitSpin(PointerEventData data)
    {
        player.Spinning(false);
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

    #region Up
    void UpEvent()
    {
        Entry entry = new Entry();
        Entry exit = new Entry();

        entry.eventID = EventTriggerType.PointerDown;
        exit.eventID = EventTriggerType.PointerExit;

        entry.callback.AddListener((data) => { EntryUp((PointerEventData)data); });
        exit.callback.AddListener((data) => { ExitUp((PointerEventData)data); });

        upMoveBtn.triggers.Add(entry);
        upMoveBtn.triggers.Add(exit);
    }
    void EntryUp(PointerEventData data)
    {
        player.SetVertical(1);
    }
    void ExitUp(PointerEventData data)
    {
        player.SetVertical(0);
    }
    #endregion

    #region Down
    void DownEvent()
    {
        Entry entry = new Entry();
        Entry exit = new Entry();

        entry.eventID = EventTriggerType.PointerDown;
        exit.eventID = EventTriggerType.PointerExit;

        entry.callback.AddListener((data) => { EntryDown((PointerEventData)data); });
        exit.callback.AddListener((data) => { ExitDown((PointerEventData)data); });

        downMoveBtn.triggers.Add(entry);
        downMoveBtn.triggers.Add(exit);
    }
    void EntryDown(PointerEventData data)
    {
        player.SetVertical(-1);
    }
    void ExitDown(PointerEventData data)
    {
        player.SetVertical(0);
    }
    #endregion
}
