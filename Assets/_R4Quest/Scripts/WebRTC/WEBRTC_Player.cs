using System.Collections;
using System.Collections.Generic;
using Unity.WebRTC;
using UnityEngine;

public class WEBRTC_Player : MonoBehaviour
{

    RTCPeerConnection localConnection, remoteConnection;
    RTCDataChannel sendChannel, receiveChannel;

    private void Awake()
    {
        // Initialize WebRTC
        //WebRTC.Initialize();
        // Create local peer
        localConnection = new RTCPeerConnection();
        sendChannel = localConnection.CreateDataChannel("sendChannel");
        sendChannel.OnOpen = handleSendChannelStatusChange;
        sendChannel.OnClose = handleSendChannelStatusChange;
        // Create remote peer
        remoteConnection = new RTCPeerConnection();
        remoteConnection.OnDataChannel = ReceiveChannelCallback;
        // register comms paths
        localConnection.OnIceCandidate = e =>
        {
            var addIceCandidate = !string.IsNullOrEmpty(e.Candidate)
                                  || remoteConnection.AddIceCandidate(e);
        };
        remoteConnection.OnIceCandidate = e =>
        {
            var addIceCandidate = !string.IsNullOrEmpty(e.Candidate)
                                  || localConnection.AddIceCandidate(e);
        };
        localConnection.OnIceConnectionChange = state =>
        {
            Debug.Log(state);
        };
    }

    private void handleSendChannelStatusChange()
    {
        throw new System.NotImplementedException();
    }

    //handle begin
    IEnumerator Call(){
        var op1 = localConnection.CreateOffer();
        yield return op1;
        var sessionDescription = op1.Desc;
        var op2 = localConnection.SetLocalDescription(ref sessionDescription);
        yield return op2;
        var op1Desc = op1.Desc;
        var op3 = remoteConnection.SetRemoteDescription(ref op1Desc);
        yield return op3;
        var op4 = remoteConnection.CreateAnswer();
        yield return op4;
        var op4Desc = op4.Desc;
        var op5 = remoteConnection.SetLocalDescription(ref op4Desc);
        yield return op5;
        var rtcSessionDescription = op4Desc;
        var op6 = localConnection.SetRemoteDescription(ref rtcSessionDescription);
        yield return op6;
    }

    //handle send messages
    void SendMessage(string message)
    {
        sendChannel.Send(message);
    }

    void SendBinary(byte[] bytes)
    {
        sendChannel.Send(bytes);
    }

    //handle receive messages
    void ReceiveChannelCallback(RTCDataChannel channel) 
    {
        receiveChannel = channel;
        receiveChannel.OnMessage = HandleReceiveMessage;  
    }
    void HandleReceiveMessage(byte[] bytes)
    {
        var message = System.Text.Encoding.UTF8.GetString(bytes);
        Debug.Log(message);
    }

    //handle end
    private void OnDestroy()
    {
        sendChannel.Close();
        receiveChannel.Close();

        localConnection.Close();
        remoteConnection.Close();

        //WebRTC.Finalize();
    }

}