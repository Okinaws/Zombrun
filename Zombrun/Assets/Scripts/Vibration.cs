using MoreMountains.NiceVibrations;

public class Vibration : Singleton<Vibration>
{
    public void Vibrate(long time)
    {
        MMNVAndroid.AndroidVibrate(time);
    }
}
