
public class PlayerHealthComponent : HealthComponent
{
    void Start()
    {
        InterfaceHandler.SetMaxSliderValue(maxHealthPoints);
    }

    void Update()
    {
        InterfaceHandler.SetSliderValue(curHealthPoints);
    }
}
