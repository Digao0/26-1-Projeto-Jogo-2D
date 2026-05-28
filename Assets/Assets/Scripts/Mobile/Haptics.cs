using UnityEngine;

public static class Haptics
{
    // Vibração curta — feedback de ataque
    public static void Light() => Vibrate(25);

    // Vibração forte — feedback de dano recebido
    public static void Impact() => Vibrate(80);

    static void Vibrate(long milliseconds)
    {
#if UNITY_EDITOR
        return;
#elif UNITY_ANDROID
        try
        {
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            using var activity    = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using var vibrator    = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            vibrator.Call("vibrate", milliseconds);
        }
        catch { }
#elif UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
