using System;
using Declarations;
using Declarations.Media;
using Declarations.MediaLibrary;
using Implementation.Media;
using LibVlcWrapper;

namespace Implementation.MediaLibrary
{
    internal class MediaLibraryImpl : DisposableBase, IReferenceCount, INativePointer, IMediaLibrary
    {
        private IntPtr _mHMediaLib = IntPtr.Zero;

        public MediaLibraryImpl(IntPtr mediaLib)
        {
            _mHMediaLib = LibVlcMethods.libvlc_media_library_new(mediaLib);
        }

        protected override void Dispose(bool disposing)
        {
            Release();
        }

        public void Load()
        {
            var result = LibVlcMethods.libvlc_media_library_load(_mHMediaLib);
        }

        public IMediaList MediaList
        {
            get
            {
                return new MediaList(LibVlcMethods.libvlc_media_library_media_list(_mHMediaLib));
            }
        }

        public void AddRef()
        {
            LibVlcMethods.libvlc_media_library_retain(_mHMediaLib);
        }

        public void Release()
        {
            LibVlcMethods.libvlc_media_library_release(_mHMediaLib);
        }

        public IntPtr Pointer
        {
            get 
            {
                return _mHMediaLib;
            }
        }
    }
}
