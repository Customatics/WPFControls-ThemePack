using System;
using System.Runtime.InteropServices;
using ThemePack.Common.Models.Constants;

namespace ThemePack.Common.Helpers
{
    public static class Utils
    {
        private const string DllName = "LeapTest.Win.Utils.dll";

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private extern static SafeMemoryHandle compress_mem_to_heap(IntPtr pSrc_buf, int src_buf_len, out int pOut_len, int flags);

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        private extern static void free_mem(IntPtr p);

        [DllImport(DllName, SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public extern static LT_STATUS lt_processes_handle_action(ProcessesAction action);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lt_html_tree_create(IntPtr buffer, out IntPtr tree);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lt_html_tree_destroy(IntPtr tree);

        private sealed class SafeMemoryHandle : SafeHandle
        {
            public override bool IsInvalid { get { return handle == IntPtr.Zero; } }

            public SafeMemoryHandle()
                : base(IntPtr.Zero, true)
            { }

            protected override bool ReleaseHandle()
            {
                if (IsInvalid == false)
                {
                    try
                    {
                        free_mem(handle);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public static byte[] Compress(byte[] source)
        {
            var pinnedSource = default(GCHandle);
            byte[] compressed;

            const int flags = 10; // it seems to have the best results with this value

            int compressedLength;

            try
            {
                pinnedSource = GCHandle.Alloc(source, GCHandleType.Pinned);
                var sourcePointer = pinnedSource.AddrOfPinnedObject();

                using (var compressedPointer = compress_mem_to_heap(sourcePointer, source.Length, out compressedLength, flags))
                {
                    compressed = new byte[compressedLength];

                    Marshal.Copy(compressedPointer.DangerousGetHandle(), compressed, 0, compressedLength);
                }
            }
            finally
            {
                if (pinnedSource.IsAllocated)
                {
                    pinnedSource.Free();
                }
            }

            return compressed;
        }

        public enum LT_STATUS
        {
            LT_OK = 0,
            LT_ERROR_NO_MEMORY = -10000,
            LT_ERROR_CANT_COPY_STRING = 10000,
            LT_ERROR_CANT_LOWER_STRING = 10001,
            LT_ERROR_CANT_GET_PROCESSES = 20001,
            LT_ERROR_CANT_OPEN_PROCESS = 20002,
            LT_ERROR_CANT_ENUM_PROCESS_MODULES = 20003,
            LT_ERROR_CANT_GET_PROCESS_NAME = 20004,
            LT_ERROR_UNSUPPORTED_PROCESSES_ACTION = 20005,
            LT_ERROR_CANT_GET_WINDOW_PLACEMENT = 20006,
            LT_ERROR_CANT_ENUM_WINDOWS = 20007,

            LT_ERROR_VIDEO_ENCODER_NOT_FOUND = 30000,
            LT_ERROR_VIDEO_CANNOT_ALLOCATE_STREAM = 30001,
            LT_ERROR_VIDEO_INVALID_CODEC_TYPE = 30002,
            LT_ERROR_VIDEO_CANNOT_ALLOCATE_FRAME = 30003,
            LT_ERROR_VIDEO_CANNOT_OPEN_VIDEO_CODEC = 30004,
            LT_ERROR_VIDEO_CANNOT_CREATE_CONVERSION_CONTEXT = 30005,
            LT_ERROR_VIDEO_NO_CODEC = 30006,
            LT_ERROR_VIDEO_CANNOT_OPEN_FILE = 30007,
            LT_ERROR_VIDEO_CANNOT_WRITE_HEADER = 30008,
            LT_ERROR_VIDEO_CANNOT_MAKE_FRAME_WRITEABLE = 30009,
            LT_ERROR_VIDEO_CANNOT_ALLOCATE_OUTPUT_CONTEXT = 30010,
            LT_ERROR_VIDEO_ENCODING_FRAME_FAIL = 30011,
            LT_ERROR_VIDEO_WRITING_FRAME_FAIL = 30012,
            LT_ERROR_VIDEO_CANNOT_WRITE_TRAILER = 30013,
            LT_ERROR_VIDEO_CANNOT_ALLOCATE_FRAME_BUFFER = 30014,
            LT_ERROR_VIDEO_CANNOT_CLOSE_FILE = 30015,
            LT_ERROR_VIDEO_CANNOT_COPY_IMAGE = 30016,
        }
    }
}
