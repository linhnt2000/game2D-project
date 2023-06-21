using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    private static AudioHelper instance;

    [SerializeField]
    private PlaylistController playlistController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static AudioHelper Instance
    {
        get
        {
            return instance;
        }
    }

    public void CorrectAudioSettings()
    {
        bool? currentMusicMuted = PersistentAudioSettings.MusicMuted;
        if (currentMusicMuted != null && currentMusicMuted.Value)
        {
            PersistentAudioSettings.MusicMuted = currentMusicMuted.Value;

        }
        bool? currentSoundMuted = PersistentAudioSettings.MixerMuted;
        if (currentSoundMuted != null && currentSoundMuted.Value)
        {
            PersistentAudioSettings.MixerMuted = currentSoundMuted.Value;
        }
    }

    public void MutePlaylist()
    {
        if (PersistentAudioSettings.MusicMuted == null || !PersistentAudioSettings.MusicMuted.Value)
        {
            playlistController.MutePlaylist();
        }
    }

    public void UnmutePlaylist()
    {
        if (PersistentAudioSettings.MusicMuted == null || !PersistentAudioSettings.MusicMuted.Value)
        {
            playlistController.UnmutePlaylist();
        }
    }

    public void MuteSound()
    {
        if (PersistentAudioSettings.MixerMuted == null || !PersistentAudioSettings.MixerMuted.Value)
        {
            MasterAudio.MixerMuted = true;
        }
    }

    public void UnmuteSound()
    {
        if (PersistentAudioSettings.MixerMuted == null || !PersistentAudioSettings.MixerMuted.Value)
        {
            MasterAudio.MixerMuted = false;
        }
    }
}
