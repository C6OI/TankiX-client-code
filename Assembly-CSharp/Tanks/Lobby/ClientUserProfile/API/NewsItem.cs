using System;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class NewsItem {
        [ProtocolOptional] public DateTime Date { get; set; }

        [ProtocolOptional] public string HeaderText { get; set; }

        [ProtocolOptional] public string ShortText { get; set; }

        [ProtocolOptional] public string LongText { get; set; }

        [ProtocolOptional] public string PreviewImageUrl { get; set; }

        [ProtocolOptional] public string PreviewImageGuid { get; set; }

        [ProtocolOptional] public string CentralIconGuid { get; set; }

        [ProtocolOptional] public string Tooltip { get; set; }

        public bool PreviewImageFitInParent { get; set; }

        [ProtocolOptional] public string ExternalUrl { get; set; }

        [ProtocolOptional] public string InternalUrl { get; set; }

        public NewsItemLayout Layout { get; set; }

        public override string ToString() => string.Format(
            "Date: {0}, HeaderText: {1}, ShortText: {2}, LongText: {3}, PreviewImageUrl: {4}, PreviewImageGuid: {5}, CentralIconGuid: {6}, PreviewImageFitInParent: {7}, ExternalUrl: {8}, InternalUrl: {9}, Layout: {10}",
            Date, HeaderText, ShortText, LongText, PreviewImageUrl, PreviewImageGuid, CentralIconGuid, PreviewImageFitInParent, ExternalUrl, InternalUrl, Layout);
    }
}