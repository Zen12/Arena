using System;
using UnityEngine;

namespace UnityView
{
    public class UnitView : MonoBehaviour, IEngineView
    {
        private Transform _tr;
        private Vector3 _start;
        private Vector3 _end;
        private float _lerpSpeed;
        private CommandType _command;

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

        public void ApplyChange(Vector3 start, Vector3 end, float deltaTime, CommandType commandType)
        {
            _command = commandType;
            _start = start;
            _end = end;
            _lerpSpeed = deltaTime;
            _tr.position = _start;
        }

        public void OnActivate()
        {
            gameObject.SetActive(true);
            Init();
        }

        public void OnDeactivate()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_command == CommandType.Idle)
                return;

            if (_command == CommandType.Move)
            {
                var deltaTime = Time.deltaTime;
                _tr.position = Vector3.Lerp(_start, _end, deltaTime * _lerpSpeed);
            }

        }
    }
}