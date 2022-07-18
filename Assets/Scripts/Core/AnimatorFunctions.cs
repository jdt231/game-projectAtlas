using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    private PlayerController player;

    [Header("Particles")]
    [SerializeField] private ParticleSystem particleSystemStep;
    [SerializeField] private int emitAmountStep;
    [SerializeField] private ParticleSystem particleSystemPunch;
    [SerializeField] private int emitAmountPunch;
    [SerializeField] private ParticleSystem particleSystemPunchUp;
    [SerializeField] private int emitAmountPunchUp;
    [SerializeField] private ParticleSystem particleSystemKick;
    [SerializeField] private int emitAmountKick;
    [SerializeField] private ParticleSystem particleSystemKickDown;
    [SerializeField] private int emitAmountKickDown;
    [SerializeField] private ParticleSystem particleSystemDeath;
    [SerializeField] private int emitAmountDeath;


    [Header("Sound Bank")]
    [SerializeField] private AudioClip soundPunch;
    [SerializeField] private float soundPunchVolume = 1;
    [SerializeField] private AudioClip soundKick;
    [SerializeField] private float soundKickVolume = 1;
    [SerializeField] private AudioClip soundJump;
    [SerializeField] private float soundJumpVolume = 1;
    [SerializeField] private AudioClip soundHurt;
    [SerializeField] private float soundHurtVolume = 1;
    [SerializeField] private AudioClip [] soundRun; 
    [SerializeField] private float soundRunVolume = 1; 
    [SerializeField] private AudioClip soundGateUnlock;
    [SerializeField] private float soundGateUnlockVolume = 1;
    [SerializeField] private AudioClip soundGateOpening;
    [SerializeField] private float soundGateOpeningVolume = 1;
    [SerializeField] private AudioClip soundGateOpened;
    [SerializeField] private float soundGateOpenedVolume = 1;
    [SerializeField] private AudioClip sound9;
    [SerializeField] private float sound9Volume = 1;
    [SerializeField] private AudioClip sound10;
    [SerializeField] private float sound10Volume = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // SFX playing functions

    void PlaySoundPunch()
    {
        player.sfxAudioSource.PlayOneShot(soundPunch);
    }

    void PlaySoundKick()
    {
        player.sfxAudioSource.PlayOneShot(soundKick);
    }

    void PlaySoundJump()
    {
        player.sfxAudioSource.PlayOneShot(soundJump, soundJumpVolume);
    }

    void PlaySoundHurt()
    {
        player.sfxAudioSource.PlayOneShot(soundHurt, soundHurtVolume);
    }

    void PlaySoundRun()
    {
        player.sfxAudioSource.PlayOneShot(soundRun[Random.Range(0,soundRun.Length)], soundRunVolume);
    }

    void PlaySoundGateUnlock()
    {
        player.sfxAudioSource.PlayOneShot(soundGateUnlock, soundGateUnlockVolume);
    }

    void PlaySoundGateOpening()
    {
        player.sfxAudioSource.PlayOneShot(soundGateOpening, soundGateOpeningVolume);
    }

    void PlaySoundGateOpened()
    {
        player.sfxAudioSource.PlayOneShot(soundGateOpened, soundGateOpenedVolume);
    }

    void PlaySound9()
    {
        player.sfxAudioSource.PlayOneShot(sound9, sound9Volume);
    }

    void PlaySound10()
    {
        player.sfxAudioSource.PlayOneShot(sound10, sound10Volume);
    }

    // Particle emmision functions

    void EmitParticlesStep()
    {
        particleSystemStep.Emit(emitAmountStep);
    }

    void EmitParticlesPunch()
    {
        particleSystemPunch.Emit(emitAmountPunch);
    }

    void EmitParticlesPunchUp()
    {
        particleSystemPunchUp.Emit(emitAmountPunchUp);
    }

    void EmitParticlesKick()
    {
        particleSystemKick.Emit(emitAmountKick);
    }

    void EmitParticlesKickDown()
    {
        particleSystemKickDown.Emit(emitAmountKickDown);
    }

    public void EmitParticlesDeath()
    {
        particleSystemDeath.Emit(emitAmountDeath);
    }
}
