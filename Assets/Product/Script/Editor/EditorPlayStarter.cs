﻿using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;
using Product.Domain.Model;
using Product.View;
using UnityEngine.SceneManagement;

namespace Product.Editor
{
    /// <summary>
    /// エディタ再生時に環境を整えてくれる君
    /// </summary>
    [InitializeOnLoad]
    public class EditorPlayStarter
    {
#region constants

        static readonly string ApplicationEntryScenePath = EditorBuildSettings.scenes.First().path;

        static readonly Dictionary<string, Action> ReloadActionBySceneName = new Dictionary<string, Action>
        {
            // ここにシーン名ごとのモックデータでのロード処理を書いてください
            {
                "Home",
                () =>
                {
                    // モックデータ
                    HomeSceneEntry.HofeInfo = new HomeInfo("プレイヤー名", "コメント", 99); // 今回は雑にstatic...
                    SceneManager.LoadSceneAsync("Home");
                }
            },
        };

        /// <summary>
        /// アプリの初期化処理やロード処理を行います
        /// </summary>
        static void InitializeApp(Action loadMockDataScene)
        {
            InitData();
            RequestLogin(loadMockDataScene);
        }

        static void InitData()
        {
            // データを初期化したりー
        }

        static void RequestLogin(Action callback)
        {
            // ログイン処理を行ったりー
            callback();
        }

#endregion

#region properties

        static bool pauseMode = false;
        static bool prePlayed = true;

#endregion

#region private methods

        static EditorPlayStarter()
        {
            EditorApplication.playmodeStateChanged += OnPlayModeStageChanged;
            pauseMode = EditorApplication.isPaused;
        }

        static void OnPrePlay()
        {
            string activeScenePath = EditorSceneManager.GetActiveScene().path;
            // 一番最初のシーンの場合はモックロード使わないよー
            if (activeScenePath != ApplicationEntryScenePath)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(activeScenePath);
                if (ReloadActionBySceneName.ContainsKey(sceneName))
                {
                    EditorPrefs.SetBool("MockLoadMode", true);
                }
            }
        }

        static void OnPostPlay()
        {
            string sceneName = EditorSceneManager.GetActiveScene().name;
            if (ReloadActionBySceneName.ContainsKey(sceneName))
            {
                EditorPrefs.SetBool("MockLoadMode", false);
                InitializeApp(ReloadActionBySceneName[sceneName]);
            }
            else if (ApplicationEntryScenePath != EditorSceneManager.GetActiveScene().path)
            {
                Debug.LogWarning("Mockローダーが実装されてないよ！");
            }
        }

        static void OnPlayModeStageChanged()
        {
            if (!pauseMode && EditorApplication.isPaused)
            {
                // ポーズされた場合
                pauseMode = true;
            }
            else if (pauseMode && !EditorApplication.isPaused)
            {
                // ポーズが解除された場合
                pauseMode = false;
            }
            else if (!EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode)
            {
                prePlayed = true;
                // プレイ前処理(プレイボタンが押された瞬間)
                OnPrePlay();
            }
            else if (EditorApplication.isPlaying && EditorApplication.isPlayingOrWillChangePlaymode && prePlayed)
            {
                prePlayed = false;
                // プレイ後処理(MonoBehaviour.Start後)
                OnPostPlay();
            }
            else if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // UnityEditorは再生終了時に自動的にポーズボタンを解除するので
                pauseMode = false;
                // 停止前処理(停止ボタンが押された瞬間)
            }
            else if (!EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                // 停止後処理(完全にプレイモードが止まった時)
                // 今のところ処理はしないけどフェーズとしてあった方が綺麗なので一応置いておく
            }
        }

#endregion
    }
}
