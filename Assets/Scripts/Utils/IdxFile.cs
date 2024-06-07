using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class BundleItem
{
	public int version;
	public int filesize;
	public string name;
	public string hashCode;
}

public class IdxFile
{
	List<string[]> mArrayData = new List<string[]> ();

	bool mIsFirstRowAsCols = false;

	bool isFirstRowAsCols {
		set { mIsFirstRowAsCols = value; }
		get { return mIsFirstRowAsCols; }
	}

	int rowCount {
		get { 
			if (mIsFirstRowAsCols) {
				if (mArrayData.Count > 0)
					return mArrayData.Count - 1;
				return 0;
			}
			else
				return mArrayData.Count;
		}
	}

	int colCount {
		get { 
			if (mArrayData.Count > 0)
				return mArrayData [0].Length; 
			return 0;
		}
	}

	public List<BundleItem> Load (string fileContent)
	{
		mArrayData.Clear ();

		StringReader sr = new StringReader (fileContent);

		string line;
		while ((line = sr.ReadLine ()) != null) {
			mArrayData.Add (line.Split ('\t'));
		}
		sr.Close ();
		sr.Dispose ();

		List<BundleItem> list = new List<BundleItem> ();
		for (int i = 0; i < rowCount; i++) {
			BundleItem item = new BundleItem ();
			item.name = GetString (i, 0);
			item.version = GetInt (i, 1);
			item.hashCode = GetString (i, 2);
			item.filesize = GetInt (i, 3);
			list.Add (item);
		}

		return list;
	}

    static public int getFileSize(string filename)
    {
        FileInfo file = new FileInfo(filename);
        if (file == null) return 0;
        return (int)file.Length;
    }

	static public string Save (List<BundleItem> list, string path)
	{
		StringBuilder sb = new StringBuilder ();
		foreach (var v in list) {
			sb.Append (v.name);
			sb.Append ('\t');
			sb.Append (v.version);
			sb.Append ('\t');
			sb.Append (v.hashCode);

            sb.Append('\t');
            // get file size
			sb.Append(getFileSize(path + v.name));
            sb.Append("\r\n");
		}
		return sb.ToString ();
	}

	string GetString (int row, int col)
	{
		if (mIsFirstRowAsCols)
			row = row + 1;
		if (row < 0 || row >= mArrayData.Count)
			return null;
		if (col < 0 || col >= mArrayData [row].Length)
			return null;
		return mArrayData [row] [col];
	}

	int GetInt (int row, int col)
	{
		if (mIsFirstRowAsCols)
			row = row + 1;
		if (row < 0 || row >= mArrayData.Count)
			return 0;
		if (col < 0 || col >= mArrayData [row].Length)
			return 0;
		return int.Parse (mArrayData [row] [col]);
	}

	float GetFloat(int row, int col)
	{
		if (mIsFirstRowAsCols)
			row = row + 1;
		if (row < 0 || row >= mArrayData.Count)
			return 0;
		if (col < 0 || col >= mArrayData [row].Length)
			return 0;
		return (mArrayData [row] [col]).ToFloat();
	}

}
