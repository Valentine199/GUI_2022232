using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : NetworkBehaviour
{
    [SerializeField] private AudioClip _initSound;
    [SerializeField] private AudioClip[] _ambianceSound;
    [SerializeField] private AudioClip _endSound;
    private AudioSource _audioSource;
    private bool _multipleSongs = false;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();

        SoundPlayer soundRequester = GetComponent<SoundPlayer>();
        soundRequester.PlayInitSound += PlayInitSound;
        soundRequester.PlayAmbiance += PlayAmbiance;
        soundRequester.StopAmbiance += StopAllSound;
        soundRequester.PlayEndSound += PlayDeadSound;
    }

    private void OnDisable()
    {
        SoundPlayer soundRequester = GetComponent<SoundPlayer>();
        soundRequester.PlayInitSound -= PlayInitSound;
        soundRequester.PlayAmbiance -= PlayAmbiance;
        soundRequester.StopAmbiance -= StopAllSound;
        soundRequester.PlayEndSound -= PlayDeadSound;
    }

    //caller functions
    private void PlayInitSound()
    {
        if(_initSound == null) { return; }
        StartCoroutine(PlaySound(_initSound));

        _multipleSongs = false;
        PlayAmbiance();
    }

    private void PlayAmbiance()
    {
        if(_ambianceSound.Length == 0) { return; }

        if(_ambianceSound.Length > 1)
        {
            _multipleSongs = true;
            SyncedSoundServerRpc();
            //StartCoroutine(PlayMultipleSounds());
        }
        else
        {
            AudioClip clip = _ambianceSound[0];
            StartCoroutine(PlaySoundOnLoop(clip));
        }
    }

    private void StopAllSound()
    {
        _multipleSongs = false;
        if(!_audioSource.isPlaying) { return; }
        StopAllCoroutines();
        _audioSource.Stop();
    }

    private void PlayDeadSound()
    {
        if(_audioSource.isPlaying) { StopAllSound(); }
        _multipleSongs = false;
        if (_endSound == null) { return; }
        StartCoroutine(PlaySound(_endSound));
    }

    // IEnumartors for the playing
    private IEnumerator PlaySound(AudioClip audio)
    {
        _audioSource.clip = audio;  
        _audioSource.loop = false;
        _audioSource.Play();
        yield return new WaitForSeconds(_audioSource.clip.length);

        if(_multipleSongs)
        {
            SyncedSoundServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SyncedSoundServerRpc()
    {
        int r = UnityEngine.Random.Range(0, _ambianceSound.Length);

        SyncedSoundClientRpc(r);
    }

    [ClientRpc]
    private void SyncedSoundClientRpc(int i)
    {
        AudioClip nextSong = _ambianceSound[i];
        StartCoroutine(PlaySound(nextSong));
    }

    private IEnumerator PlaySoundOnLoop(AudioClip audio)
    {
        if (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(_audioSource.clip.length - _audioSource.time);
        }

        _audioSource.clip = audio;
        _audioSource.loop = true;
        _audioSource.Play();
        yield return null;
    }

}

public interface SoundPlayer
{
    public event Action PlayInitSound;
    public event Action PlayAmbiance;
    public event Action StopAmbiance;
    public event Action PlayEndSound;
}