namespace Lobby.ClientControls.API {
    public class ListItemSelectSoundEffectController : UISoundEffectController {
        public override string HandlerName => "ListItemSelect";

        void OnItemSelect(ListItem listItem) => PlaySoundEffect();
    }
}