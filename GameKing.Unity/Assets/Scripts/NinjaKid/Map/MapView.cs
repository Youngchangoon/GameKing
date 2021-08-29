using System.Collections.Generic;
using GameKing.Shared.MessagePackObjects;
using GameKing.Unity.NinjaKid.Messages;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameKing.Unity.NinjaKid.Map
{
    public enum MapState
    {
        None,
        SelectPos,
        MovePos,
        AttackPos,
    }

    public class MapView : MonoBehaviour, IInitializable
    {
        [SerializeField] private RectTransform cellRoot;
        [SerializeField] private CellView cellViewPrefab;

        public MapState CurMapState { get; private set; }

        private List<List<CellView>> _cellArray;

        public void Initialize()
        {
            _cellArray = new List<List<CellView>>();
            CurMapState = MapState.None;
        }

        /// <summary>
        /// 맵뷰를 생성한다.
        /// </summary>
        /// <param name="mapModel"></param>
        public void CreateMapView(MapModel mapModel)
        {
            _cellArray.Clear();

            var gridLayout = cellRoot.GetComponent<GridLayoutGroup>();
            gridLayout.constraintCount = mapModel.list.Count;

            for (var y = 0; y < mapModel.list.Count; ++y)
            {
                var newList = new List<CellView>();
                for (var x = 0; x < mapModel.list[y].Count; ++x)
                {
                    var newCell = Instantiate(cellViewPrefab, cellRoot);
                    newCell.Init(new Vector2Int(x, y));

                    newList.Add(newCell);
                }

                _cellArray.Add(newList);
            }
        }

        /// <summary>
        /// 맵 전체를 선택 가능/불가능 하게 만든다.
        /// </summary>
        /// <param name="enable"></param>
        public void SetAllCanSelect(bool enable)
        {
            for (var y = 0; y < _cellArray.Count; ++y)
            {
                for (var x = 0; x < _cellArray[y].Count; ++x)
                {
                    _cellArray[y][x].UpdateCellCanSelect(enable);
                }
            }
        }

        public void SetOpen(int x, int y, bool isOpen)
        {
            _cellArray[y][x].SetOpen(isOpen);
        }

        /// <summary>
        /// basePos를 중심으로 8방 만 선택 가능한 셀로 만든다.
        /// </summary>
        /// <param name="basePos"></param>
        public void SetCanSelectNearByPos(Vector2Int basePos)
        {
            for (var y = 0; y < _cellArray.Count; ++y)
            {
                for (var x = 0; x < _cellArray[y].Count; ++x)
                {
                    var isNear = Mathf.Abs(basePos.x - x) <= 1 && Mathf.Abs(basePos.y - y) <= 1;

                    _cellArray[y][x].UpdateCellCanSelect(isNear);
                }
            }
        }

        public Vector2 GetPosition(int x, int y)
        {
            return _cellArray[y][x].transform.position;
        }

        public void SetCellAlpha(int x, int y, float alpha)
        {
            _cellArray[y][x].SetCellAlpha(alpha);
        }

        public void SetCellAlphaAll(float alpha)
        {
            for (var y = 0; y < _cellArray.Count; ++y)
            {
                for (var x = 0; x < _cellArray[y].Count; ++x)
                {
                    SetCellAlpha(x, y, alpha);
                }
            }
        }
    }
}