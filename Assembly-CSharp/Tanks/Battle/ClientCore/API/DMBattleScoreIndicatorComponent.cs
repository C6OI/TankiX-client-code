using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.API {
    public class DMBattleScoreIndicatorComponent : MonoBehaviour, Component {
        const string VISIBLE_ANIMATION_PROP = "Visible";

        const string INITIALLY_VISIBLE_ANIMATION_PROP = "InitiallyVisible";

        const string BLINK_ANIMATION_PROP = "Blink";

        const string NO_ANIMATION_PROP = "NoAnimation";

        [SerializeField] Text scoreText;

        [SerializeField] Text scoreLimitText;

        [SerializeField] Animator iconAnimator;

        [SerializeField] bool normallyVisible;

        [SerializeField] bool noAnimation;

        bool limitVisible;

        ProgressBar progressBar;

        int score;

        int scoreLimit;

        public int Score {
            get => score;
            set {
                score = value;
                scoreText.text = value.ToString();
            }
        }

        public int ScoreLimit {
            get => scoreLimit;
            set {
                scoreLimit = value;
                scoreLimitText.text = value.ToString();
            }
        }

        public float ProgressValue {
            get => ProgressBar().ProgressValue;
            set => ProgressBar().ProgressValue = value;
        }

        public bool LimitVisible {
            get => limitVisible;
            set {
                limitVisible = value;
                scoreLimitText.GetComponent<Animator>().SetBool("Visible", value);
            }
        }

        public void Awake() {
            Score = 0;
            ScoreLimit = 0;
        }

        public void OnEnable() {
            propagateAnimationParam("Visible", normallyVisible);
            propagateAnimationParam("InitiallyVisible", normallyVisible);
            propagateAnimationParam("NoAnimation", noAnimation);
        }

        public void BlinkIcon() {
            iconAnimator.SetTrigger("Blink");
        }

        void propagateAnimationParam(string paramName, bool paramValue) {
            scoreLimitText.GetComponent<Animator>().SetBool(paramName, paramValue);
        }

        ProgressBar ProgressBar() {
            if (progressBar == null) {
                progressBar = GetComponent<ProgressBar>();
            }

            return progressBar;
        }
    }
}