using System;
using System.Collections;
using UnityEngine;

namespace UnityView
{
    public class UnitView : MonoBehaviour, IEngineView
    {
        public uint Id;
        public ITimeline Timeline;
        private Transform _tr;
        private Vector3 _start;
        private Vector3 _end;
        private CommandType _command;
        private float _startTime;
        private float _endTime;

        private Coroutine _lastRoutine;

        private void Awake()
        {
            _tr = transform;
            Init();
        }

        private void Init()
        {
            _end = _tr.position;
            _start = _end;
            _command = CommandType.Idle;
        }

        public void ApplyChange(Vector3 start, Vector3 end, float startTime, float endTime, CommandType commandType)
        {
            _command = commandType;
            _start = start;
            _end = end;
            _startTime = startTime;
            _endTime = endTime;
            _tr.position = _start;

            if (_lastRoutine != null && gameObject.activeSelf)
            {
                StopCoroutine(_lastRoutine);
            }
            
            _tr.localScale = Vector3.one;

            if (commandType == CommandType.Attack && gameObject.activeSelf)
            {
                _lastRoutine = StartCoroutine(ScaleRoutine());
            }
            
            if (commandType == CommandType.Die)
            {
                OnDeactivate();
            }
        }

        public void OnActivate()
        {
            gameObject.SetActive(true);
            Init();
        }

        public void OnDeactivate()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_command == CommandType.Idle)
                return;

            if (_command == CommandType.Move)
            {
                var lerp = Mathf.InverseLerp(_startTime, _endTime, Timeline.CurrentTime);
                _tr.position = Vector3.Lerp(_start, _end, lerp);
            }

        }

        private IEnumerator ScaleRoutine()
        {
            var delta = 0f;

            while (delta < 1f)
            {
                _tr.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, delta);
                delta += Time.deltaTime;
            }

            delta = 0f;
            
            while (delta < 1f)
            {
                _tr.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2f, 1 - delta);
                delta += Time.deltaTime;
                yield return null;
            }
        }
    }
}