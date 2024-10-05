//----------------------------------------------
//    Auto Generated. DO NOT edit manually!
//----------------------------------------------

#pragma warning disable 649

using System;
using UnityEngine;
using System.Collections.Generic;

public partial class FileList : ScriptableObject {

	[NonSerialized]
	private int mVersion = 1;

	[SerializeField]
	private Media[] _MediaItems;
	public int GetMediaItems(List<Media> items) {
		int len = _MediaItems.Length;
		for (int i = 0; i < len; i++) {
			items.Add(_MediaItems[i].Init(mVersion, DataGetterObject));
		}
		return len;
	}

	public Media GetMedia(string art_ID) {
		int min = 0;
		int max = _MediaItems.Length;
		while (min < max) {
			int index = (min + max) >> 1;
			Media item = _MediaItems[index];
			if (item.art_ID == art_ID) { return item.Init(mVersion, DataGetterObject); }
			if (string.Compare(art_ID, item.art_ID) < 0) {
				max = index;
			} else {
				min = index + 1;
			}
		}
		return null;
	}

	public void Reset() {
		mVersion++;
	}

	public interface IDataGetter {
		Media GetMedia(string art_ID);
	}

	private class DataGetter : IDataGetter {
		private Func<string, Media> _GetMedia;
		public Media GetMedia(string art_ID) {
			return _GetMedia(art_ID);
		}
		public DataGetter(Func<string, Media> getMedia) {
			_GetMedia = getMedia;
		}
	}

	[NonSerialized]
	private DataGetter mDataGetterObject;
	private DataGetter DataGetterObject {
		get {
			if (mDataGetterObject == null) {
				mDataGetterObject = new DataGetter(GetMedia);
			}
			return mDataGetterObject;
		}
	}
}

[Serializable]
public class Media {

	[SerializeField]
	private string _Art_ID;
	public string art_ID { get { return _Art_ID; } }

	[SerializeField]
	private string _Name;
	public string name { get { return _Name; } }

	[SerializeField]
	private string _Description;
	public string description { get { return _Description; } }

	[SerializeField]
	private string _Notes;
	public string notes { get { return _Notes; } }

	[NonSerialized]
	private int mVersion = 0;
	public Media Init(int version, FileList.IDataGetter getter) {
		if (mVersion == version) { return this; }
		mVersion = version;
		return this;
	}

	public override string ToString() {
		return string.Format("[Media]{{art_ID:{0}, name:{1}, description:{2}, notes:{3}}}",
			art_ID, name, description, notes);
	}

}

