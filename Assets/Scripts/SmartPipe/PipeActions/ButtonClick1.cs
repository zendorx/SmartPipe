
public class ButtonClick1
{
    public string Id;

    public static void Emmit(string id)
    {
        var action = new ButtonClick1();
        action.Id = id;
        OLD_SmartPipe.EmmitAction(action);
    }
}