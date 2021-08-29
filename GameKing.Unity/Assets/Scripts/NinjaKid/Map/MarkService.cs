using GameKing.Shared.MessagePackObjects;
using UnityEngine;
using Zenject;

namespace GameKing.Unity.NinjaKid.Map
{
    public class MarkService : IInitializable
    {
        [Inject(Id = "Mark0")] private MarkView _playerMark0;
        [Inject(Id = "Mark1")] private MarkView _playerMark1;

        [Inject] private MapService _mapService;
        [Inject] private MapView _mapView;

        private MarkModel[] _markModels;
        private MarkView[] _markViews;

        public void Initialize()
        {
            _markViews = new[] { _playerMark0, _playerMark1 };
            _markModels = new MarkModel[2];

            for (var i = 0; i < _markViews.Length; ++i)
                _markViews[i].Init(i, _mapView.transform.Find("MarkRoot"));
        }

        /// <summary>
        /// 말의 위치를 설정한다.
        /// </summary>
        /// <param name="markModel"></param>
        /// <param name="playerIndex"></param>
        /// <param name="isMe"></param>
        public void SetMarkPos(MarkModel markModel, int playerIndex, bool isMe)
        {
            var mapPosition = _mapService.GetMapPosition(markModel.x, markModel.y);

            _markViews[playerIndex].SetPosition(mapPosition);
            _markModels[playerIndex] = markModel;

            if (isMe)
            {
                _mapService.SetCelAlphaAll(1);
                _mapService.SetCellAlpha(markModel.x, markModel.y, 0.5f);
            }
        }

        /// <summary>
        /// 말의 포지션을 반환한다.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public Vector2Int GetMarkPos(int playerIndex)
        {
            var curModel = _markModels[playerIndex];

            return new Vector2Int(curModel.x, curModel.y);
        }

        public void DamagePos(int damage, int x, int y)
        {
            for (var i = 0; i < _markModels.Length; ++i)
            {
                var curModel = _markModels[i];
                if (curModel.x != x || curModel.y != y)
                    continue;

                curModel.hp -= damage;
                _markViews[i].SetHp(curModel.hp, 100);
            }
        }
    }
}