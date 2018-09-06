using System;
using System.Collections.Generic;
namespace SharpVectors.Dom.Svg
    /// Note we're using <see cref="List{T}"/> (as opposed to deriving from) to hide unneeded <see cref="List{T}"/> methods
	public abstract class SvgList<T> : IEnumerable<T>

        protected List<T> _items;
        #region Constructor
            get { return (uint) _items.Count; }
        }

        /// <summary>
        public void Clear()
        {
            // Note that we cannot use List<T>'s Clear method since we need to
            // remove all items from the itemOwnerMap
            while (_items.Count > 0) 
                RemoveItem(0);
        }

        /// <summary>
        public T Initialize(T newItem)
        {
            Clear();
            return AppendItem(newItem);
        }

        /// <summary>
        public T GetItem(uint index)
        {
            if ( index < 0 || _items.Count <= index )
                throw new DomException(DomExceptionType.IndexSizeErr);

            return _items[(int)index];
        }

        /// <summary>
        public T InsertItemBefore(T newItem, uint index)
        {
            if ( index < 0 || _items.Count <= index )
                throw new DomException(DomExceptionType.IndexSizeErr);

            // cache cast
            int i = (int) index;

            // if newItem exists in a list, remove it from that list
            if (_itemOwnerMap.ContainsKey(newItem) )
                _itemOwnerMap[newItem].RemoveItem(newItem);

            // insert item into this list
            _items.Insert(i, newItem);

            // update the itemOwnerMap to associate newItem with this list
            _itemOwnerMap[newItem] = this;

            return _items[i];
        }

        /// <summary>
        public T ReplaceItem(T newItem, uint index)
        {
            if ( index < 0 || _items.Count <= index )
                throw new DomException(DomExceptionType.IndexSizeErr);

            // cache cast
            int i = (int) index;

            // if newItem exists in a list, remove it from that list
            if (_itemOwnerMap.ContainsKey(newItem))
                _itemOwnerMap[newItem].RemoveItem(newItem);

            // remove oldItem from itemOwnerMap
            _itemOwnerMap.Remove(_items[i]);

            // update the itemOwnerMap to associate newItem with this list
            _itemOwnerMap[newItem] = this;

            // store newItem and return
            return _items[i] = newItem;
        }

        /// <summary>
        public T RemoveItem(uint index)
        {
            if ( index < 0 || _items.Count <= index )
                throw new DomException(DomExceptionType.IndexSizeErr);

            // cache cast
            int i = (int)index;

            // save removed item so we can return it
            T result = _items[i];

            // item is longer associated with this list, so remove item from itemOwnerMap
            _itemOwnerMap.Remove(result);

            // remove item from this list
            _items.RemoveAt(i);

            // return removed item
            return result;
        }

        /// <summary>
        public T AppendItem(T newItem)
            if (_itemOwnerMap.ContainsKey(newItem))
                _itemOwnerMap[newItem].RemoveItem(newItem);
            // update the itemOwnerMap to associate newItem with this list
            _itemOwnerMap[newItem] = this;
            // add item and return
            return newItem;
        #region IEnumerable Interface
            return _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

            int index = _items.IndexOf(item);
                this.RemoveItem((uint)index);
    }