using OSGeo.OGR;
using System.Runtime.InteropServices;

namespace TestGDAL
{
    public static class FeatureExtension
    {
        [DllImport("gdal.dll", EntryPoint = "OGR_F_GetFieldAsBinary", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr OGR_F_GetFieldAsBinary(HandleRef handle, int index, out int byteCount);
        [DllImport("gdal.dll", EntryPoint = "OGR_F_GetFieldAsString", CallingConvention = CallingConvention.Cdecl)]
        public extern static IntPtr OGR_F_GetFieldAsString(HandleRef handle, int i);
        [DllImport("gdal.dll", EntryPoint = "CPLStrnlen", CallingConvention = CallingConvention.Cdecl)]
        public extern static uint CPLStrnlen(IntPtr handle, uint nMaxLen);

        public static byte[] GetFieldAsBinary(this Feature feature, int index, FeatureDatastoreType datastoreType)
        {
            if (datastoreType == FeatureDatastoreType.GDB)
            {
                int byteCount = 0;
                IntPtr pIntPtr = OGR_F_GetFieldAsBinary(Feature.getCPtr(feature), index, out byteCount);
                byte[] byteArray = new byte[byteCount];
                Marshal.Copy(pIntPtr, byteArray, 0, byteCount);
                return byteArray;
            }
            else
            {
                IntPtr pchar = OGR_F_GetFieldAsString(Feature.getCPtr(feature), index);
                int length = (int)CPLStrnlen(pchar, uint.MaxValue);
                byte[] byteArray = new byte[length];
                Marshal.Copy(pchar, byteArray, 0, length);
                return byteArray;
            }
        }
        public static byte[] GetFieldAsBinary(this Feature feature, string fieldName, FeatureDatastoreType datastoreType)
        {
            int index = feature.GetFieldIndex(fieldName);
            if (datastoreType == FeatureDatastoreType.GDB)
            {
                int byteCount = 0;
                IntPtr pIntPtr = OGR_F_GetFieldAsBinary(Feature.getCPtr(feature), index, out byteCount);
                byte[] byteArray = new byte[byteCount];
                Marshal.Copy(pIntPtr, byteArray, 0, byteCount);
                return byteArray;
            }
            else
            {
                IntPtr pchar = OGR_F_GetFieldAsString(Feature.getCPtr(feature), index);
                int length = (int)CPLStrnlen(pchar, uint.MaxValue);
                byte[] byteArray = new byte[length];
                Marshal.Copy(pchar, byteArray, 0, length);
                return byteArray;
            }
        }
    }
}
