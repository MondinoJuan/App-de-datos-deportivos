using Foundation;
using UIKit;

namespace Frontend;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

[Register("SceneDelegate")]
public class SceneDelegate : UIResponder, IUIWindowSceneDelegate
{
    [Export("window")]
    public UIWindow? Window { get; set; }

    [Export("scene:willConnectToSession:options:")]
    public void WillConnect(UIScene scene, UISceneSession session, UISceneConnectionOptions connectionOptions)
    {
        if (scene is UIWindowScene windowScene)
        {
            Window = new UIWindow(windowScene);
            Window.RootViewController = new UIViewController();
            Window.MakeKeyAndVisible();
        }
    }

    [Export("sceneSupportedInterfaceOrientations:")]
    public UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIScene scene)
    {
        return UIInterfaceOrientationMask.Landscape;
    }
}
