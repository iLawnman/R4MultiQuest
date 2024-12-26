using UnityEngine;
using UnityEngine.Networking;

public class LoggingDownloadHandler : DownloadHandlerScript {

    public LoggingDownloadHandler(): base() {
    }

    public LoggingDownloadHandler(byte[] buffer): base(buffer) {
    }

    protected override byte[] GetData() { return null; }

    protected override bool ReceiveData(byte[] data, int dataLength) {
        if(data == null || data.Length < 1) {
            Debug.Log("LoggingDownloadHandler :: ReceiveData - received a null/empty buffer");
            return false;
        }

        Debug.Log(string.Format("LoggingDownloadHandler :: ReceiveData - received {0} bytes", dataLength));
        return true;
    }

    protected override void CompleteContent() {
        Debug.Log("LoggingDownloadHandler :: CompleteContent - DOWNLOAD COMPLETE!");
    }

    protected override void ReceiveContentLengthHeader(ulong contentLength) {
        Debug.Log(string.Format("LoggingDownloadHandler :: ReceiveContentLength - length {0}", contentLength));
    }
}