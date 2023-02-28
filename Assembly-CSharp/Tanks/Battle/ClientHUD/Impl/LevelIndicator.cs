using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientProfile.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class LevelIndicator<T> : AnimatedIndicatorWithFinishComponent<T> where T : Component, new() {
        [SerializeField] ColoredProgressBar levelProgress;

        [SerializeField] Text levelValue;

        [SerializeField] Text deltaLevelValue;

        [SerializeField] ExperienceIndicator exp;

        NormalizedAnimatedValue animation;

        long fromExp;

        int initialLevel;

        int level;

        int[] levels;

        long toExp;

        void Awake() {
            animation = GetComponent<NormalizedAnimatedValue>();
        }

        public void Update() {
            float num = animation.value * (toExp - fromExp);
            LevelInfo info = LevelInfo.Get(fromExp + (long)num, levels);

            if (info.Level != level) {
                GetComponent<Animator>().SetTrigger("Up");
                levelValue.text = info.Level.ToString();
                exp.LevelUp();
                levelProgress.ColoredProgress = info.Level / (float)levels.Length;
                level = info.Level;
                deltaLevelValue.gameObject.SetActive(true);
                deltaLevelValue.text = "+" + (info.Level - initialLevel);
            }

            info.ClampExp();
            exp.Change(info);
            TryToSetAnimationFinished(info.AbsolutExperience, toExp);
        }

        public void Init(Entity screenEntity, long fromExp, long toExp, int[] levels) {
            SetEntity(screenEntity);
            LevelInfo info = LevelInfo.Get(fromExp, levels);

            if (info.IsMaxLevel) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);
            level = info.Level;
            this.levels = levels;
            this.fromExp = fromExp;
            this.toExp = toExp;
            initialLevel = info.Level;
            exp.Init(info);
            levelProgress.InitialProgress = info.Level / (float)levels.Length;
            levelProgress.ColoredProgress = levelProgress.InitialProgress;
            levelValue.text = info.Level.ToString();
            GetComponent<Animator>().SetTrigger("Start");
            deltaLevelValue.gameObject.SetActive(false);
        }
    }
}