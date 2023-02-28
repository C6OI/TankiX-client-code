using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueCarouselUIComponent : BehaviourComponent {
        [SerializeField] LeagueTitleUIComponent leagueTitlePrefab;

        [SerializeField] RectTransform scrollContent;

        [SerializeField] Button leftScrollButton;

        [SerializeField] Button rightScrollButton;

        [SerializeField] float autoScrollSpeed = 1f;

        [SerializeField] float pageWidth = 400f;

        [SerializeField] float pagesOffset = 20f;

        [SerializeField] int pageCount;

        [SerializeField] int currentPage = 1;

        [SerializeField] bool interactWithScrollView;

        [SerializeField] LocalizedField leagueLocalizedField;

        public CarouselItemSelected itemSelected;

        public LeagueTitleUIComponent CurrentLeague => GetComponentsInChildren<LeagueTitleUIComponent>()[currentPage - 1];

        void Start() {
            leftScrollButton.onClick.AddListener(ScrollLeft);
            rightScrollButton.onClick.AddListener(ScrollRight);
        }

        void Update() {
            if (!interactWithScrollView) {
                pageCount = scrollContent.childCount;
                Vector2 b = new(-(currentPage - 1) * pageWidth - pagesOffset, scrollContent.anchoredPosition.y);
                scrollContent.anchoredPosition = Vector2.Lerp(scrollContent.anchoredPosition, b, autoScrollSpeed * Time.deltaTime);
            }
        }

        void OnDisable() {
            Clear();
        }

        void ScrollRight() {
            SelectPage(Mathf.Min(pageCount, currentPage + 1));
        }

        void ScrollLeft() {
            SelectPage(Mathf.Max(1, currentPage - 1));
        }

        void SelectPage(int page) {
            currentPage = page;
            interactWithScrollView = false;

            if (itemSelected != null) {
                itemSelected(CurrentLeague);
            }

            SetButtons();
        }

        void SetButtons() {
            leftScrollButton.gameObject.SetActive(currentPage != 1);
            rightScrollButton.gameObject.SetActive(currentPage != GetComponentsInChildren<LeagueTitleUIComponent>().Length);
        }

        public LeagueTitleUIComponent AddLeagueItem(Entity entity) => GetNewLeagueTitleLayout(entity);

        LeagueTitleUIComponent GetNewLeagueTitleLayout(Entity entity) {
            LeagueTitleUIComponent leagueTitleUIComponent = Instantiate(leagueTitlePrefab);
            leagueTitleUIComponent.transform.SetParent(scrollContent, false);
            leagueTitleUIComponent.gameObject.SetActive(true);
            string text = entity.GetComponent<LeagueNameComponent>().Name + " " + leagueLocalizedField.Value;
            string spriteUid = entity.GetComponent<LeagueIconComponent>().SpriteUid;
            leagueTitleUIComponent.Name = text;
            leagueTitleUIComponent.Icon = spriteUid;
            return leagueTitleUIComponent;
        }

        public void SelectItem(Entity entity) {
            LeagueTitleUIComponent[] componentsInChildren = GetComponentsInChildren<LeagueTitleUIComponent>();

            for (int i = 0; i < componentsInChildren.Length; i++) {
                if (componentsInChildren[i].LeagueEntity.Equals(entity)) {
                    SelectPage(i + 1);
                    return;
                }
            }

            SelectPage(1);
        }

        void Clear() {
            scrollContent.DestroyChildren();
        }
    }
}