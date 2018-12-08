using Product.Domain.Model;
using UnityEngine;

namespace Product.View
{
    public class HomeSceneEntry : BaseEntry
    {
        /// <summary> ホーム情報 </summary>
        public static HomeInfo HofeInfo { get; set; }

        void Awake()
        {
            Debug.Log("ホーム情報を取得: " + HofeInfo.PlayerName);
        }

        void Start()
        {
            Debug.Log("HomeInfoを元に各Viewの初期化処理開始");
        }
    }
}