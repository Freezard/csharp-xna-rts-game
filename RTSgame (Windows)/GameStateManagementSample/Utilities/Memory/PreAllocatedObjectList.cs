using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RTSgame.Utilities.Memory
{

    /// <summary>
    /// List for pre allocated objects.
    /// Every object that wants to use this
    /// needs to have a parameterless constructor,
    /// and all it's parents up to AlwaysAllocatedObject
    /// need to have parameterless constructors too,
    /// and all this constructors need to call their
    /// parents parameterless constructor.
    /// Can hold up to 64k objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PreAllocatedObjectList<T> where T : AlwaysAllocatedObject, new()
    {
        private T[] data;

        private BitArray occupied;

        private int currentPointer = 0;

        public PreAllocatedObjectList(int Size)
        {
            data = new T[Size];

            for (int i = 0; i < Size; i++)
            {
                data[i] = new T();
                data[i].SetMemoryId((UInt16) i);
                
            }

            occupied = new BitArray(Size, false);
        }

        /// <summary>
        /// Returns a reference for an unused object to use.
        /// CAUTION! The object may contain old data, make
        /// sure to clean properly.
        /// </summary>
        /// <returns></returns>
        public T GetNewInstance()
        {
            for (int i = currentPointer + 1; i == currentPointer; i++)
            {
                if (i == data.Length)
                    i = 0;

                if (!occupied[i])
                {
                    occupied[i] = true;
                    currentPointer = i;
                    return data[i];
                }
                else
                {
                    continue;
                }
            }

            DebugPrinter.Write("CRITICAL ERROR, PreAllocatedList: " + this + ", ran out of space!");
            currentPointer++;
            if (currentPointer == data.Length)
                currentPointer = 0;
            return data[currentPointer];
        }

        /// <summary>
        /// Sets the object free for others to allocate/use.
        /// Make sure to set any pointer pointing towards this object
        /// to null after calling this, (except the one in the PreAllocatedList).
        /// </summary>
        public void Destroy(T item)
        {
            occupied[item.GetMemoryId()] = false;
        }

        // Make for loop

    }
}
