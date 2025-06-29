using System;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Music
{
    public class MusicManager : SingletonPersistentMono<MusicManager>
    {
        [Header("背景音乐片段，序号对应")]
        public AudioClip[] backgroundClips;
        [Header("鼠标释放食物音效")]
        public AudioClip mouseClick;
        [Header("怪物吃掉食物音效")]
        public AudioClip monsterEat;
        private AudioSource _backgroundMusic;

        private void Start()
        {
            _backgroundMusic = GetComponent<AudioSource>();
        }
        // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        // {
        //     Debug.Log($"场景已切换: {scene.name}");
        //     _backgroundMusic.clip = backgroundClips[scene.buildIndex];
        // }
        // void OnEnable()
        // {
        //     // 订阅场景加载事件
        //     SceneManager.sceneLoaded += OnSceneLoaded;
        // }
        //
        // void OnDisable()
        // {
        //     // 取消订阅（避免内存泄漏）
        //     SceneManager.sceneLoaded -= OnSceneLoaded;
        // }

        public void PlayBackgroundMusic(int index)
        {
            _backgroundMusic.clip = backgroundClips[index];
            _backgroundMusic.Play();
        }
        public void MouseClick()
        {
            _backgroundMusic.PlayOneShot(mouseClick);
        }

        public void MonsterEat()
        {
            _backgroundMusic.PlayOneShot(monsterEat);
        }
    }
}
