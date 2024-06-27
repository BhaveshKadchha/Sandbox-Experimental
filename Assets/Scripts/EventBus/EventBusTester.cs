using UnityEngine;

public class EventBusTester : MonoBehaviour
{
    EventBinding<TestEvent> testEventBinding;
    EventBinding<PlayerEvent> playerEventBinding;

    private void OnEnable()
    {
        testEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
        EventBus<TestEvent>.Register(testEventBinding);

        playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
        EventBus<PlayerEvent>.Register(playerEventBinding);
    }

    private void OnDisable()
    {
        EventBus<TestEvent>.Deregister(testEventBinding);
        EventBus<PlayerEvent>.Deregister(playerEventBinding);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            EventBus<TestEvent>.Raise(new TestEvent());

        if(Input.GetKeyDown(KeyCode.S))
            EventBus<PlayerEvent>.Raise(new PlayerEvent { 
                health = 100,
                mana = 100
            });;
    }


    void HandleTestEvent()
    {
        Debug.LogError("Test Event Recieved");
    }

    void HandlePlayerEvent(PlayerEvent playerEvent) =>
        Debug.Log($"Player event received! Health: {playerEvent.health}, Mana: {playerEvent.mana}");
}
