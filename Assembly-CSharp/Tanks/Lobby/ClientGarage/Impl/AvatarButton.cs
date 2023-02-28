using System;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class AvatarButton : MonoBehaviour {
        const string equipedFrameName = "equiped";

        const string selectedFrameName = "selected";

        [SerializeField] Button button;

        [SerializeField] ImageSkin icon;

        [SerializeField] ImageListSkin frame;

        [SerializeField] GameObject selectedFrame;

        [SerializeField] GameObject equipedFrame;

        [SerializeField] GameObject lockImage;

        readonly float delta = 0.2f;

        public Func<int> GetIndex = () => 0;

        bool isUserItem;

        public Action OnDoubleClick = delegate { };

        public Action OnPress = delegate { };

        float time;

        void Awake() {
            button.onClick.AddListener(OnPressButton);
        }

        void OnPressButton() {
            OnPress();

            if (Time.realtimeSinceStartup - time < delta) {
                OnDoubleClick();
                time = 0f;
            } else {
                time = Time.realtimeSinceStartup;
            }
        }

        public void Init(string iconUid, string rarity, IAvatarStateChanger changer) {
            icon.SpriteUid = iconUid;
            frame.SelectSprite(rarity);
            changer.SetEquipped = SetEquipped;
            changer.SetSelected = SetSelected;
            changer.SetUnlocked = SetUnlocked;
            changer.OnBought = SetAsBought;
            lockImage.SetActive(false);
            Color white = Color.white;
            white.a = 0.1f;
            icon.GetComponent<Image>().color = white;
            frame.GetComponent<Image>().color = white;
        }

        public void SetSelected(bool selected) {
            selectedFrame.SetActive(selected);
        }

        public void SetEquipped(bool equipped) {
            equipedFrame.SetActive(equipped);
        }

        public void SetUnlocked(bool unlocked) {
            lockImage.SetActive(!unlocked);
        }

        public void SetAsBought() {
            isUserItem = true;
            Color white = Color.white;
            icon.GetComponent<Image>().color = white;
            frame.GetComponent<Image>().color = white;
        }
    }
}