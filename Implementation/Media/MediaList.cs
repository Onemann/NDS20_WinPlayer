//    nVLC
//    
//    Author:  Roman Ginzburg
//
//    nVLC is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    nVLC is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//    GNU General Public License for more details.
//     
// ========================================================================

using System;
using System.Collections.Generic;
using Declarations;
using Declarations.Events;
using Declarations.Media;
using Implementation.Events;
using LibVlcWrapper;

namespace Implementation.Media
{
   internal class MediaList : DisposableBase, IMediaList, INativePointer, IEventProvider, IReferenceCount
   {
      protected IntPtr MHMediaList;
      protected IntPtr MHMediaLib;
      IntPtr _mHEventManager = IntPtr.Zero;
      IMediaListEvents _mEvents = null;

      public MediaList(IntPtr hMediaLib)
      {
         MHMediaLib = hMediaLib;
         MHMediaList = LibVlcMethods.libvlc_media_list_new(hMediaLib);
      }

      public MediaList(IntPtr hMediaList, ReferenceCountAction action)
      {
         MHMediaList = hMediaList;

         switch (action)
         {
            case ReferenceCountAction.AddRef:
               this.AddRef();
               break;
            case ReferenceCountAction.Release:
               this.Release();
               break;
         }
      }

      protected struct MediaListLock : IDisposable
      {
         IntPtr _mHMediaList;

         public MediaListLock(IntPtr hMediaList)
         {
            _mHMediaList = hMediaList;
            LibVlcMethods.libvlc_media_list_lock(_mHMediaList);
         }

         #region IDisposable Members

         public void Dispose()
         {
            LibVlcMethods.libvlc_media_list_unlock(_mHMediaList);
         }

         #endregion
      }

      #region IList<IMedia> Members

      public int IndexOf(IMedia item)
      {
         using (new MediaListLock(MHMediaList))
         {
            return LibVlcMethods.libvlc_media_list_index_of_item(MHMediaList, ((INativePointer)item).Pointer);
         }
      }

      public void Insert(int index, IMedia item)
      {
         using (new MediaListLock(MHMediaList))
         {
            LibVlcMethods.libvlc_media_list_insert_media(MHMediaList, ((INativePointer)item).Pointer, index);
         }
      }

      public void RemoveAt(int index)
      {
         using (new MediaListLock(MHMediaList))
         {
            LibVlcMethods.libvlc_media_list_remove_index(MHMediaList, index);
         }
      }

      public IMedia this[int index]
      {
         get
         {
            using (new MediaListLock(MHMediaList))
            {
               var hMedia = LibVlcMethods.libvlc_media_list_item_at_index(MHMediaList, index);
               if (hMedia == IntPtr.Zero)
               {
                  throw new Exception(string.Format("Media at index {0} not found", index));
               }

               return new BasicMedia(hMedia, ReferenceCountAction.AddRef);
            }
         }
         set
         {
            this.Insert(index, value);
         }
      }

      #endregion

      #region ICollection<IMedia> Members

      public void Add(IMedia item)
      {
         using (new MediaListLock(MHMediaList))
         {
            LibVlcMethods.libvlc_media_list_add_media(MHMediaList, ((INativePointer)item).Pointer);
         }
      }

      public void Clear()
      {
         using (new MediaListLock(MHMediaList))
         {
            var count = LibVlcMethods.libvlc_media_list_count(MHMediaList);

            for (var i = 0; i < count; i++)
            {
               LibVlcMethods.libvlc_media_list_remove_index(MHMediaList, 0);
            }
         }
      }

      public bool Contains(IMedia item)
      {
         return this.IndexOf(item) != -1;
      }

      public void CopyTo(IMedia[] array, int arrayIndex)
      {
         throw new NotImplementedException();
      }

      public int Count
      {
         get
         {
            using (new MediaListLock(MHMediaList))
            {
               return LibVlcMethods.libvlc_media_list_count(MHMediaList);
            }
         }
      }

      public bool IsReadOnly
      {
         get 
         { 
            return LibVlcMethods.libvlc_media_list_is_readonly(MHMediaList) == 0; 
         }
      }

      public bool Remove(IMedia item)
      {
         var index = this.IndexOf(item);
         if (index < 0)
         {
            return false;
         }

         this.RemoveAt(index);
         return true;
      }

      #endregion

      #region IEnumerable<IMedia> Members

      public IEnumerator<IMedia> GetEnumerator()
      {
         throw new NotImplementedException();
      }

      #endregion

      #region IEnumerable Members

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
         return this.GetEnumerator();
      }

      #endregion

      protected override void Dispose(bool disposing)
      {
         Release();
      }

      #region INativePointer Members

      public IntPtr Pointer
      {
         get 
         { 
            return MHMediaList; 
         }
      }

      #endregion

      #region IEventProvider Members

      public IntPtr EventManagerHandle
      {
         get 
         {
            if (_mHEventManager == IntPtr.Zero)
            {
               _mHEventManager = LibVlcMethods.libvlc_media_list_event_manager(MHMediaList);
            }

            return _mHEventManager;
         }
      }

      #endregion

      #region IMediaList Members

      public IMediaListEvents Events
      {
         get 
         {
            if (_mEvents == null)
            {
               _mEvents = new MediaListEventManager(this);
            }
            return _mEvents;
         }
      }

      #endregion

      #region IReferenceCount Members

      public void AddRef()
      {
         LibVlcMethods.libvlc_media_list_retain(MHMediaList);
      }

      public void Release()
      {
         try
         {
            LibVlcMethods.libvlc_media_list_release(MHMediaList);
         }
         catch (AccessViolationException)
         { }
      }

      #endregion
   }
}
