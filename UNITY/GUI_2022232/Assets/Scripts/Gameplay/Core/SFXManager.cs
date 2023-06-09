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

        ISoundPlayer soundRequester = GetComponent<ISoundPlayer>();
        soundRequester.PlayInitSound += PlayInitSoundServerRpc;
        soundRequester.PlayAmbiance += PlayAmbiance;
        soundRequester.StopAmbiance += StopAllSound;
        soundRequester.PlayEndSound += PlayDeadSound;
    }

    private void OnDisable()
    {
        ISoundPlayer soundRequester = GetComponent<ISoundPlayer>();
        soundRequester.PlayInitSound -= PlayInitSoundServerRpc;
        soundRequester.PlayAmbiance -= PlayAmbiance;
        soundRequester.StopAmbiance -= StopAllSound;
        soundRequester.PlayEndSound -= PlayDeadSound;

        _audioSource.clip = null;
    }

    //caller functions
    [ServerRpc(RequireOwnership = false)]
    private void PlayInitSoundServerRpc()
    {
        if(_initSound == null) { return; }
        PlayInitSoundClientRpc();
    }

    [ClientRpc]
    private void PlayInitSoundClientRpc()
    {
        _multipleSongs = false;
        StartCoroutine(PlaySound(_initSound));

        PlayAmbiance();
    }

    private void PlayAmbiance()
    {
        if(_ambianceSound.Length == 0) { return; }

        if(_ambianceSound.Length > 1)
        {
            _multipleSongs = true;
            PlayManySongs();
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

        //StartFade(0);
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
            PlayManySongs();
        }
    }

    //[ServerRpc(RequireOwnership = false)]
    private void PlayManySongs()
    {
        int r = UnityEngine.Random.Range(0, _ambianceSound.Length);
        AudioClip nextSong = _ambianceSound[r];
        StartCoroutine(PlaySound(nextSong));
        //StartFade(0.07f);
        //SyncedSoundClientRpc(r);
    }

    //[ClientRpc]
    //private void SyncedSoundClientRpc(int i)
    //{
        
    //}

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

    private  void StartFade(float targetVolume)
    {

        float duration = 3.5f;

        float currentTime = 0;
        float start = _audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
        }

        if (targetVolume <= 0)
        {
            _audioSource.Stop();
            StopAllCoroutines();
        }
    }

}

public interface ISoundPlayer
{
    public event Action PlayInitSound;
    public event Action PlayAmbiance;
    public event Action StopAmbiance;
    public event Action PlayEndSound;
}