using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace DataSakura.Runtime.Utilities.ARImageTargetRuntimeLibrary
{
    public class ARImageTargetLibrary
    {
        [SerializeField]
        ARTrackedImageManager m_TrackedImageManager;

        private RuntimeReferenceImageLibrary runtimeLibrary;

        void AddImage(Texture2D imageToAdd)
        {
            if (!(ARSession.state == ARSessionState.SessionInitializing || ARSession.state == ARSessionState.SessionTracking))
                return; // Session state is invalid

            if (runtimeLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                mutableLibrary.ScheduleAddImageWithValidationJob(
                    imageToAdd,
                    "my new image",
                    0.2f /* 50 cm */);
            }
        }
        public void CreateImageLibrary()
        {
            runtimeLibrary = m_TrackedImageManager.CreateRuntimeLibrary();
        }
        
        struct DeallocateJob : IJob
        {
            [DeallocateOnJobCompletion]
            public NativeArray<byte> data;

            public void Execute() { }
        }

        void AddImage(NativeArray<byte> grayscaleImageBytes,
            int widthInPixels, int heightInPixels,
            float widthInMeters)
        {
            if (!(ARSession.state == ARSessionState.SessionInitializing || ARSession.state == ARSessionState.SessionTracking))
                return; // Session state is invalid

            if (runtimeLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
            {
                var aspectRatio = (float)widthInPixels / (float)heightInPixels;
                var sizeInMeters = new Vector2(widthInMeters, widthInMeters * aspectRatio);
                var referenceImage = new XRReferenceImage(
                    // Guid is assigned after image is added
                    SerializableGuid.empty,
                    // No texture associated with this reference image
                    SerializableGuid.empty,
                    sizeInMeters, "My Image", null);

                var jobState = mutableLibrary.ScheduleAddImageWithValidationJob(
                    grayscaleImageBytes,
                    new Vector2Int(widthInPixels, heightInPixels),
                    TextureFormat.R8,
                    referenceImage);

                // Schedule a job that deallocates the image bytes after the image
                // is added to the reference image library.
                new DeallocateJob { data = grayscaleImageBytes }.Schedule(jobState.jobHandle);
            }
            else
            {
                // Cannot add the image, so dispose its memory.
                grayscaleImageBytes.Dispose();
            }
        }
    }
}