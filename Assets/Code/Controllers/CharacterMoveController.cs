using System;
using Code.Assistance;
using Code.Configs;
using Code.Factory;
using Code.UserInput;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEditor;
using UnityEngine;

namespace Code.Controllers
{
    public class CharacterMoveController : MonoBehaviourPun, IPunObservable
    {
        public float Happiness => _happiness;

        [SerializeField] private Transform _happyLineView;
        [SerializeField] private Transform _workInstrument;

        private Rigidbody _rigidbody;
        private InputInitialization _input;
        private PlayerConfig _config;
        private float _happiness = 100;

        private Vector3 _newVelocity;
        private float _horizontal;
        private float _vertical;
        //private bool _isFiring;

        // private void Awake()
        // {
        //     if (_magicalRay == null)
        //     {
        //         Debug.LogError("<Color=Red><b>Missing</b></Color> magical ray Reference.", this);
        //     }
        //     else
        //     {
        //         _magicalRay.gameObject.SetActive(false);
        //     }
        //
        //     //GetUserData();
        // }

        public void Init(InputInitialization input, PlayerConfig config, Rigidbody rb)
        {
            _input = input;
            _config = config;
            _rigidbody = rb;
            _input.InputHorizontal.AxisOnChange += HorizontalOnAxisOnChange;
            _input.InputVertical.AxisOnChange += VerticalOnAxisOnChange;

            StartCamera();
            ShowHappyLine();
        }

        private void HorizontalOnAxisOnChange(float value) => _horizontal = value;
        private void VerticalOnAxisOnChange(float value) => _vertical = value;

        private void StartCamera()
        {
            CameraController cameraController = gameObject.GetComponent<CameraController>();
            if (cameraController != null)
            {
                if (photonView.IsMine)
                {
                    cameraController.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("Missing CameraWork Component on playerPrefab.", this);
            }
        }

        private void ShowHappyLine()
        {
            if (_happyLineView != null)
            {
                Transform _uiGo = Instantiate(_happyLineView);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }
        
        // private void GetUserData()
        // {
        //     PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        //     {
        //         Debug.Log("Got user data:");
        //         if (result.Data == null || !result.Data.ContainsKey("Happiness"))
        //             Debug.Log("No params health");
        //         else
        //         {
        //             _happiness = Convert.ToSingle(result.Data["Happiness"].Value);
        //             Debug.Log("Happiness: " + result.Data["Happiness"].Value);
        //         }
        //     }, error =>
        //     {
        //         Debug.Log("Got error retrieving user data:");
        //         Debug.Log(error.GenerateErrorReport());
        //     });
        // }

        private void FixedUpdate()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
        }

        private void ProcessInputs()
        {
            if(_horizontal!= 0.0f)
            {
                Vector3 relativePos = _newVelocity;
                var angle = Vector3.Angle(Vector3.forward, relativePos);
                var axis = Vector3.Cross(Vector3.forward, relativePos);
                transform.rotation = Quaternion.AngleAxis(angle, axis);
            }
            
            _newVelocity.Set(_horizontal, 0.0f, _vertical);
            _rigidbody.AddForce(_newVelocity * _config.Speed);


            // if (Input.GetButtonDown("Work"))
            // {
            //     if (!_isFiring)
            //     {
            //         _isFiring = true;
            //         _workInstrument.gameObject.SetActive(true);
            //     }
            // }
            //
            // if (Input.GetButtonUp("Work"))
            // {
            //     if (_isFiring)
            //     {
            //         _isFiring = false;
            //         _workInstrument.gameObject.SetActive(false);
            //     }
            // }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            // if (stream.IsWriting)
            // {
            //     stream.SendNext(_isFiring);
            // }
            // else
            // {
            //     _isFiring = (bool) stream.ReceiveNext();
            // }
        }

        public void OnDestroy()
        {
            _input.InputHorizontal.AxisOnChange -= HorizontalOnAxisOnChange;
            _input.InputVertical.AxisOnChange -= VerticalOnAxisOnChange;
        }
    }
}
