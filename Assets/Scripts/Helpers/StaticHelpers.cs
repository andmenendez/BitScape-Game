using UnityEngine;

public static class StaticHelpers {
    public static void SetClipFromResource(this AudioSource origin, string clip) {
        origin.clip = Resources.Load<AudioClip>($"Sounds/{clip}");
        Debug.Log($"play sound audiosource: {clip}, {origin.name}");
    }
}
