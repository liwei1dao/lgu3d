/***************************************************
 * 文件名： ShareContent.cs
 * 时  间： 2018-05-22 12:15:14
 * 作  者： 
 * 描  述： 
 ***************************************************/
using UnityEngine; 
using System;
using System.Collections;


namespace lgu3d
{
	/// <summary>
	/// Content type.
	/// </summary>
	public class ShareContent
	{	      		
		Hashtable shareParams = new Hashtable();
		Hashtable customizeShareParams = new Hashtable();

		/*iOS/Android*/
		public void SetTitle(String title) {
			shareParams["title"] = title;
		}

		/*iOS/Android*/
		public void SetText(String text) {
			shareParams["text"] = text;
		}

		/*iOS/Android*/ //url仅在微信（包括好友和朋友圈）中使用
		public void SetUrl(String url) {
			shareParams["url"] = url;
		}

        public void SetRoomId(String roomId)
        {
            shareParams["roomId"] = roomId;
        }

        public void SetRoomToken(String roomToken)
        {
            shareParams["roomToken"] = roomToken;
        }

        /*iOS/Android - 本地图片路径*/
        public void SetImagePath(String imagePath) {
			#if UNITY_ANDROID
			shareParams["imagePath"] = imagePath;
			#elif UNITY_IPHONE
			shareParams["imageUrl"] = imagePath;
			#endif
		}

		/*iOS/Android - 网络图片路径*/
		public void SetImageUrl(String imageUrl) {
			shareParams["imageUrl"] = imageUrl;
		}

		/*iOS/Android - 分享类型*/
		public void SetShareType(int shareType) {
			#if UNITY_ANDROID
			if (shareType == 0) {
				shareType = 1;
			}
			#endif
			shareParams["shareType"] = shareType;
		}

		/*Android Only*/ //是标题的网络链接，仅在人人网和QQ空间使用
		public void SetTitleUrl(String titleUrl) {
			shareParams["titleUrl"] = titleUrl;
		}

		/*iOS/Android*/  //仅在人人网和QQ空间使用
		public void SetComment(String comment) {
			shareParams["comment"] = comment;
		}

		/*Android Only*///仅在QQ空间使用
		public void SetSite(String site) {
			shareParams["site"] = site;
		}

		/*Android Only*/// 仅在QQ空间使用
		public void SetSiteUrl(String siteUrl) {
			shareParams["siteUrl"] = siteUrl;
		}

		/*Android Only*/
		public void SetAddress(String address) {
			shareParams["address"] = address;
		}

		/*iOS/Android*/
		public void SetFilePath(String filePath) {
			shareParams["filePath"] = filePath;
		}

		/*iOS/Android*/
		public void SetMusicUrl(String musicUrl) {
			shareParams["musicUrl"] = musicUrl;
		}

		/*iOS/Android - Sina/Tencent/Twitter/VKontakte*/
		public void SetLatitude(String latitude) {
			shareParams["latitude"] = latitude;
		}

		/*iOS/Android - Sina/Tencent/Twitter/VKontakte*/
		public void SetLongitude(String longitude) {
			shareParams["longitude"] = longitude;
		}
		
		/*iOS/Android - YouDaoNote*/
		public void SetSource(String source){
			#if UNITY_ANDROID
			shareParams["url"] = source;
			#elif UNITY_IPHONE
			shareParams ["source"] = source;
			#endif
		}
		
		/*iOS/Android - YouDaoNote*/
		public void SetAuthor(String author){
			#if UNITY_ANDROID
			shareParams["address"] = author;
			#elif UNITY_IPHONE
			shareParams ["author"] = author;
			#endif
		}
		
		/*iOS/Android - Flickr*/
		public void SetSafetyLevel(int safetyLevel){
			shareParams ["safetyLevel"] = safetyLevel;
		}
		
		/*iOS/Android - Flickr*/
		public void SetContentType(int contentType){
			shareParams ["contentType"] = contentType;
		}
		
		/*iOS/Android - Flickr*/
		public void SetHidden(int hidden){
			shareParams ["hidden"] = hidden;
		}
		
		/*iOS/Android - Flickr*/
		public void SetIsPublic(bool isPublic){
			shareParams ["isPublic"] = isPublic;
		}
		
		/*iOS/Android - Flickr*/
		public void SetIsFriend(bool isFriend){
			shareParams ["isFriend"] = isFriend;
		}
		
		/*iOS/Android - Flickr*/
		public void SetIsFamily(bool isFamily){
			shareParams ["isFamily"] = isFamily;
		}
		
		/*iOS/Android - VKontakte*/
		public void SetFriendsOnly(bool friendsOnly){
			#if UNITY_ANDROID
			shareParams["isFriend"] = friendsOnly;
			#elif UNITY_IPHONE
			shareParams ["friendsOnly"] = friendsOnly;
			#endif
		}
		
		/*iOS/Android - VKontakte*/
		public void SetGroupID(String groupID){
			shareParams ["groupID"] = groupID;
		}
		
		/*iOS/Android - WhatsApp*/
		public void SetAudioPath(String audioPath){
			#if UNITY_ANDROID
			shareParams["filePath"] = audioPath;
			#elif UNITY_IPHONE
			shareParams ["audioPath"] = audioPath;
			#endif
		}
		
		/*iOS/Android - WhatsApp*/
		public void SetVideoPath(String videoPath){
			#if UNITY_ANDROID
			shareParams["filePath"] = videoPath;
			#elif UNITY_IPHONE
			shareParams ["videoPath"] = videoPath;
			#endif
		}
		
		/*iOS/Android - YouDaoNote/YinXiang/Evernote*/
		public void SetNotebook(String notebook){
			shareParams ["notebook"] = notebook;
		}
		
		/*iOS/Android - Pocket/Flickr/YinXiang/Evernote*/
		public void SetTags(String tags){
			shareParams ["tags"] = tags;
		}

		/*iOS Only - Sina*/
		public void SetObjectID(String objectId) {
			shareParams["objectID"] = objectId;
		}

		/*iOS Only - Renren*/
		public void SetAlbumID(String albumId) {
			shareParams["AlbumID"] = albumId;
		}

		/*iOS Only - Wechat*/
		public void SetEmotionPath(String emotionPath){
			shareParams["emotionPath"] = emotionPath;
		}

		/*iOS Only - Wechat/Yixin*/
		public void SetExtInfoPath(String extInfoPath){
			shareParams["extInfoPath"] = extInfoPath;
		}

		/*iOS Only - Wechat*/ 
		public void SetSourceFileExtension(String sourceFileExtension){
			shareParams["sourceFileExtension"] = sourceFileExtension;
		}

		/*iOS Only - Wechat*/
		public void SetSourceFilePath(String sourceFilePath){
			shareParams["sourceFilePath"] = sourceFilePath;
		}

		/*iOS Only - QQ/Wechat/Yixin*/
		public void SetThumbImageUrl(String thumbImageUrl){
			shareParams["thumbImageUrl"] = thumbImageUrl;
		}

		public String GetShareParamsStr() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			Debug.Log("ParseShareParams  ===>>> " + jsonStr );
			return jsonStr;
		}

		public Hashtable GetShareParams() {
			if (customizeShareParams.Count > 0) {
				shareParams["customizeShareParams"] = customizeShareParams;
			}
			String jsonStr = MiniJSON.jsonEncode (shareParams);
			Debug.Log("ParseShareParams  ===>>> " + jsonStr );
			return shareParams;
		}

        public void SetShareParams(Hashtable shareParams, Hashtable customizeShareParams=null)
        {
            if(shareParams!=null)
            {
                this.shareParams = shareParams;
            }

            if (customizeShareParams != null)
            {
                this.customizeShareParams = customizeShareParams;
            }
        }
    }

}


