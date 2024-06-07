public class UIWindowHudBase : UIWindowBase
{
    public static UIType UIType
    {
        get { return UIType.Hud; }
    }

    public override void OnInit()
    {
        base.OnInit();
        isInStack = false;
    }
}