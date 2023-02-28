using System.Collections.Generic;

namespace Tanks.Lobby.ClientControls.API {
    public class CommentedListDataProvider : DefaultListDataProvider {
        readonly List<string> Comments = new();

        public int CommentCount { get; private set; }

        public string GetComment(object data) {
            int index = dataStorage.IndexOf(data);
            return Comments[index];
        }

        public bool HasComment(object data) {
            int index = dataStorage.IndexOf(data);
            return !string.IsNullOrEmpty(Comments[index]);
        }

        public void AddItem(object data, string comment) {
            Comments.Add(comment);

            if (!string.IsNullOrEmpty(comment)) {
                CommentCount++;
            }

            AddItem(data);
        }

        public override void AddItem(object data) {
            Comments.Add(string.Empty);
            base.AddItem(data);
        }

        public override void RemoveItem(object data) {
            int index = dataStorage.IndexOf(data);

            if (!string.IsNullOrEmpty(Comments[index])) {
                CommentCount--;
            }

            Comments.RemoveAt(index);
            base.RemoveItem(data);
        }

        public override void ClearItems() {
            Comments.Clear();
            CommentCount = 0;
            base.ClearItems();
        }
    }
}