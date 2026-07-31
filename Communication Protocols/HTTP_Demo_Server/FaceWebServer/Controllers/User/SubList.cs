using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceProtocolServer.Controllers.User
{
    public class SubListEnumerator<T> : IEnumerator<T>
    {
        private SubList<T> Sublist { get; set; }
        private int _CurrentIndex;

        public SubListEnumerator(SubList<T> oList)
        {
            Sublist = oList;
            _CurrentIndex = oList.BeginIndex - 1;
        }

        public T Current => Sublist[_CurrentIndex];



        object IEnumerator.Current => Sublist[_CurrentIndex];

        public void Dispose()
        {
            Sublist = null;
            return;
        }

        public bool MoveNext()
        {
            int iMax = Sublist.Count + Sublist.BeginIndex - 1;
            if (_CurrentIndex >= iMax) return false;
            _CurrentIndex++;
            return true;
        }

        public void Reset()
        {
            _CurrentIndex = Sublist.BeginIndex;
        }
    }


    public class SubList<T> : IEnumerable<T>, IList<T>
    {

        public int Count { get; private set; }
        public int BeginIndex { get; private set; }
        public List<T> SrcList { get; private set; }

        //public string TaskName { get; private set; }
        public bool IsReadOnly => true;

        public T this[int index] { get => SrcList[index]; set => throw new NotImplementedException(); }

        public SubList(List<T> oList, int iBegin, int iCount)
        {
            Count = iCount;
            BeginIndex = iBegin;
            SrcList = oList;

        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SubListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SubListEnumerator<T>(this);
        }

        public int IndexOf(T item)
        {
            return SrcList.IndexOf(item);
        }

        public bool Contains(T item)
        {
            return SrcList.Contains(item);
        }

        #region 不实现的部分
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
