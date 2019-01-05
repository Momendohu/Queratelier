using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Beam : MonoBehaviour {
    ParticleSystem ps;

    // これらのリストは、各フレームでトリガーの条件に
    // 一致するパーティクルを格納します
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

    void OnEnable () {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger () {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter,enter);

        for(int i = 0;i < numEnter;i++) {
            ParticleSystem.Particle p = enter[i];
            p.startLifetime = 0;
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter,enter);
    }
}