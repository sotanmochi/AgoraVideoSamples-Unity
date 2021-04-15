﻿using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace AgoraExtension.Samples
{
    public class ClientPresenter : MonoBehaviour
    {
        [SerializeField] ConnectionView _ConnectionView;
        [SerializeField] AgoraClientContext _Context;

        void Awake()
        {
            _ConnectionView.OnTriggeredJoinEventAsObservable()
            .Subscribe(channelId => UniTask.Void(async () =>  
            {
                await _Context.Join(channelId);
            }))
            .AddTo(this);

            _ConnectionView.OnTriggeredLeaveEventAsObservable()
            .Subscribe(_ => 
            {
                _Context.Leave();
            })
            .AddTo(this);

            _Context.OnJoinedAsObservable()
            .Subscribe(userId => 
            {
                _ConnectionView.SetUserId(userId.ToString());
            })
            .AddTo(this);

            _Context.OnLeftAsObservable()
            .Subscribe(userId => 
            {
                _ConnectionView.SetUserId("00000000");
            })
            .AddTo(this);
        }
    }
}
