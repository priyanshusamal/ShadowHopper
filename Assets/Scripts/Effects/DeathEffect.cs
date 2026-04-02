using UnityEngine;
public class DeathEffect : MonoBehaviour
{
    private void Awake()
    {
        deatheffect = GetComponentInChildren<ParticleSystem>();
    }
    public ParticleSystem deatheffect;

    public void Play()
    {
        if (deatheffect == null)
            return;

        deatheffect.Play();

    }

}
